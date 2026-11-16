namespace RočníkováPráce
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            oscillator1 = new Oscillator();
            oscillator2 = new Oscillator();
            oscillator3 = new Oscillator();
            envelope1 = new Envelope();
            SuspendLayout();
            // 
            // oscillator1
            // 
            oscillator1.Location = new Point(12, 12);
            oscillator1.Name = "oscillator1";
            oscillator1.Size = new Size(300, 103);
            oscillator1.TabIndex = 0;
            oscillator1.TabStop = false;
            oscillator1.Text = "oscillator1";
            oscillator1.Enter += oscillator1_Enter_1;
            // 
            // oscillator2
            // 
            oscillator2.Location = new Point(12, 121);
            oscillator2.Name = "oscillator2";
            oscillator2.Size = new Size(300, 103);
            oscillator2.TabIndex = 6;
            oscillator2.TabStop = false;
            oscillator2.Text = "oscillator2";
            // 
            // oscillator3
            // 
            oscillator3.Location = new Point(12, 230);
            oscillator3.Name = "oscillator3";
            oscillator3.Size = new Size(300, 103);
            oscillator3.TabIndex = 6;
            oscillator3.TabStop = false;
            oscillator3.Text = "oscillator3";
            // 
            // envelope1
            // 
            envelope1.Location = new Point(358, 15);
            envelope1.Name = "envelope1";
            envelope1.Size = new Size(300, 100);
            envelope1.TabIndex = 7;
            envelope1.TabStop = false;
            envelope1.Text = "envelope1";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1348, 675);
            Controls.Add(envelope1);
            Controls.Add(oscillator3);
            Controls.Add(oscillator2);
            Controls.Add(oscillator1);
            Name = "Form1";
            Text = "BasicSynth";
            KeyDown += Form1_KeyDown;
            ResumeLayout(false);
        }

        #endregion

        private Oscillator oscillator1;
        private Oscillator oscillator2;
        private Oscillator oscillator3;
        private Envelope envelope1;
    }
}
