using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Diagnostics;
using System.Media;
using static RočníkováPráce.SynthWaveProvider;
namespace RočníkováPráce
{
    public partial class Form1 : Form
    {
        private IWavePlayer waveOut;
        private SynthWaveProvider synthWaveProvider;
        private const int SampleRate = 44100;
        private const short BitsPerSample = 16;
        private HashSet<Keys> KeysHeld = new HashSet<Keys>();
        private Keys ActiveKey = Keys.None;
        private readonly Stopwatch TimeSinceKeyPress = new Stopwatch();
        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyUp += Form1_KeyUp;
            synthWaveProvider = new SynthWaveProvider(130.81f, SampleRate);
            waveOut = new WaveOut(); //NAudio output device
            waveOut.Init(synthWaveProvider);
            waveOut.Play();

        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            IEnumerable<Oscillator> oscillators = this.Controls.OfType<Oscillator>().Where(o => o.On);
            if (KeysHeld.Contains(e.KeyCode))
            {
                return;
            }
            ActiveKey = e.KeyCode;
            KeysHeld.Add(e.KeyCode);
            TimeSinceKeyPress.Restart();
            int semitoneFromRoot = 0;
            switch (e.KeyCode)
            {
                case Keys.Z:
                    semitoneFromRoot = 0;
                    break;
                case Keys.S:
                    semitoneFromRoot = 1;
                    break;
                case Keys.X:
                    semitoneFromRoot = 2;
                    break;
                case Keys.D:
                    semitoneFromRoot = 3;
                    break;
                case Keys.C:
                    semitoneFromRoot = 4;
                    break;
                case Keys.V:
                    semitoneFromRoot = 5;
                    break;
                case Keys.G:
                    semitoneFromRoot = 6;
                    break;
                case Keys.B:
                    semitoneFromRoot = 7;
                    break;
                case Keys.H:
                    semitoneFromRoot = 8;
                    break;
                case Keys.N:
                    semitoneFromRoot = 9;
                    break;
                case Keys.J:
                    semitoneFromRoot = 10;
                    break;
                case Keys.M:
                    semitoneFromRoot = 11;
                    break;
                //second octave
                case Keys.Oemcomma:
                    semitoneFromRoot = 12;
                    break;
                case Keys.Q:
                    semitoneFromRoot = 12;
                    break;
                case Keys.D2:
                    semitoneFromRoot = 13;
                    break;
                case Keys.W:
                    semitoneFromRoot = 14;
                    break;
                case Keys.D3:
                    semitoneFromRoot = 15;
                    break;
                case Keys.E:
                    semitoneFromRoot = 16;
                    break;
                case Keys.R:
                    semitoneFromRoot = 17;
                    break;
                case Keys.D5:
                    semitoneFromRoot = 18;
                    break;
                case Keys.T:
                    semitoneFromRoot = 19;
                    break;
                case Keys.D6:
                    semitoneFromRoot = 20;
                    break;
                case Keys.Y:
                    semitoneFromRoot = 21;
                    break;
                case Keys.D7:
                    semitoneFromRoot = 22;
                    break;
                case Keys.U:
                    semitoneFromRoot = 23;
                    break;
                case Keys.I:
                    semitoneFromRoot = 24;
                    break;
                default: return;
            }
            float rootFrequency = 130.81f;
            float frequency = (float)(rootFrequency * Math.Pow(2, semitoneFromRoot / 12f));
            synthWaveProvider.NoteOn(frequency);

            var activeOscControls = this.Controls.OfType<Oscillator>().Where(o => o.On);
            var layers = new List<OscillatorLayer>();
            foreach (var osc in activeOscControls)
            {
                layers.Add(new OscillatorLayer
                {
                    WaveForm = osc.WaveForm,
                    Volume = Convert.ToInt32(osc.Volume),
                    PitchOffset = osc.Pitch
                });
            }
            var envSettings = new EnvelopeSettings
            {
                Attack = envelope1.Attack,
                Decay = envelope1.Decay,
                Sustain = envelope1.Sustain,
                Release = envelope1.Release,
                Delay = envelope1.Delay
            };
            synthWaveProvider.UpdateEnvelopeSettings(envSettings);
            synthWaveProvider.UpdateOscillators(layers);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            KeysHeld.Remove(e.KeyCode);
            if (e.KeyCode == ActiveKey)
            {
                ActiveKey = Keys.None;
                synthWaveProvider.NoteOff();
            }


        }
        private void oscillator1_Enter(object sender, EventArgs e)
        {

        }
        private void oscillator1_Enter_1(object sender, EventArgs e)
        {

        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
