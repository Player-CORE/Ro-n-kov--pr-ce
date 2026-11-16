using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Devices;
using NAudio.SoundFont;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RočníkováPráce
{
    public class SynthWaveProvider : IWaveProvider
    {

        
        private readonly WaveFormat waveFormat;
        private double frequency;
        private readonly float masterAmplitude;
        public WaveForm WaveForm { get; private set; }

        private List<OscillatorLayer> activeOscillators = new List<OscillatorLayer>();
        private EnvelopeSettings currentEnvelope = new EnvelopeSettings { Attack = 100, Decay = 0, Sustain = 50, Release = 100, Delay = 0 };
        private object lockObject = new object();
        private long sampleCounter = 0;
        private long releaseSampleCounter = 0;
        private bool isPlaying = false;
        private bool isReleasing = false;
        private float releaseStartLevel = 0f;
        private float lastEnvLevel = 0f;
        private readonly Random rnd = new Random();

        public void NoteOn(float newFrequency) 
        {
            lock (lockObject)
            {
                frequency = newFrequency;
                sampleCounter = 0;
                releaseSampleCounter = 0;
                isPlaying = true;
                isReleasing = false;
            }
        }
        public void NoteOff()
        {
            lock (lockObject)
            {
                if(isPlaying && !isReleasing)
                {
                    isReleasing= true;
                    releaseSampleCounter= 0;
                    releaseStartLevel = lastEnvLevel;

                }
            }
        }

        public SynthWaveProvider(double initialFrequency = 0f, int samplerate = 44100)
        {
            waveFormat = new WaveFormat(samplerate, 16, 1); // bytes and channels
            this.frequency = initialFrequency;
            masterAmplitude = short.MaxValue * 0.25f;
        }
        public WaveFormat WaveFormat
        {
            get { return waveFormat; }
        }
        public struct EnvelopeSettings
        {
            public float Attack;   // in ms
            public float Decay;    // in ms
            public float Sustain;  // percentage (0-100)
            public float Release;  // in ms
            public float Delay; // in ms
        }
        public float EnvelopeValue(float time, float releaseTime, EnvelopeSettings envSettings, bool isRelease, float startReleaseLevel) //in ms since key pressed
        {
            float attack = envSettings.Attack; //100
            float decay = envSettings.Decay; //0
            float sustain = envSettings.Sustain; //50%
            float release = envSettings.Release; //100
            float delay = envSettings.Delay; //0
            
            if (isRelease)
            {
                if (envSettings.Release <= 0) return 0f;
                return Math.Max(0f, startReleaseLevel * (1f - releaseTime / release));
            }
            if (time <= delay)
            {
                return 0f;
            }
            else if (time <= attack + delay)
            {
                if (attack == 0)
                {
                    return 1;
                }
                return (time - delay) / attack;
            }
            else if (time <= attack + decay + delay)
            {
                if (decay <= 0) return sustain / 100;
                return 1 - (((time - attack - delay) / decay) * (1 - sustain / 100));
            }
            else
            {
                return sustain / 100;
            }
        }
        public void UpdateOscillators(IEnumerable<OscillatorLayer> oscillators) 
        {
            lock (lockObject) 
            {
                activeOscillators = new List<OscillatorLayer>(oscillators);
            }
        }
        public void UpdateEnvelopeSettings(EnvelopeSettings envSettings)
        {
            lock (lockObject)
            {
                currentEnvelope = envSettings;
            }
        }
        static double PolyBlep(double t, double dt)  //anti aliasing 
        {
            if (t < dt)
            {
                t /= dt;
                return t + t - t * t - 1;
            }
            else if (t > 1 - dt)
            {
                t = (t - 1) / dt;

                return t + t + t * t + 1;
            }
            else
            {
                return 0;
            }
        }
        public int Read(Span<Byte> buffer)
        {
            List<OscillatorLayer> oscillatorsSnapshot;
            EnvelopeSettings envSnapshot;
            bool playingSnapshot;
            bool releasingSnapshot;
            float releaseStartSnapshot;

            lock (lockObject)
            {
                oscillatorsSnapshot = new List<OscillatorLayer>(activeOscillators);
                envSnapshot = currentEnvelope;
                playingSnapshot = isPlaying;
                releasingSnapshot = isReleasing;
                releaseStartSnapshot = releaseStartLevel;
            }
            Span<short> sampleSpan = MemoryMarshal.Cast<byte, short>(buffer); //Way to write short values but have them read as bytes
            if (!playingSnapshot)
            {
                sampleSpan.Clear();
                return buffer.Length;   
            }
            for (int i = 0; i < sampleSpan.Length; i++)
            {
                double mixedSample = 0;
                float timeMs = (float)(sampleCounter * 1000.0 / waveFormat.SampleRate);
                float releaseTimeMs = (float)(releaseSampleCounter * 1000.0 / waveFormat.SampleRate);
                float envMult = EnvelopeValue(timeMs, releaseTimeMs, envSnapshot, releasingSnapshot, releaseStartSnapshot);
                lastEnvLevel = envMult;
                if (releasingSnapshot && envMult <=0f)
                {
                    isReleasing = false;
                    isPlaying = false;
                    sampleSpan.Slice(i).Clear(); //fill the rest of the audio buffer with 0
                    break;
                }

                foreach (var osc in oscillatorsSnapshot)
                {
                    double sampleValue = 0;
                    double freq = frequency * Math.Pow(2, osc.PitchOffset / 12.0);
                    double dt = freq / waveFormat.SampleRate;

                    switch (osc.WaveForm)
                    {
                        case WaveForm.Sine:
                            sampleValue = Math.Sin(Math.PI * 2 * osc.Phase);
                            break;
                        case WaveForm.Triangle:
                            if (osc.Phase < 0.5)
                            {
                                sampleValue = 2 * 2 * osc.Phase - 1; //linear rise in half the phase
                            }
                            else
                            {
                                sampleValue = 3 - (2 * 2 * osc.Phase); //linear fall in half the phase
                            }
                            break;
                        case WaveForm.Saw:
                            sampleValue = 2.0 * osc.Phase - 1.0; //linear rise from 0-2 minus 1 - from -1 to 1
                            sampleValue -= PolyBlep(osc.Phase, dt);
                            break;
                        case WaveForm.Square:
                            sampleValue = (osc.Phase < 0.5) ? 1.0 : -1.0;
                            sampleValue += PolyBlep(osc.Phase, dt);
                            sampleValue -= PolyBlep((osc.Phase + 0.5) % 1, dt);
                            break;
                        case WaveForm.Noise:
                            sampleValue = rnd.NextDouble() * 2.0 - 1.0;
                            break;
                    }
                    sampleValue *= envMult;
                    double volumeMultiplier = osc.Volume / 100.0;
                    mixedSample += sampleValue * volumeMultiplier;
                    osc.Phase += dt;
                    if (osc.Phase >= 1.0)
                    {
                        osc.Phase -= 1.0;
                    }
                }
                sampleSpan[i] = (short)Math.Clamp(mixedSample * masterAmplitude, short.MinValue, short.MaxValue);
                if (releasingSnapshot) 
                {
                    releaseSampleCounter++;
                }
                else
                {
                    sampleCounter++;
                }
            }
            return buffer.Length;
        }
    }
    public class OscillatorLayer
    {       
        public WaveForm WaveForm { get; set; }
        public int Volume { get; set; }
        public int PitchOffset {  get; set; }
        public double Phase { get; set; }

    }
    
    public enum WaveForm
    {
        Sine,
        Triangle,
        Saw,
        Square,
        Noise
    }
}
