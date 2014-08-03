namespace WindowsFormsApplication1
{
    partial class Form3
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button4 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.button11 = new System.Windows.Forms.Button();
            this.slider3 = new System.Windows.Forms.TrackBar();
            this.slider2 = new System.Windows.Forms.TrackBar();
            this.button12 = new System.Windows.Forms.Button();
            this.slider1 = new System.Windows.Forms.TrackBar();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.slider3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.slider2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.slider1)).BeginInit();
            this.SuspendLayout();
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(314, 345);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(60, 23);
            this.button4.TabIndex = 38;
            this.button4.Text = "Zamknij";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 359);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 13);
            this.label4.TabIndex = 37;
            this.label4.Text = "Z";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 332);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 13);
            this.label5.TabIndex = 36;
            this.label5.Text = "Y";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 306);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(14, 13);
            this.label6.TabIndex = 35;
            this.label6.Text = "X";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(35, 356);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(46, 20);
            this.textBox4.TabIndex = 34;
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(35, 329);
            this.textBox5.Name = "textBox5";
            this.textBox5.ReadOnly = true;
            this.textBox5.Size = new System.Drawing.Size(46, 20);
            this.textBox5.TabIndex = 33;
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(35, 303);
            this.textBox6.Name = "textBox6";
            this.textBox6.ReadOnly = true;
            this.textBox6.Size = new System.Drawing.Size(46, 20);
            this.textBox6.TabIndex = 32;
            // 
            // button11
            // 
            this.button11.Enabled = false;
            this.button11.Location = new System.Drawing.Point(223, 343);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(75, 23);
            this.button11.TabIndex = 31;
            this.button11.Text = "Rozłącz";
            this.button11.UseVisualStyleBackColor = true;
            // 
            // slider3
            // 
            this.slider3.Location = new System.Drawing.Point(283, 303);
            this.slider3.Maximum = 180;
            this.slider3.Minimum = -180;
            this.slider3.Name = "slider3";
            this.slider3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.slider3.Size = new System.Drawing.Size(147, 45);
            this.slider3.TabIndex = 29;
            this.slider3.Scroll += new System.EventHandler(this.slider3_Scroll);
            // 
            // slider2
            // 
            this.slider2.Location = new System.Drawing.Point(87, 306);
            this.slider2.Maximum = 180;
            this.slider2.Minimum = -180;
            this.slider2.Name = "slider2";
            this.slider2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.slider2.RightToLeftLayout = true;
            this.slider2.Size = new System.Drawing.Size(190, 45);
            this.slider2.TabIndex = 28;
            this.slider2.Scroll += new System.EventHandler(this.slider2_Scroll);
            // 
            // button12
            // 
            this.button12.BackColor = System.Drawing.Color.Peru;
            this.button12.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.button12.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button12.ForeColor = System.Drawing.Color.Black;
            this.button12.Location = new System.Drawing.Point(35, 394);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(45, 33);
            this.button12.TabIndex = 27;
            this.button12.Text = "Reset";
            this.button12.UseVisualStyleBackColor = false;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // slider1
            // 
            this.slider1.Location = new System.Drawing.Point(456, 21);
            this.slider1.Maximum = 90;
            this.slider1.Minimum = -90;
            this.slider1.Name = "slider1";
            this.slider1.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.slider1.Size = new System.Drawing.Size(45, 190);
            this.slider1.TabIndex = 26;
            this.slider1.Scroll += new System.EventHandler(this.slider1_Scroll);
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(641, 454);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.slider3);
            this.Controls.Add(this.slider2);
            this.Controls.Add(this.button12);
            this.Controls.Add(this.slider1);
            this.Name = "Form3";
            this.Text = "Form3";
            this.Load += new System.EventHandler(this.Form3_Load);
            ((System.ComponentModel.ISupportInitialize)(this.slider3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.slider2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.slider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.TrackBar slider3;
        private System.Windows.Forms.TrackBar slider2;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.TrackBar slider1;
        private System.IO.Ports.SerialPort serialPort1;
    }
}