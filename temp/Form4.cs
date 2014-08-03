using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;


namespace WindowsFormsApplication1
{
    public partial class Form4 : Form
    {

        //SerialPort port;
        SerialPort port = new SerialPort();

        string comPort, RxString;
        string[] ArduinoData = null;
        string[] ports = SerialPort.GetPortNames();

        // Load images
        Bitmap mybitmap1 = new Bitmap(WindowsFormsApplication1.Properties.Resources.kompas);
        Bitmap mybitmap2 = new Bitmap(WindowsFormsApplication1.Properties.Resources.ramka);
        Bitmap mybitmap3 = new Bitmap(WindowsFormsApplication1.Properties.Resources.pozycja_kata);
        Bitmap mybitmap4 = new Bitmap(WindowsFormsApplication1.Properties.Resources.znacznik);

        //Pen myPen;

        double PitchAngle = 0;
        double RollAngle = 0;
        double YawAngle = 0;

        Point ptBoule = new Point(-25, -410); //Ground-Sky initial location
        Point ptHeading = new Point(-592, 150); // Heading ticks
        Point ptRotation = new Point(150, 150); // Point of rotation


        Form1 F1 = new Form1();
        public Form4()
        {
            InitializeComponent();

            // Populates the comboBox with available COM ports
            foreach (string s in ports) comboBox1.Items.Add(s);

            // Serial port handler
            port.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);

            // Keyboard handler
          //  this.KeyPress += new KeyPressEventHandler(Form1_KeyDown);

            // This bit of code (using double buffer) reduces flicker from Refresh commands
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            //////////// END "reduce flicker" code ///////

        }

        
        // Reset button
        private void button1_Click(object sender, EventArgs e)
        {
            PitchAngle = 0;
            RollAngle = 0;
            YawAngle = 0;
            slider1.Value = 0;
            slider2.Value = 0;
            slider3.Value = 0;

            Invalidate();

        }


        // OnPaint takes care of drawing all graphics to the screen automatically
        protected override void OnPaint(PaintEventArgs paintEvnt)
        {
            // Calling the base class OnPaint
            base.OnPaint(paintEvnt);


            // Clipping mask for Attitude Indicator
            paintEvnt.Graphics.Clip = new Region(new Rectangle(0, 0, 300, 300));
            paintEvnt.Graphics.FillRegion(Brushes.Black, paintEvnt.Graphics.Clip);


            // Make sure lines are drawn smoothly
            paintEvnt.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            // Create the graphics object
            Graphics gfx = paintEvnt.Graphics;

            // Adjust and draw horizon image
            RotateAndTranslate(paintEvnt, mybitmap1, RollAngle, 0, ptBoule, (double)(4 * PitchAngle), ptRotation, 1);

            RotateAndTranslate2(paintEvnt, mybitmap3, YawAngle, RollAngle, 0, ptHeading, (double)(4 * PitchAngle), ptRotation, 1);



            // Draw a mask
            //Pen maskPen = new Pen(this.BackColor, 220); // width of mask
            //gfx.DrawRectangle(maskPen, -100, -100, 500, 500); // size of mask

            gfx.DrawImage(mybitmap2, 0, 0); // Draw bezel image
            gfx.DrawImage(mybitmap4, 75, 125); // Draw wings image

            //myPen = new Pen(System.Drawing.Color.Green, 3);
            //gfx.DrawLine(myPen, 200, 20, 20, 210); // Draw a line

            // The sliders are updated from the Pitch, Roll & Yaw values (calculated first from serial data)
            //slider1.Value = Convert.ToInt16(PitchAngle); //Update sliders
            //slider2.Value = Convert.ToInt16((-1.0 * RollAngle / Math.PI) * 180);  //with values from
            //slider3.Value = Convert.ToInt16((YawAngle / Math.PI) * 180);   //serial data (if available)           

            // Update text boxes with angles (if we have any)

            if (port.IsOpen == false) // Use slider values if port is closed
            {
                textBox1.Text = Convert.ToString(slider1.Value);
                textBox2.Text = Convert.ToString(slider2.Value);
                textBox3.Text = Convert.ToString(slider3.Value);
            }

            if (ArduinoData != null && port.IsOpen == true) // Use Arduino data if port is open
            {
                textBox1.Text = ArduinoData[0];
                textBox2.Text = ArduinoData[1];
                textBox3.Text = ArduinoData[2];
            }

        }


        


        protected void RotateAndTranslate(PaintEventArgs pe, Image img, Double alphaRot, Double alphaTrs, Point ptImg, double deltaPx, Point ptRot, float scaleFactor)
        {
            double beta = 0;
            double d = 0;
            float deltaXRot = 0;
            float deltaYRot = 0;
            float deltaXTrs = 0;
            float deltaYTrs = 0;

            // Rotation

            if (ptImg != ptRot)
            {
                // Internals coeffs
                if (ptRot.X != 0)
                {
                    beta = Math.Atan((double)ptRot.Y / (double)ptRot.X);
                }

                d = Math.Sqrt((ptRot.X * ptRot.X) + (ptRot.Y * ptRot.Y));

                // Computed offset
                deltaXRot = (float)(d * (Math.Cos(alphaRot - beta) - Math.Cos(alphaRot) * Math.Cos(alphaRot + beta) - Math.Sin(alphaRot) * Math.Sin(alphaRot + beta)));
                deltaYRot = (float)(d * (Math.Sin(beta - alphaRot) + Math.Sin(alphaRot) * Math.Cos(alphaRot + beta) - Math.Cos(alphaRot) * Math.Sin(alphaRot + beta)));
            }

            // Translation

            // Computed offset
            deltaXTrs = (float)(deltaPx * (Math.Sin(alphaTrs)));
            deltaYTrs = (float)(-deltaPx * (-Math.Cos(alphaTrs)));

            // Rotate image support
            pe.Graphics.RotateTransform((float)(alphaRot * 180 / Math.PI));

            // Dispay image
            pe.Graphics.DrawImage(img, (ptImg.X + deltaXRot + deltaXTrs) * scaleFactor, (ptImg.Y + deltaYRot + deltaYTrs) * scaleFactor, img.Width * scaleFactor, img.Height * scaleFactor);

            // Put image support as found
            pe.Graphics.RotateTransform((float)(-alphaRot * 180 / Math.PI));
        }

        protected void RotateAndTranslate2(PaintEventArgs pe, Image img, Double yawRot, Double alphaRot, Double alphaTrs, Point ptImg, double deltaPx, Point ptRot, float scaleFactor)
        {
            double beta = 0;
            double d = 0;
            float deltaXRot = 0;
            float deltaYRot = 0;
            float deltaXTrs = 0;
            float deltaYTrs = 0;

            // Rotation

            if (ptImg != ptRot)
            {
                // Internals coeffs
                if (ptRot.X != 0)
                {
                    beta = Math.Atan((double)ptRot.Y / (double)ptRot.X);
                }

                d = Math.Sqrt((ptRot.X * ptRot.X) + (ptRot.Y * ptRot.Y));

                // Computed offset
                deltaXRot = (float)(d * (Math.Cos(alphaRot - beta) - Math.Cos(alphaRot) * Math.Cos(alphaRot + beta) - Math.Sin(alphaRot) * Math.Sin(alphaRot + beta) + yawRot));
                deltaYRot = (float)(d * (Math.Sin(beta - alphaRot) + Math.Sin(alphaRot) * Math.Cos(alphaRot + beta) - Math.Cos(alphaRot) * Math.Sin(alphaRot + beta)));
            }

            // Translation

            // Computed offset
            deltaXTrs = (float)(deltaPx * (Math.Sin(alphaTrs)));
            deltaYTrs = (float)(-deltaPx * (-Math.Cos(alphaTrs)));

            // Rotate image support
            pe.Graphics.RotateTransform((float)(alphaRot * 180 / Math.PI));

            // Dispay image
            pe.Graphics.DrawImage(img, (ptImg.X + deltaXRot + deltaXTrs) * scaleFactor, (ptImg.Y + deltaYRot + deltaYTrs) * scaleFactor, img.Width * scaleFactor, img.Height * scaleFactor);

            // Put image support as found
            pe.Graphics.RotateTransform((float)(-alphaRot * 180 / Math.PI));
        }

       


        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {

            if (port.IsOpen == true)
            {
                RxString = port.ReadLine();
                ArduinoData = RxString.Split(',', '\n', '\r');

                if (ArduinoData.Count() == 4) //ensures we have all data, plus line end ("\n" or "\r")
                {


                    /*
                    
                    PitchAngle = Convert.ToDouble(ArduinoData[0]);
                    RollAngle = -1.0 * Convert.ToDouble(ArduinoData[1]) * Math.PI / 180;
                    YawAngle = -1.0 * Convert.ToDouble(ArduinoData[2]) * Math.PI / 180;
                    if (YawAngle < -Math.PI) YawAngle = YawAngle + Math.PI;
                    Invalidate();
                
                     */
                }
            }
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (port.IsOpen == true) port.Close();

        }

        // Serial port selection --- comboBox
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        // Disconnect serial port
        private void button2_Click(object sender, EventArgs e)
        {
            port.Close();
            comboBox1.Enabled = true;
            comboBox1.Text = "Wybierz port COM...";
            button2.Enabled = false;

            slider1.Enabled = true;
            slider2.Enabled = true;
            slider3.Enabled = true;

            button1.Visible = true;
            Thread.Sleep(25);

        }


        // Refresh comboBox to show any newly added ports
        private void comboBox_Click(object sender, EventArgs e)
        {

        }

        private void buttonAbout_Click(object sender, EventArgs e)
        {
           // AboutBox1 a = new AboutBox1();
            //a.ShowDialog();
        }

        /*private void Form4_KeyDown(object sender, KeyEventArgs e)
        {
            //MessageBox.Show("Key: "+e.KeyChar);
            switch (e.KeyChar)
            {

                case (char)54: // Right (Keypad 6)
                    slider2.Value--;
                    RollAngle = slider2.Value * Math.PI / 180;
                    Refresh();

                    break;

                case (char)52:
                    slider2.Value++;
                    RollAngle = slider2.Value * Math.PI / 180;
                    Refresh();
                    break;

                case (char)56:
                    slider1.Value++;
                    PitchAngle = slider1.Value;
                    Refresh();
                    break;

                case (char)50:
                    slider1.Value--;
                    PitchAngle = slider1.Value;
                    Refresh();
                    break;

                case (char)51:
                    slider3.Value--;
                    YawAngle = slider3.Value*Math.PI / 180;
                    Refresh();
                    break;

                case (char)49:
                    slider3.Value++;
                    YawAngle = slider3.Value * Math.PI / 180;
                    Refresh();
                    break;

                default:
                    return;
            }
        }
        */
       


        private void slider1_Scroll_1(object sender, EventArgs e)
        {
            PitchAngle = slider1.Value;
            RollAngle = slider2.Value * Math.PI / 180;
            YawAngle = (double)slider3.Value * Math.PI / 180;

            Invalidate();
        }

        private void slider2_Scroll(object sender, EventArgs e)
        {

            PitchAngle = slider1.Value;
            RollAngle = slider2.Value * Math.PI / 180;
            YawAngle = (double)slider3.Value * Math.PI / 180;
            Invalidate();
        }

        private void slider3_Scroll(object sender, EventArgs e)
        {
            PitchAngle = slider1.Value;
            RollAngle = slider2.Value * Math.PI / 180;
            YawAngle = (double)slider3.Value * Math.PI / 180;

            Invalidate();

        }


        private void button3_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            mybitmap2.MakeTransparent(Color.Yellow); // Sets image transparency
            mybitmap3.MakeTransparent(Color.Black); // Sets image transparency
            mybitmap4.MakeTransparent(Color.Yellow); // Sets image transparency
        
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            PitchAngle = 0;
            RollAngle = 0;
            YawAngle = 0;
            slider1.Value = 0;
            slider2.Value = 0;
            slider3.Value = 0;

            Invalidate();
        }




    }

}
