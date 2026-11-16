using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace RočníkováPráce
{
    public class Oscillator : GroupBox
    {
        public Oscillator()
        {
            this.Size = new Size(300, 100);
            this.Controls.Add(new Button()
            {
                Name = "Sine",
                Location = new Point(10, 15),
                Text = "Sine",
                BackColor = Color.DarkGray
            });
            this.Controls.Add(new Button()
            {
                Name = "Triangle",
                Location = new Point(65, 15),
                Text = "Triangle"
            });
            this.Controls.Add(new Button()
            {
                Name = "Saw",
                Location = new Point(120, 15),
                Text = "Saw"
            });
            this.Controls.Add(new Button()
            {
                Name = "Square",
                Location = new Point(10, 50),
                Text = "Square"
            });
            this.Controls.Add(new Button()
            {
                Name = "Noise",
                Location = new Point(65, 50),
                Text = "Noise"
            });
            foreach (Control control in this.Controls)
            {
                control.Size = new Size(50, 30);
                control.Font = new Font("Microsoft Sans Serif", 6.75f);
                control.Click += WaveButton_Click; 
            }
            this.Controls.Add(new CheckBox()
            {
                Name = "OscillatorOn",
                Location = new Point(210, 10),
                Size = new Size(50, 30),
                Text = "On",
                Checked = true
            });
            this.Controls.Add(new NumericUpDown()
            {
                Name = "Volume",
                Location = new Point(200, 75),
                Size = new Size(100, 50),
                Value = 70,
                Maximum = 100,
                
            });
            this.Controls.Add(new NumericUpDown()
            {
                Name = "Pitch",
                Location = new Point(200, 50),
                Size = new Size(100, 50),
                Value = 0,
                Maximum = 48,
                Minimum = -48,

            });
            this.Controls.Add(new Label()
            {
                Name = "Volume",
                Location = new Point(150, 78),
                Size = new Size(100, 50),
                Text = "Volume"
            });
            this.Controls.Add(new Label()
            {
                Name = "Pitch",
                Location = new Point(150, 53),
                Size = new Size(100, 50),
                Text = "Pitch"
            });
        }
        public WaveForm WaveForm { get; private set; }
        public bool On => ((CheckBox)this.Controls["OscillatorOn"]).Checked;
        public int Volume => (Int32)((NumericUpDown)this.Controls["Volume"]).Value;
        public int Pitch => (Int32)((NumericUpDown)this.Controls["Pitch"]).Value;
        private void WaveButton_Click(object sender, EventArgs e) // these parameters are required
        {
            Button button = (Button)sender;
            //parse each button with text to the same enum property
            // No try parse, text is the same
            this.WaveForm = (WaveForm)Enum.Parse(typeof(WaveForm), button.Text);
            foreach (Button otherButtons in this.Controls.OfType<Button>())
            {
                otherButtons.UseVisualStyleBackColor = true; //
            }
            button.BackColor = Color.DarkGray;
        }
    }
}