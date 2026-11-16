using System.Media;
namespace Ročníková_práce
{
    public partial class Form1 : Form
    {
        private const int SampleRate = 44100;
        private const short BitsPerSample = 16;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            short[] wave = new short[SampleRate];
            byte[] binaryWave = new byte[SampleRate * sizeof(short)]; //sizeof returns size in bytes(2)
            float frequency = 440f; 
            for (int i = 0; i < SampleRate; i++)
            {
                wave[i] = Convert.ToInt16(short.MaxValue * Math.Sin(((Math.PI * 2 * frequency) / SampleRate) * i));
            }
            Buffer.BlockCopy(wave, 0, binaryWave, 0, wave.Length * sizeof(short));
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                //coverting to wav
                short blockAlign = BitsPerSample/8;
                int subChunkTwoSize = SampleRate * blockAlign;
                bw.Write(new[] { 'R', 'I', 'F', 'F'});
                bw.Write(36 + subChunkTwoSize);
                bw.Write(new[] { 'W', 'A', 'V', 'E','f', 'm', 't', ' ' });
                bw.Write(16);
                bw.Write((short)1);
                bw.Write((short)1);
                bw.Write(SampleRate);
                bw.Write(SampleRate*blockAlign);
                bw.Write(blockAlign);
                bw.Write(BitsPerSample);
                bw.Write(new[] { 'd', 'a', 't', 'a' });
                bw.Write(subChunkTwoSize);
                bw.Write(binaryWave);
                ms.Position = 0;
                new SoundPlayer(ms).Play();
            }
        }
    }
}
