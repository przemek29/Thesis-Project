using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form3 : Form
    {
        public double tab_size = 0;
        // Ładowanie obrazów
        Bitmap mybitmap1 = new Bitmap(WindowsFormsApplication1.Properties.Resources.horyzont);
        Bitmap mybitmap2 = new Bitmap(WindowsFormsApplication1.Properties.Resources.ramka);
        Bitmap mybitmap3 = new Bitmap(WindowsFormsApplication1.Properties.Resources.pozycja_kata);
        Bitmap mybitmap4 = new Bitmap(WindowsFormsApplication1.Properties.Resources.znacznik);
        Bitmap mybitmap5 = new Bitmap(WindowsFormsApplication1.Properties.Resources.kompas);


        //Deklaracja kątów dla żyroskopu;
        double PitchAngle = 0;
        double RollAngle = 0;
        double YawAngle = 0;

        Point ptBoule = new Point(-25, -410); //poczatkowa lokalizacja linia ziemia-niebo
        Point ptHeading = new Point(-592, 150); // pozycja znacznika
        Point ptRotation = new Point(150, 150); // punkt rotacji

        Form1 F1 = new Form1();
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            //ustawianie przejrzystosci obrazu
            mybitmap2.MakeTransparent(Color.Yellow);
            mybitmap3.MakeTransparent(Color.Black);
            mybitmap4.MakeTransparent(Color.Yellow); 
        }

        // OnPaint zajmuje się rysunkami wyświetlając je na formie automatycznie
        protected override void OnPaint(PaintEventArgs paintEvnt)
        {

            // Wywołanie klasy bazowej OnPaint
            base.OnPaint(paintEvnt);


            //Obszar czynny wskaźnika horyzontu
            paintEvnt.Graphics.Clip = new Region(new Rectangle(0, 0, 300, 300));
            paintEvnt.Graphics.FillRegion(Brushes.Black, paintEvnt.Graphics.Clip);


            // Upewnij się, że linie są rysowane płynnie
            paintEvnt.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            // Utworz obiekt graficzny
            Graphics gfx = paintEvnt.Graphics;

            // Reguluj i narysuj horyzont
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



            
            if (serialPort1.IsOpen == false) // Use slider values if port is closed
            {
                textBox6.Text = Convert.ToString(slider1.Value);
                textBox5.Text = Convert.ToString(slider2.Value);
                textBox4.Text = Convert.ToString(slider3.Value);
            }

            if (tab_size != null && serialPort1.IsOpen == true) // Use Arduino data if port is open
            {

                //******************TUTAJ WPISAC WYLICZONE KATY**********************
                //textBox6.Text = ArduinoData[0];
                //textBox5.Text = ArduinoData[1];
                //textBox4.Text = ArduinoData[2];
            
        
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

            // ************Rotacja**************

            if (ptImg != ptRot)
            {

                if (ptRot.X != 0)
                {
                    beta = Math.Atan((double)ptRot.Y / (double)ptRot.X);
                }

                d = Math.Sqrt((ptRot.X * ptRot.X) + (ptRot.Y * ptRot.Y));

                // obliczanie offsetu rotacji
                deltaXRot = (float)(d * (Math.Cos(alphaRot - beta) - Math.Cos(alphaRot) * Math.Cos(alphaRot + beta) - Math.Sin(alphaRot) * Math.Sin(alphaRot + beta)));
                deltaYRot = (float)(d * (Math.Sin(beta - alphaRot) + Math.Sin(alphaRot) * Math.Cos(alphaRot + beta) - Math.Cos(alphaRot) * Math.Sin(alphaRot + beta)));
            }

            //************** Translacja*************

            // obliczanie offsetu translacji
            deltaXTrs = (float)(deltaPx * (Math.Sin(alphaTrs)));
            deltaYTrs = (float)(-deltaPx * (-Math.Cos(alphaTrs)));

            // Rotacja obrazu
            pe.Graphics.RotateTransform((float)(alphaRot * 180 / Math.PI));

            // Wyświetlanie obrazu
            pe.Graphics.DrawImage(img, (ptImg.X + deltaXRot + deltaXTrs) * scaleFactor, (ptImg.Y + deltaYRot + deltaYTrs) * scaleFactor, img.Width * scaleFactor, img.Height * scaleFactor);

            // 
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

            //*************** Rotacja *******************//

            if (ptImg != ptRot)
            {

                if (ptRot.X != 0)
                {
                    beta = Math.Atan((double)ptRot.Y / (double)ptRot.X);
                }

                d = Math.Sqrt((ptRot.X * ptRot.X) + (ptRot.Y * ptRot.Y));

                // Obliczony offset rotacji
                deltaXRot = (float)(d * (Math.Cos(alphaRot - beta) - Math.Cos(alphaRot) * Math.Cos(alphaRot + beta) - Math.Sin(alphaRot) * Math.Sin(alphaRot + beta) + yawRot));
                deltaYRot = (float)(d * (Math.Sin(beta - alphaRot) + Math.Sin(alphaRot) * Math.Cos(alphaRot + beta) - Math.Cos(alphaRot) * Math.Sin(alphaRot + beta)));
            }

            // ********************Tranclacja *******************//

            // Obliczony offset translacji
            deltaXTrs = (float)(deltaPx * (Math.Sin(alphaTrs)));
            deltaYTrs = (float)(-deltaPx * (-Math.Cos(alphaTrs)));

            // Obróc obraz
            pe.Graphics.RotateTransform((float)(alphaRot * 180 / Math.PI));

            // Wyświetl obraz
            pe.Graphics.DrawImage(img, (ptImg.X + deltaXRot + deltaXTrs) * scaleFactor, (ptImg.Y + deltaYRot + deltaYTrs) * scaleFactor, img.Width * scaleFactor, img.Height * scaleFactor);

            // Umieść obraz na formie
            pe.Graphics.RotateTransform((float)(-alphaRot * 180 / Math.PI));
        }
       
       

        
        
        

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }
        //*---------------------Przycisk resetu--------------------*//
        private void button12_Click(object sender, EventArgs e)
        {
            //zerowanie sliderów i kątów przestrznnych
            PitchAngle = 0;
            RollAngle = 0;
            YawAngle = 0;
            slider1.Value = 0;
            slider2.Value = 0;
            slider3.Value = 0;

            Invalidate();
        }
        //**********************SUWAK NR1**********************//
        private void slider1_Scroll(object sender, EventArgs e)
        {

            PitchAngle = slider1.Value;
            RollAngle = slider2.Value * Math.PI / 180;
            YawAngle = (double)slider3.Value * Math.PI / 180;

            Invalidate();

        }
        //**********************SUWAK NR2**********************//
        private void slider2_Scroll(object sender, EventArgs e)
        {

            PitchAngle = slider1.Value;
            RollAngle = slider2.Value * Math.PI / 180;
            YawAngle = (double)slider3.Value * Math.PI / 180;
            Invalidate();

        }
        //**********************SUWAK NR3**********************//
        private void slider3_Scroll(object sender, EventArgs e)
        {

            PitchAngle = slider1.Value;
            RollAngle = slider2.Value * Math.PI / 180;
            YawAngle = (double)slider3.Value * Math.PI / 180;

            Invalidate();
        }
        
       
    
    }

}
