using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RočníkováPráce
{
    public class Envelope : GroupBox
    {
        public Envelope()
        {
            this.Size = new Size(300, 100);
            this.Controls.Add(new NumericUpDown()
            {
                Name = "Attack",
                Location = new Point(25, 75),
                Value = 10,
                Maximum = 10000
            });
            this.Controls.Add(new NumericUpDown()
            {
                Name = "Decay",
                Location = new Point(75, 75),
                Value = 0,
                Maximum = 10000
            });
            this.Controls.Add(new NumericUpDown()
            {
                Name = "Sustain",
                Location = new Point(125, 75),
                Value = 100,
                Maximum = 100
            });
            this.Controls.Add(new NumericUpDown()
            {
                Name = "Release",
                Location = new Point(175, 75),
                Value = 10,
                Maximum = 10000
            });
            this.Controls.Add(new NumericUpDown()
            {
                Name = "Delay",
                Location = new Point(225, 75),
                Value = 0,
                Maximum = 5000
            });
            this.Controls.Add(new Label()
            {
                Name = "Attack",
                Location = new Point(25, 60),
                Text = "Attack"
            });
            this.Controls.Add(new Label()
            {
                Name = "Decay",
                Location = new Point(75, 60),
                Text = "Decay"
            });
            this.Controls.Add(new Label()
            {
                Name = "Sustain",
                Location = new Point(125, 60),
                Text = "Sustain"
            });
            this.Controls.Add(new Label()
            {
                Name = "Release",
                Location = new Point(175, 60),
                Text = "Release"
            });
            this.Controls.Add(new Label()
            {
                Name = "Delay",
                Location = new Point(225, 60),
                Text = "Delay"

            });
            foreach (Control control in this.Controls)
            {
                control.Size = new Size(40, 50);
                control.Font = new Font("Microsoft Sans Serif", 6.75f);
            }
        }
        public int Attack => (Int32)((NumericUpDown)this.Controls["Attack"]).Value;
        public int Decay => (Int32)((NumericUpDown)this.Controls["Decay"]).Value;
        public int Sustain => (Int32)((NumericUpDown)this.Controls["Sustain"]).Value;
        public int Release => (Int32)((NumericUpDown)this.Controls["Release"]).Value;
        public int Delay => (Int32)((NumericUpDown)this.Controls["Delay"]).Value;

    }
}
