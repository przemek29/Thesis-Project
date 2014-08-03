using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO.Ports;
using System.Windows.Threading;
using Domena;

namespace WindowsFormsApplication1
{
    //public interface IDeviceCommunication
    //{
    //    void Initialise();
    //    event SerialDataReceivedEventHandler ReceiveData;
    //    void Terminate();
    //}

    //public class DeviceCommunication : IDeviceCommunication
    //{
    //    private SerialPort _port = new SerialPort();

    //    public void Initialise()
    //    {
    //        _port.Open();
    //        _port.DataReceived += PortOnDataReceived;
    //    }

    //    private void PortOnDataReceived(object sender, SerialDataReceivedEventArgs serialDataReceivedEventArgs)
    //    {
    //        if (ReceiveData != null)
    //            ReceiveData(sender, serialDataReceivedEventArgs);
    //    }

    //    public event SerialDataReceivedEventHandler ReceiveData;

    //    public void Terminate()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //public class FakeCommunitaction : IDeviceCommunication
    //{
    //    public void Initialise()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public event CosTam ReceiveData;
    //    public void Terminate()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    public partial class Form1 : Form
    {
        //private readonly IDeviceCommunication _device;
        private DispatcherTimer timer = new DispatcherTimer();


        Forma F2 = new Forma();
              
        public Image kompas;
        public Image img;
      

        Bitmap mybitmap1 = new Bitmap(WindowsFormsApplication1.Properties.Resources.HeadingIndicator_Background);
        Bitmap mybitmap2 = new Bitmap(WindowsFormsApplication1.Properties.Resources.HeadingWeel);
        Bitmap mybitmap3 = new Bitmap(WindowsFormsApplication1.Properties.Resources.HeadingIndicator_Aircraft);




        public Form1(/*IDeviceCommunication device*/)
       
        {
            //_device = device;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            timer.Tick += TimerOnTick;
            timer.Start();    
            InitializeComponent();

            //_device.ReceiveData += serialPort1_DataReceived;
        }
        /*
        int sekundy = 60;
        int minuty = 0;
        */

        public string[] dane_pomiarowe = new string[7];
        public string[] dane_pomiarowe_poprzednie = new string[7];
        string[] dane_pomiarowe_nowe = new string[7];

        public int kierunek_koniec = 0;
        public int kierunek_poczatek = 0;
        public string odebrany_str = " ";
        public int licznik = 0;
        public int licznik_kalibracji = 0;
        public int licz =0;
        public int tab_size = 0;
        public int index = 0;
        public int i = 0;
        public int count_kompas = 0;
        public int count_giro = 0;
        public int indeks = 0;
        public double temperatura;
        public double kat;
        public double stopnie;

        public double x_komp;
        public double y_komp;
        public double z_komp;

        public double x_giro;
        public double y_giro;
        public double z_giro;
        //*************************************
        //ZMIENNE DO FILTRACJI KALMANA KOMPASU
        //*************************************
        public double x_est_ostanie = 1; // wartość początkowa
        public double P_last =10;// 0.0001;       //wartość macierzy błędu filtracji
        public double Q = 0.00001;       //wartość zakłóceń procesu
        public double R = 0.01;//9;         //wartość macierzy błędów pomiarowych
        public double K_ost=1;                //macierz wzmocnień wagowych Kalmana
        public double K;                //macierz wzmocnień wagowych Kalmana kompasu
        public double P;                //macierz błędu filtracji
        public double P_temp;           //
        public double x_temp_est;
        public double x_est;
        public double z_pomiarowy;       //pomiar wraz z szumem
        public double z_real = 1;       //idealna wartość której poszukujemy
        //********************************************************************************
      
        //*************************************
        //ZMIENNE DO FILTRACJI KALMANA ŻYROSKOPU
        //*************************************
        public double x_giro_est_ostanie = 1; // wartość początkowa
        public double y_giro_est_ostanie = 1; // wartość początkowa
        public double z_giro_est_ostanie = 1; // wartość początkowa

        public double Px_giro_last = 10;// 0.0001;       //wartość macierzy błędu filtracji
        public double Py_giro_last = 10;// 0.0001;       //wartość macierzy błędu filtracji
        public double Pz_giro_last = 10;// 0.0001;       //wartość macierzy błędu filtracji

        public double Qx_giro = 0.000001;       //wartość zakłóceń procesu
        public double Qy_giro = 0.000001;       //wartość zakłóceń procesu
        public double Qz_giro = 0.000001;       //wartość zakłóceń procesu

        public double Rx_giro = 0.01;//9;         //wartość macierzy błędów pomiarowych
        public double Ry_giro = 0.01;//9;         //wartość macierzy błędów pomiarowych
        public double Rz_giro = 0.01;//9;         //wartość macierzy błędów pomiarowych

        public double Kx_giro_ost = 1;                //macierz wzmocnień wagowych Kalmana
        public double Ky_giro_ost = 1;                //macierz wzmocnień wagowych Kalmana
        public double Kz_giro_ost = 1;                //macierz wzmocnień wagowych Kalmana

        public double Kx_giro;                //macierz wzmocnień wagowych Kalmana kompasu
        public double Ky_giro;                //macierz wzmocnień wagowych Kalmana kompasu
        public double Kz_giro;                //macierz wzmocnień wagowych Kalmana kompasu

        public double Px_giro;                //macierz błędu filtracji
        public double Py_giro;                //macierz błędu filtracji
        public double Pz_giro;                //macierz błędu filtracji

        public double Px_giro_temp;           //
        public double Py_giro_temp;           //
        public double Pz_giro_temp;           //

        public double x_giro_temp_est;
        public double y_giro_temp_est;
        public double z_giro_temp_est;

        public double x_giro_est;
        public double y_giro_est;
        public double z_giro_est;

        public double x_giro_pomiarowy;       //pomiar wraz z szumem
        public double y_giro_pomiarowy;       //pomiar wraz z szumem
        public double z_giro_pomiarowy;       //pomiar wraz z szumem

        public double x_giro_real = 0;       //idealna wartość której poszukujemy
        public double y_giro_real = 0;       //idealna wartość której poszukujemy
        public double z_giro_real = 0;       //idealna wartość której poszukujemy
        //********************************************************************************


        //********************************
        //***********Do tabel*************
        //********************************
        public string[] azymut = new string[0];
        public string[] przechylenie = new string[0];
        public string[] pochylenie = new string[0];
        public string[] obrot = new string[0];
        public string[] data = new string[0];
        public string[] godzina = new string[0];
        public string[] indeks_n = new string[0];

        public double[] azymut_int = new double[0];
        public double[] przechylenie_int = new double[0];
        public double[] pochylenie_int = new double[0];
        public double[] obrot_int = new double[0];
        
        //*************************
        //Do kalibracji magnetometru
        //*************************
        public double wektor3D; 
        
        public double max_komp_x=1;
        public double max_komp_y=1;
        public double max_komp_z=1;

        public double min_komp_x=-1;
        public double min_komp_y=-1;
        public double min_komp_z=-1;

        public double gain_komp_x = 1;
        public double gain_komp_y = 1;
        public double gain_komp_z = 1;

        public double offset_komp_x = 0;
        public double offset_komp_y = 0;
        public double offset_komp_z = 0;
        //****************************


        //*************************
        //Do kalibracji GIROSKOPU
        //*************************
        public double suma_x = 0;
        public double suma_y = 0;
        public double suma_z = 0;

        public double srednia_x = 0;
        public double srednia_y = 0;
        public double srednia_z = 0;

        //**********************************
        //*****DO CAŁKOWANIA NUMERYCZNEGO***
        //**********************************

        public double pole_x_poczatek = 0;
        public double pole_y_poczatek = 0;
        public double pole_z_poczatek = 0;

        public double suma_x_pol = 0;
        public double suma_y_pol = 0;
        public double suma_z_pol = 0;

        public double suma_x_pol_poczatek = 0;
        public double suma_y_pol_poczatek = 0;
        public double suma_z_pol_poczatek = 0;

        public double suma_x_pol_koniec = 0;
        public double suma_y_pol_koniec = 0;
        public double suma_z_pol_koniec = 0;

        public double start_pole_x = 0;
        public double start_pole_y = 0;
        public double start_pole_z = 0;
       
       

        public bool button7_klikniety = false;

      

        private void Form1_Load(object sender, EventArgs e)
            {

                pictureBox4.Image = Properties.Resources.kompas;
                pictureBox5.Image = Properties.Resources.wskaznik;
                img = new Bitmap(pictureBox4.Image);
                //angle.Enabled = true;
                //angle_ValueChanged(null, EventArgs.Empty);


                timer1.Start();
                label56.Text = DateTime.Now.ToShortDateString();
                //*********************
                //****INICJALIZACJA****
                //*********************
               
                //~~~~~~~~KOMPAS~~~~~~~~~~
                x_est_ostanie = z_real;// +0.0001;
                //~~~~~~GIRO~~~~~~~~~~~~~~
                x_giro_est_ostanie = x_giro_real;
                y_giro_est_ostanie = y_giro_real;
                z_giro_est_ostanie = z_giro_real;
            //*******************************************



                if (serialPort1.IsOpen == false)
                {
                    //*-- USTAWIENIE POCZĄTKOWYCH STANÓW KONTROLEK I ETYKIER **--//

                    panel1.BackColor = Color.Red; //
                    label17.Text = "Rozłączony";
                    //*-- WSZYTANIE DOSTĘPNEJ LISTY PORTÓW COM DO COMBOBOXA **--/                                          

                    string[] port = System.IO.Ports.SerialPort.GetPortNames();
                    foreach (string item in port)
                    {
                        comboBox1.Items.Add(item);
                    }

                    if (comboBox1.Items.Count == 0)
                    {
                        MessageBox.Show("Nie wykryto żadnego portu COM");
                    }
                    else
                    {
                        comboBox1.Text = port[0]; //pierwszym elementem wyswietlanym w comboboxie będzie szósty port (BT)
                    }
                }
            }



        private void TimerOnTick(object a_sender, EventArgs a_eventArgs)
        {
            UpdateMethod();
        }

        private void UpdateMethod()
        {
           // listBox1.Items.Add(DateTime.Now.ToLongTimeString());
            Int16 sumai_x_pol = Convert.ToInt16(suma_x_pol);
            Int16 sumai_y_pol = Convert.ToInt16(suma_y_pol);
            Int16 sumai_z_pol = Convert.ToInt16(suma_z_pol);

            if (sumai_x_pol > 90) sumai_x_pol = 90;
            if (sumai_x_pol < -90) sumai_x_pol = -90;

            if (sumai_y_pol > 180) sumai_y_pol = 180;
            if (sumai_y_pol < -180) sumai_y_pol = -180;

            if (sumai_z_pol > 180) sumai_z_pol = 180;
            if (sumai_z_pol < -180) sumai_z_pol = -180;
            

            F2.slider1.Value = sumai_x_pol;
            F2.slider2.Value = sumai_y_pol;
            F2.slider3.Value = sumai_z_pol;


            F2.PitchAngle = Convert.ToDouble(F2.slider1.Value);
            F2.RollAngle = -1.0 * Convert.ToDouble(F2.slider2.Value) * Math.PI / 180;
            F2.YawAngle = -1.0 * Convert.ToDouble(F2.slider3.Value) * Math.PI / 180;
            if (F2.YawAngle < -Math.PI) F2.YawAngle = F2.YawAngle + Math.PI;
            Refresh();


            F2.Refresh();
           // F2.textBox1 = Convert.ToString(F2.slider1.Value);

        }

       /* private void button1_Click(object sender, EventArgs e)
        {
            UpdateMethod();
        }*/

      
            private void button5_Click(object sender, EventArgs e)
            {


                pictureBox1.Image = Properties.Resources.Gyroscope;
                pictureBox2.Image = Properties.Resources._013;
           
                if (serialPort1.IsOpen == false)
                {
                    try
                    {
                        button5.Text = "Rozłącz";
                        if (checkBox4.Checked == true)
                        {
                            serialPort1.BaudRate = 14400;
                        }
                        if (checkBox3.Checked == true)
                        {
                            serialPort1.BaudRate = 9600;
                        }
                            serialPort1.PortName = comboBox1.Text;
                        serialPort1.Open();
                        panel1.BackColor = Color.Green;
                        label17.Text = "Połączony";
                        notifyIcon1.ShowBalloonTip(1000, "KOMUNIKAT", "Połączono z portem COM", ToolTipIcon.Info);
                    }
                    catch (UnauthorizedAccessException)
                    {

                        notifyIcon1.ShowBalloonTip(1000, "KOMUNIKAT", "Nie masz uprawnień na otwarcie portu COM, być może jeden z twoich programów go wykorzystuje", ToolTipIcon.Error);
                        MessageBox.Show("NIE MA DOSTĘPU DO PORTU COM!", "UWAGA", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        richTextBox1.AppendText(odebrany_str);

                    }



                }
                else
                {
                    
                    
                    button5.Text = "Połącz";
                    serialPort1.Close();
                    panel1.BackColor = Color.Red;
                    label17.Text = "Rozłączony";
                    notifyIcon1.ShowBalloonTip(1000, "KOMUNIKAT", "Rozłączono z portem COM", ToolTipIcon.Info);

                }

            }

            public void rysuj_wykres(double a, double b)
            {
                //******WYKRES*************//
                
                 chart1.Series["Azymut pomiarowy"].Points.AddY(a);
                 chart1.Series["Azymut pomiarowy z poprawką deklinacyjną"].Points.AddY(b);
                 chart1.Series["Azymut po filtracji Kalmana"].Points.AddY(x_est);

                 chart1.ChartAreas["ChartArea1"].AxisX.Title = "Liczba próbek odebranych z magnetometru";
                 chart1.ChartAreas["ChartArea1"].AxisY.Title = "Azymut pomiarowy [°]";
                
                 chart2.Series["X"].Points.AddY(x_giro);
                 chart2.Series["X po filtracji"].Points.AddY(x_giro_est);
                /////
                 chart2.ChartAreas["ChartArea1"].AxisX.Title = "Liczba próbek odebranych z żyroskopu";
                 chart2.ChartAreas["ChartArea1"].AxisY.Title = "Prędkość kątowa X [°/s]";
                
                 chart3.Series["Y"].Points.AddY(y_giro);
                 chart3.Series["Y po filtracji"].Points.AddY(y_giro_est);
                ///
                 chart3.ChartAreas["ChartArea1"].AxisX.Title = "Liczba próbek odebranych z żyroskopu";
                 chart3.ChartAreas["ChartArea1"].AxisY.Title = "Prędkość kątowa Y [°/s]";
                 
                 chart4.Series["Z"].Points.AddY(z_giro);
                 chart4.Series["Z po filtracji"].Points.AddY(z_giro_est);
                ///
                 chart4.ChartAreas["ChartArea1"].AxisX.Title = "Liczba próbek odebranych z żyroskopu";
                 chart4.ChartAreas["ChartArea1"].AxisY.Title = "Prędkość kątowa Z [°/s]";


                 
                //**************************************************************
                //***************WYKRES ZMIAN KĄTÓW W PRZESTRZENI***************
                //**************************************************************
                  chart5.Series["Przechylenie (OŚ X)"].Points.AddY(suma_x_pol);
                  chart5.ChartAreas["ChartArea1"].AxisX.Title = "Liczba próbek odebranych z żyroskopu";
                  chart5.ChartAreas["ChartArea1"].AxisY.Title = "Przechylenie [°]";
  
                  chart6.Series["Pochylenie (OŚ Y)"].Points.AddY(suma_y_pol);
                  chart6.ChartAreas["ChartArea1"].AxisX.Title = "Liczba próbek odebranych z żyroskopu";
                  chart6.ChartAreas["ChartArea1"].AxisY.Title = "Pochylenie [°]";  
                
                  chart7.Series["Obrót (OŚ Z)"].Points.AddY(suma_z_pol);
                  chart7.ChartAreas["ChartArea1"].AxisX.Title = "Liczba próbek odebranych z żyroskopu";
                  chart7.ChartAreas["ChartArea1"].AxisY.Title = "Obrót [°]";

                  

                //scrollowanie wykresu kompasu
                 chart1.ChartAreas["ChartArea1"].AxisY.ScaleView.Zoomable = true;
                 chart1.ChartAreas["ChartArea1"].AxisY2.ScaleView.Size.Equals(Math.Abs(x_est));
                 chart1.ChartAreas["ChartArea1"].CursorY.AutoScroll = true;
                 chart1.ChartAreas["ChartArea1"].CursorY.IsUserSelectionEnabled = true;
                 //*****************************

                //***********Rysowanie natężenia pola magnetycznego dla wszystkich osi pomiarowych*****************//
                 chart10.Series["X"].Points.AddY(x_komp);
                 chart10.ChartAreas["ChartArea1"].AxisX.Title = "Liczba próbek odebranych z magnetometru";
                 chart10.ChartAreas["ChartArea1"].AxisY.Title = "Natężenie pola magnetycznego";

                 chart9.Series["Y"].Points.AddY(y_komp);
                 chart9.ChartAreas["ChartArea1"].AxisX.Title = "Liczba próbek odebranych z magnetometru";
                 chart9.ChartAreas["ChartArea1"].AxisY.Title = "Natężenie pola magnetycznego";

                 chart8.Series["Z"].Points.AddY(z_komp);
                 chart8.ChartAreas["ChartArea1"].AxisX.Title = "Liczba próbek odebranych z magnetometru";
                 chart8.ChartAreas["ChartArea1"].AxisY.Title = "Natężenie pola magnetycznego";

                 if (checkBox1.Checked == true)
                 {
                     chart11.Series["Series1"].Points.AddXY(x_komp, y_komp);
                 }
                 else chart11.Series["Series1"].Points.Clear();
                 //200 próbek na wykresie i kasowanie
                 count_kompas++;
                 if (count_kompas == 500)
                 {
                     //**Z kompasu***/
                  chart1.Series["Azymut pomiarowy"].Points.Clear();                           //wykasowanie wszystkich punktów pomiarowych w serii 1
                   chart1.Series["Azymut pomiarowy z poprawką deklinacyjną"].Points.Clear();   //wykasowanie wszystkich punktów pomiarowych w serii 2
                   chart1.Series["Azymut po filtracji Kalmana"].Points.Clear();                //wykasowanie wszystkich punktów pomiarowych w serii 3
                   chart10.Series["X"].Points.Clear();
                   chart9.Series["Y"].Points.Clear();
                   chart8.Series["Z"].Points.Clear();
                     count_kompas = 0;
               }
               count_giro++;
               if (count_giro == 500)
               {
                   //**Z żyroskopu**/
                
                                     chart2.Series["X"].Points.Clear();
                   chart2.Series["X po filtracji"].Points.Clear();
                   chart3.Series["Y"].Points.Clear();
                   chart3.Series["Y po filtracji"].Points.Clear();
                   chart4.Series["Z"].Points.Clear();
                   chart4.Series["Z po filtracji"].Points.Clear();

                   chart5.Series["Przechylenie (OŚ X)"].Points.Clear();
                   chart6.Series["Pochylenie (OŚ Y)"].Points.Clear();
                   chart7.Series["Obrót (OŚ Z)"].Points.Clear();
                   count_giro = 0;
               }


               chart12.Series["Temperatura rdzenia żyroskopu"].Points.AddY(temperatura);
            }

            public void rx_parse(object sender, EventArgs e)
            {
                
                
                if (licznik > 2)
                {
                   
                    dane_pomiarowe_nowe[0] = dane_pomiarowe_poprzednie [0];
                    dane_pomiarowe_nowe[1] = dane_pomiarowe_poprzednie[1];
                    dane_pomiarowe_nowe[2] = dane_pomiarowe_poprzednie[2];
                    dane_pomiarowe_nowe[3] = dane_pomiarowe_poprzednie[3];
                    dane_pomiarowe_nowe[4] = dane_pomiarowe_poprzednie[4];
                    dane_pomiarowe_nowe[5] = dane_pomiarowe_poprzednie[5];
                    dane_pomiarowe_nowe[6] = dane_pomiarowe_poprzednie[6];
                    
                }
                System.Media.SoundPlayer myPlayer = new System.Media.SoundPlayer();
                myPlayer.Stream = Properties.Resources.beep_01;


                kierunek_poczatek = kierunek_koniec;

                tab_size++;
                licz++;
                string[] do_tabeli = new string[5];

                progressBar1.Value = licznik_kalibracji;

                textBox4.Text = x_est.ToString();


                suma_x_pol_poczatek = suma_x_pol_koniec;
                suma_y_pol_poczatek = suma_y_pol_koniec;
                suma_z_pol_poczatek = suma_z_pol_koniec;
                //**********************************
                //WYZNACZENIE BIASU GIROSKOPU
                //*********************************

                textBox25.Text = Convert.ToString(licznik_kalibracji);

                if (radioButton1.Checked==true)
                {
                    textBox32.Text = "TRWA KALIBRACJA...";
                   if (licznik_kalibracji==1) MessageBox.Show("ROZPOCZYNA SIĘ PROCES KALIBRACJI....PROSZĘ NIE ZMIENIAĆ POŁOŻENIA ŻYROSKOPU");
                  
                    if (licznik_kalibracji < 1000)
                    {
                        licznik_kalibracji++;
                        suma_x = suma_x + x_giro_est;
                        suma_y = suma_y + y_giro_est;
                        suma_z = suma_z + z_giro_est;
                    }
                    if (licznik_kalibracji == 999 || licznik_kalibracji == 1000)
                    {
                        textBox32.Text = "KALIBRACJA ZAKOŃCZONA !";
                        if (licznik_kalibracji == 999) MessageBox.Show("KALIBRACJA ZAKOŃCZONA !");

                        //myPlayer.Play();

                        //Thread.Sleep(1000);

                        //myPlayer.Stop();
                     
                        srednia_x = suma_x / (licznik_kalibracji);
                        srednia_y = suma_y / (licznik_kalibracji);
                        srednia_z = suma_z / (licznik_kalibracji);

                        suma_x_pol = 0;
                        suma_y_pol = 0;
                        suma_z_pol = 0;

                    }
                }

                textBox15.Text = Convert.ToString(srednia_x);
                textBox16.Text = Convert.ToString(srednia_y);
                textBox23.Text = Convert.ToString(srednia_z);
                //*************************************************

                
                //*****************************************
                //**********PREDYKCJA**********************
                //*****************************************
                
                //~~~~~~~~~~KOMPAS~~~~~~~~~~~~~~
                x_temp_est = x_est_ostanie;// +0.0005;//proces_random;

                P_temp = P_last + Q;
                //~~~~~~~GIRO~~~~~~~~~~~~~~~~
                x_giro_temp_est = x_giro_est_ostanie;
                y_giro_temp_est = y_giro_est_ostanie;
                z_giro_temp_est = z_giro_est_ostanie;

                Px_giro_temp = Px_giro_last + Qx_giro;
                Py_giro_temp = Py_giro_last + Qy_giro;
                Pz_giro_temp = Pz_giro_last + Qz_giro;
                //***********************************************************




                //**************************************************************************
                //*****************Obliczanie macierzy wzmocnień Kalmana*******************
                //**************************************************************************
              

                if (checkBox2.Checked == true) 
                {
                    double roznica = z_pomiarowy - x_temp_est;
                    if (roznica > 5 || roznica < -5) K = 1;
                    else 
                    K = P_temp * (1.0 / (P_temp + R));
                }

                if (checkBox2.Checked == false)
                {
                    K = P_temp * (1.0 / (P_temp + R));
                }
                //**************************************************************************
                
                dane_pomiarowe = odebrany_str.Split(','); // rozdziel dane z macierzy pomiarowej na elementy składowe po przez znak ","


                //***Pobór danych z aTmegi*************//                
                textBox19.Text = dane_pomiarowe[3];    //x-giro
                textBox18.Text = dane_pomiarowe[4];    //y-giro
                textBox17.Text = dane_pomiarowe[5];    //z-giro


                
                textBox48.Text = dane_pomiarowe[6];
                //9797979797979777979797979797979797979797979797
               // temperatura = Convert.ToDouble(textBox48.Text);
                //**************************************************//
                //--------------------KOMPAS------------------------//
                //*************************************************//




                if (x_komp > Math.Abs(3000) || y_komp > Math.Abs(3000) || z_komp > Math.Abs(3000))
                {
                    x_komp =3000;
                    y_komp =3000;
                    z_komp =3000;
                }
                else
                {    //______________Azymut_Pomiarowy___________________//
                    x_komp = Convert.ToDouble(dane_pomiarowe[0]);  //konwersja zawartości x_kompasu textboxa do zmiennej double
                    y_komp = Convert.ToDouble(dane_pomiarowe[1]);  // konwersja zawartości y_kompasu textboxa do zmiennej double
                    z_komp = Convert.ToDouble(dane_pomiarowe[2]);  // konwersja zawartości z_kompasu textboxa do zmiennej double
                }

                if (radioButton2.Checked == true)
                {
                    textBox33.Text = "TRWA KALIBRACJA...";
                    

                    
                        if (x_komp > max_komp_x) max_komp_x = x_komp;
                        if (y_komp > max_komp_y) max_komp_y = y_komp;
                        if (z_komp > max_komp_z) max_komp_z = z_komp;
                        textBox36.Text = Convert.ToString(max_komp_x);
                        textBox35.Text = Convert.ToString(max_komp_y);
                        textBox34.Text = Convert.ToString(max_komp_z);

                        if (x_komp < min_komp_x) min_komp_x = x_komp;
                        if (y_komp < min_komp_y) min_komp_y = y_komp;
                        if (z_komp < min_komp_z) min_komp_z = z_komp;
                        textBox39.Text = Convert.ToString(min_komp_x);
                        textBox38.Text = Convert.ToString(min_komp_y);
                        textBox37.Text = Convert.ToString(min_komp_z);

                        wektor3D = Math.Sqrt((max_komp_x - min_komp_x) * (max_komp_x - min_komp_x) + (max_komp_y - min_komp_y) * (max_komp_y - min_komp_y) + (max_komp_z - min_komp_z) * (max_komp_z - min_komp_z));
                        wektor3D = Math.Round(wektor3D, 2);
                        textBox14.Text = Convert.ToString(wektor3D);

                        gain_komp_x = wektor3D / (max_komp_x - min_komp_x);
                        gain_komp_y = wektor3D / (max_komp_y - min_komp_y);
                        gain_komp_z = wektor3D / (max_komp_z - min_komp_z);

                        gain_komp_x = Math.Round(gain_komp_x, 2);
                        gain_komp_y = Math.Round(gain_komp_y, 2);
                        gain_komp_z = Math.Round(gain_komp_z, 2);

                        textBox42.Text = Convert.ToString(gain_komp_x);
                        textBox41.Text = Convert.ToString(gain_komp_y);
                        textBox40.Text = Convert.ToString(gain_komp_z);

                        offset_komp_x = ((0 - max_komp_x) + (0 - min_komp_x)) / 2;
                        offset_komp_y = ((0 - max_komp_y) + (0 - min_komp_y)) / 2;
                        offset_komp_z = ((0 - max_komp_z) + (0 - min_komp_z)) / 2;

                        offset_komp_x = Math.Round(offset_komp_x, 2);
                        offset_komp_y = Math.Round(offset_komp_y, 2);
                        offset_komp_z = Math.Round(offset_komp_z, 2);


                        textBox45.Text = Convert.ToString(offset_komp_x);
                        textBox44.Text = Convert.ToString(offset_komp_y);
                        textBox43.Text = Convert.ToString(offset_komp_z);

                  
                }
                if (radioButton3.Checked == true)
                {
                    textBox33.Text = "KALIBRACJA ZAKOŃCZONA ! ";
                   // MessageBox.Show("KALIBRACJA ZAKOŃCZONA !");
                    
                }
                //*****************************
                //DANE POMIAROWE PO KALIBRACJI
                //*****************************
                x_komp = x_komp * gain_komp_x + offset_komp_x;
                y_komp = y_komp * gain_komp_x + offset_komp_y;
                z_komp = z_komp * gain_komp_z + offset_komp_z;
                //*****************************
                
                textBox1.Text = Convert.ToString(x_komp);    //x-kompasu
                textBox2.Text = Convert.ToString(y_komp);    //y-kompasu
                textBox3.Text = Convert.ToString(z_komp);    //z-kompasu 

                /*
                if (x_komp >0 )  kat =  Math.Atan2(y_komp, x_komp);
                if (x_komp < 0 && y_komp >= 0) kat = Math.Atan2(y_komp, x_komp) + Math.PI;
                if (x_komp < 0 && y_komp < 0) kat = Math.Atan2(y_komp, x_komp) - Math.PI;
                if (x_komp == 0 && y_komp > 0) kat =Math.PI/2;
                if (x_komp == 0 && y_komp < 0) kat = - Math.PI / 2;

                stopnie = kat * 180 / Math.PI;
                  */ 
                 kat = Math.Atan2(y_komp, x_komp);    //obliczanie azymutu pomiarowego

                if (kat < 0) kat += 2 * Math.PI;            //warunki na 
                if (kat > 2 * Math.PI) kat -= 2 * Math.PI;  //przejścia ćwiartek układu współrzędnych

                 stopnie = kat * 180 / Math.PI;       //zamiana radianów na stopnie         
                    
                     
                     
                   
                     stopnie = Math.Round(stopnie, 2);           //Sprowadzenie wartości do 2 miejsc po przecinku

                


                textBox7.Text = stopnie.ToString();    // wyświetlenie w polu tekstowym wartości obliczonego azymutu pomiarowego




                //Zamiana stopni dzisiętnych na stopnie, minuty i sekundy azymutu pomiarowego

                //Wypisanie wartości azymutu w stopniach,minutach i sekundach

                var miara_kata = Helper.StopnieDoMiaryKatowej(stopnie);

                textBox28.Text = miara_kata.sekundy_azymutu.ToString();
                textBox27.Text = miara_kata.minuty_azymutu.ToString();
                textBox26.Text = Math.Floor(stopnie).ToString();


                //_________________________________________________//



                //_________________Azymut_pomiarowy_z_poprawka_deklinacyjna_______________//

                double stopnie_deklinacja = Convert.ToDouble(textBox8.Text); //pobranie stopni deklinacji magnetycznej
                double minuty_deklinacja = Convert.ToDouble(textBox9.Text); //pobranie minut deklinacji magnetycznej

                double deklinacja = ((stopnie_deklinacja + (minuty_deklinacja / 60)) * Math.PI / 180) / 1000; // obliczenie poprawki deklinacyjnej
               // double deklinacja =((stopnie_deklinacja + (minuty_deklinacja / 60) +stopnie));
                
                double kat_deklinacja = kat + deklinacja;           //dodanie poprawki do wartosci

                if (kat_deklinacja < 0) kat_deklinacja += 2 * Math.PI;                //Warunki azymutalne
                if (kat_deklinacja > 2 * Math.PI) kat_deklinacja -= 2 * Math.PI;     //na przejscia ćwiartek ukl. wspólrzednych

                double deklinacja_stopnie = kat_deklinacja * 180 / Math.PI;         //zamiana radianow na stopnie
                
                //double deklinacja_stopnie = deklinacja;
                deklinacja_stopnie = Math.Round(deklinacja_stopnie, 2);             //Sprowadzenie azymutu pomiarowego do 2 miejsc po przecinku
                textBox10.Text = (deklinacja_stopnie).ToString();                   // wyświetlenie w polu tekstowym wartości obliczonego azymutu pomiarowego z poprawką deklinacyjna
                

                //*************POMIAR*******************
                z_pomiarowy = deklinacja_stopnie;// +0.001;//pomiar_random; //pomiar wraz z błędem pomiarowym

                //*************KOREKCJA***********************
                x_est = x_temp_est + K * (z_pomiarowy - x_temp_est);


                P = (1 - K) * P_temp;
                //***********W tym momencie mamy nowy system o nowych parametrach*********** 
                
                x_est = Math.Round(x_est, 2);

                //Zamiana stopni dzisiętnych na stopnie, minuty i sekundy azymutu pomiarowego po filtracji kalmana

                var miara_katowa_kalman = Helper.StopnieDoMiaryKatowej(x_est);

                //Wypisanie wartości azymutu po filtracji Kalmana w stopniach,minutach i sekundach
                textBox11.Text = miara_katowa_kalman.sekundy_azymutu.ToString();
                textBox13.Text = miara_katowa_kalman.minuty_azymutu.ToString();
                textBox46.Text = Math.Floor(x_est).ToString();
                
                //Zamiana stopni dzisiętnych na stopnie, minuty i sekundy azymutu pomiarowego
                var miara_katowa_dekl = Helper.StopnieDoMiaryKatowej(deklinacja_stopnie);

                //Wypisanie wartości azymutu z poprawką deklinacyjną w stopniach,minutach i sekundach
                textBox29.Text = miara_katowa_dekl.sekundy_azymutu.ToString();
                textBox30.Text = miara_katowa_dekl.minuty_azymutu.ToString();
                textBox31.Text = Math.Floor(deklinacja_stopnie).ToString();

                int kierunek = Convert.ToInt16(Math.Floor(x_est));
              //  kierunek = numericUpDown1.Value();
                double obracanie = x_est;
                textBox47.Text = Convert.ToString(kierunek);

                
			

                Image oldImage = pictureBox4.Image;
                pictureBox4.Image = Utilities.RotateImage(img, -(float)obracanie);

                if (oldImage != null)
                {
                    oldImage.Dispose();
                }
                
               

                //************
                //róża wiatrów
                //************
               
                if (kierunek == 0) textBox24.Text = "NORTH";
                if (kierunek > 0 && kierunek < 90) textBox24.Text = "N" + kierunek.ToString() + "E";
                if (kierunek == 90) textBox24.Text = "EAST";
                if (kierunek > 90 && kierunek <180) 
                {
                    int cwiartka_4 = 90 - kierunek;
                    kierunek = 90 + cwiartka_4;
                    textBox24.Text = "S" + kierunek.ToString() + "E";
                }
                if (kierunek == 180) textBox24.Text = "SOUTH";
                if (kierunek > 180 && kierunek < 270)
                {
                    int cwiartka_3 = kierunek - 180;
                    kierunek = cwiartka_3;
                    textBox24.Text = "S" + kierunek.ToString() + "W";
                }
                if (kierunek == 270) textBox24.Text = "WEST";
                if (kierunek > 270 && kierunek < 360)
                {
                    int cwiartka_2 = 270 - kierunek;
                    kierunek = 90 + cwiartka_2;
                    textBox24.Text = "N" + kierunek.ToString() + "W";
                }
                //***********************************************************

               
                

                
                //******************************************
                //ZMIENNE POMOCNICZE DO WZMOCNIENIA FILTRU
                //******************************************
                //pomocnicze zmienne do warunków wzmocnienia filtru Kalmana
                double roznica_x = x_giro_pomiarowy - x_giro_temp_est;
                double roznica_y = y_giro_pomiarowy - y_giro_temp_est;
                double roznica_z = z_giro_pomiarowy - z_giro_temp_est;

                //*******************
                //WARUNKI WZMOCNIENIA
                //*******************
                //jeśli różnica pomiędzy wartościa estymowaną a pomiarem będzie większa od 5 lub mniejsza od 5 --> zaufaj pomiarowi -> Wzmocnienie filtru równe jednośći
                if (roznica_x > 20 || roznica_x < -20) Kx_giro = 1;
                else Kx_giro = Px_giro_temp * (1.0 / (Px_giro_temp + Rx_giro));

                if (roznica_y > 20 || roznica_y < -20) Ky_giro = 1;
                else Ky_giro = Py_giro_temp * (1.0 / (Py_giro_temp + Ry_giro));

                if (roznica_z > 20 || roznica_z < -20) Kz_giro = 1;
                else Kz_giro = Pz_giro_temp * (1.0 / (Pz_giro_temp + Rz_giro));
                //****************************************************************
                //_________________________________________________________________________//

                //**************************************************//
                //--------------------ŻYROSKOP------------------------//
                //*************************************************//
             
                //wartość prędkości kątowej to współczynnik skalujący (gain - wyznaczony doświadczalnie na podstawie 100 pomiarów na każdą oś) razy odebrana prędkość kątowa
                //z mikrokontrolera + offset, czyli składowa stała wyznaczona podczas jednorazowej kalibracji przy każdym uruchomieniu aplikacji.

                if (x_giro > Math.Abs(3000) )//|| y_giro > Math.Abs(3000) || z_giro > Math.Abs(3000))
                {
                    x_giro = 3000;
                   
                }
                else
                {
                    x_giro = 1.5 * Convert.ToDouble(dane_pomiarowe[3]) - srednia_x;//1.047935799 * Convert.ToDouble(dane_pomiarowe[3]) - srednia_x;
                    y_giro = Convert.ToDouble(dane_pomiarowe[4]) - srednia_y;//1.082405051 * Convert.ToDouble(dane_pomiarowe[4]) - srednia_y;
                    z_giro = Convert.ToDouble(dane_pomiarowe[5]) - srednia_z; //1.0903432 * Convert.ToDouble(dane_pomiarowe[5]) - srednia_z;                  
                }
                    //******
                //POMIAR
                //******
                //przypisz odebrane dane do zmiennej wykorzystywanej do wyznaczenia estymowanej wartości filtru Kalmana
                x_giro_pomiarowy = x_giro;
                y_giro_pomiarowy = y_giro;
                z_giro_pomiarowy = z_giro;
                //*******

                //********************************************
                //*************KOREKCJA***********************
                //********************************************
                x_giro_est = x_giro_temp_est + Kx_giro * (x_giro_pomiarowy - x_giro_temp_est);
                y_giro_est = y_giro_temp_est + Ky_giro * (y_giro_pomiarowy - y_giro_temp_est);
                z_giro_est = z_giro_temp_est + Kz_giro * (z_giro_pomiarowy - z_giro_temp_est);

                //zaokrąglij przefiltrowane wartości prędkości kątowej do czterech miejsc po przecinku
                x_giro_est = Math.Round(x_giro_est, 4);
                y_giro_est = Math.Round(y_giro_est, 4);
                z_giro_est = Math.Round(z_giro_est, 4);

                //wyświetl prędkość kątową po filtracji Kalmana
                textBox22.Text = Convert.ToString(x_giro_est);
                textBox21.Text = Convert.ToString(y_giro_est);
                textBox20.Text = Convert.ToString(z_giro_est);

                //altualizacja macierzy P dla nowych parametrów
                Px_giro = (1 - Kx_giro) * Px_giro_temp;
                Py_giro = (1 - Ky_giro) * Py_giro_temp;
                Pz_giro = (1 - Kz_giro) * Pz_giro_temp;
                //*********************************************
    //***********W tym momencie mamy nowy system o nowych parametrach*********** 

               
                
                //*********************************************
                //****CAŁKOWANIE NUMERYCZNE METODĄ TRAPEZOW****
                //*********************************************

                if (licznik_kalibracji == 1000)// Zaczynamy całkowanie danych w momencie, gdy nastąpi kalibracja urządzenia

                {
                    //Początkowe pozycja wyznaczona na podstawie pierwszego pomiaru całkowania numerycznego
                    start_pole_x = 0.5 * (x_giro_est + x_giro_temp_est) * 0.0035;
                    start_pole_y = 0.5 * (y_giro_est + y_giro_temp_est) * 0.0035;
                    start_pole_z = 0.5 * (z_giro_est + z_giro_temp_est) * 0.0035;
                }
 
                    if (licznik_kalibracji > 1000)//dla pomiarów od 1000 w górę następuje właściwy proces całkowania numerycznego danych
                    {

                        start_pole_x = 0;
                        start_pole_y = 0;
                        start_pole_z = 0;

                        //Odrzucaj wartości prędkości kątowej z przedziału <-4,4> aby nie zostały poddane całkowaniu ( wówczas szybko nie narasta błąd całkowania danych)  
                        if (Math.Abs(x_giro_est) < 4 || Math.Abs(y_giro_est) < 4 || Math.Abs(z_giro_est) < 4)
                        {
                            pole_x_poczatek = 0;
                            pole_y_poczatek = 0;
                            pole_z_poczatek = 0;


                        }
                        //Jeżeli jesteśmy poza zakresem powyższego przedziału dane podjęte są procesowi całkowania
                        if (Math.Abs(x_giro_est) > 4 || Math.Abs(y_giro_est) > 4 || Math.Abs(z_giro_est) > 4)
                        {

                            pole_x_poczatek = 0.5 * (x_giro_est + x_giro_temp_est) * 0.0035;
                            pole_y_poczatek = 0.5 * (y_giro_est + y_giro_temp_est) * 0.0035;
                            pole_z_poczatek = 0.5 * (z_giro_est + z_giro_temp_est) * 0.0035;


                        }

                    }
                
                //aktualny kąt to suma poprzedniego kąta + aktualny obliczony kąt + początkowa pozycja z pierwszego pomiaru po kalibracji
                suma_x_pol = suma_x_pol_poczatek + pole_x_poczatek + start_pole_x ;
                suma_y_pol = suma_y_pol_poczatek + pole_y_poczatek + start_pole_y;
                suma_z_pol = suma_z_pol_poczatek + pole_z_poczatek + start_pole_z;

                //Zaokrąglij obliczone kąty do czterech miejsc po przecinku
                suma_x_pol = Math.Round(suma_x_pol, 4);
                suma_y_pol = Math.Round(suma_y_pol, 4);
                suma_z_pol = Math.Round(suma_z_pol, 4);
                //Wyświetl obliczone dane kątów w przestrzeni
                textBox12.Text = Convert.ToString(suma_x_pol);
                textBox6.Text = Convert.ToString(suma_y_pol);
                textBox5.Text = Convert.ToString(suma_z_pol);

                //*************************************
                //****FUNKCJA DO RYSOWANIA WYKRESÓW****
                //*************************************
                rysuj_wykres(stopnie, deklinacja_stopnie);
             

                //*****************************************************
                //****WYŚWIETLENIE RAMKI ODEBRANEJ Z MIKROPROCESORA****
                //*****************************************************
                richTextBox1.AppendText(odebrany_str);


                //**********************************************************
                //*************Aktualizacja o kolejny pomiar****************
                //**********************************************************
                //~~~~~~~~~~~~~~~~~~~KOMPAS~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                P_last = P;
                x_est_ostanie = x_est;
                //~~~~~~~~~~~~~~~~~~~~~~~~GIRO~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                Px_giro_last = Px_giro;
                Py_giro_last = Py_giro;
                Pz_giro_last = Pz_giro;

                x_giro_est_ostanie = x_giro_est;
                y_giro_est_ostanie = y_giro_est;
                z_giro_est_ostanie = z_giro_est;

                suma_x_pol_koniec = suma_x_pol;
                suma_y_pol_koniec = suma_y_pol;
                suma_z_pol_koniec = suma_z_pol;

                //kierunek_koniec = kierunek;


                //**************************
                //****Do tabeli*************
                //**************************


                if (licz == 100)
                {
                  



                    do_tabeli[0] = Convert.ToString(x_est);
                    do_tabeli[1] = Convert.ToString(suma_x_pol);
                    do_tabeli[2] = Convert.ToString(suma_y_pol);
                    do_tabeli[3] = Convert.ToString(suma_z_pol);


                    Array.Resize(ref indeks_n, tab_size); // Cykliczne powiększanie rozmiaru tablicy
                    Array.Resize(ref azymut, tab_size); // Cykliczne powiększanie rozmiaru tablicy
                    Array.Resize(ref data, tab_size); // Cykliczne powiększanie rozmiaru tablicy
                    Array.Resize(ref godzina, tab_size); // Cykliczne powiększanie rozmiaru tablicy
                    Array.Resize(ref przechylenie, tab_size); // Cykliczne powiększanie rozmiaru tablicy
                    Array.Resize(ref pochylenie, tab_size); // Cykliczne powiększanie rozmiaru tablicy
                    Array.Resize(ref obrot, tab_size); // Cykliczne powiększanie rozmiaru tablicy
                    Array.Resize(ref przechylenie_int, tab_size); // Cykliczne powiększanie rozmiaru tablicy
                    Array.Resize(ref pochylenie_int, tab_size); // Cykliczne powiększanie rozmiaru tablicy
                    Array.Resize(ref obrot_int, tab_size); // Cykliczne powiększanie rozmiaru tablicy




                
                    indeks_n[indeks] = Convert.ToString(indeks);
                    azymut[indeks] = do_tabeli[0];
                    przechylenie[indeks] = do_tabeli[1];
                    pochylenie[indeks] = do_tabeli[2];
                    obrot[indeks] = do_tabeli[3];
                    data[indeks] = DateTime.Now.ToShortDateString();
                    godzina[indeks] = DateTime.Now.ToShortTimeString();

                    indeks++;
                    licz = 0;
                 }

               
                dane_pomiarowe_poprzednie[0] = dane_pomiarowe[0];
                dane_pomiarowe_poprzednie[1] = dane_pomiarowe[1];
                dane_pomiarowe_poprzednie[2] = dane_pomiarowe[2];
                dane_pomiarowe_poprzednie[3] = dane_pomiarowe[3];
                dane_pomiarowe_poprzednie[4] = dane_pomiarowe[4];
                dane_pomiarowe_poprzednie[5] = dane_pomiarowe[5];
                dane_pomiarowe_poprzednie[6] = dane_pomiarowe[6];
               
                
               
            
            }
/*
            public static Bitmap RotateImage(Image image, float rotateAtX, float rotateAtY, float angle, bool bNoClip)
            {
                int W, H, X, Y;
                if (bNoClip)
                {
                    double dW = (double)image.Width;
                    double dH = (double)image.Height;

                    double degrees = Math.Abs(angle);
                    if (degrees <= 90)
                    {
                        double radians = 0.0174532925 * degrees;
                        double dSin = Math.Sin(radians);
                        double dCos = Math.Cos(radians);
                        W = (int)(dH * dSin + dW * dCos);
                        H = (int)(dW * dSin + dH * dCos);
                        X = (W - image.Width) / 2;
                        Y = (H - image.Height) / 2;
                    }
                    else
                    {
                        degrees -= 90;
                        double radians = 0.0174532925 * degrees;
                        double dSin = Math.Sin(radians);
                        double dCos = Math.Cos(radians);
                        W = (int)(dW * dSin + dH * dCos);
                        H = (int)(dH * dSin + dW * dCos);
                        X = (W - image.Width) / 2;
                        Y = (H - image.Height) / 2;
                    }
                }
                else
                {
                    W = image.Width;
                    H = image.Height;
                    X = 0;
                    Y = 0;
                }

                //create a new empty bitmap to hold rotated image
                Bitmap bmpRet = new Bitmap(W, H);
                bmpRet.SetResolution(image.HorizontalResolution, image.VerticalResolution);

                //make a graphics object from the empty bitmap
                Graphics g = Graphics.FromImage(bmpRet);

                //Put the rotation point in the "center" of the image
                g.TranslateTransform(rotateAtX + X, rotateAtY + Y);

                //rotate the image
                g.RotateTransform(angle);

                //move the image back
                g.TranslateTransform(-rotateAtX - X, -rotateAtY - Y);

                //draw passed in image onto graphics object
                g.DrawImage(image, new PointF(0 + X, 0 + Y));

                return bmpRet;
            }*/
            public void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
            {
                licznik++;
                //licznik_kalibracji++;
                if (licznik == 1) //jeśli pierwszy raz uruchomiona aplikacja wyczyść odebrany łańcuch
                {
                    odebrany_str = (sender as SerialPort).ReadTo("\n\r"); //odbierz łańcuch do napotkania znaku "\n"
                    odebrany_str = " ";


                }
                else
                {
                    odebrany_str = serialPort1.ReadTo("\n\r"); // przekazanie odebranego łańcucha do zmiennej rx_str
                    this.Invoke(new EventHandler(rx_parse)); //instalacja zdarzenia parsującego odebrany łańcuch
                }


                
                
            }
        
            private void textBox7_TextChanged(object sender, EventArgs e)
            {

            }

            private void button6_Click(object sender, EventArgs e)
            {

            }

            private void button6_KeyDown(object sender, KeyEventArgs e)
            {


            }

            private void button6_Click_1(object sender, EventArgs e)
            {

                       }

            private void button6_Click_2(object sender, EventArgs e)
            {
                Close();
            }

            

            private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
            {

            }
           

            private void button7_Click(object sender, EventArgs e)
            {
             
            }

            private void groupBox9_Enter(object sender, EventArgs e)
            {

            }

            private void textBox4_TextChanged(object sender, EventArgs e)
            {

            }

            private void timer1_Tick(object sender, EventArgs e)
            {
              //  serialPort1.Write("s");
                label20.Text = DateTime.Now.ToShortTimeString();
                
               }

            private void button10_Click(object sender, EventArgs e)
            {
                if (timer1.Enabled == false) //sprawdzamy czy timer jest wyłączony
                {
                    // Przykladowy komentarz
                    timer1.Enabled = true; //uruchamiamy timer
                    timer1.Start(); //wystartowanie odliczania czasu

                }
                else
                {
                 timer1.Enabled = false; //wyłączenie timera

                timer1.Stop(); //zatrzymanie
          
                }

            }

            private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
            {

            }

            private void label30_Click(object sender, EventArgs e)
            {

            }

            private void radioButton1_CheckedChanged(object sender, EventArgs e)
            {

            }

            
           
            
        
        
            private void Form1_FormClosing(object sender, FormClosingEventArgs e)
            {

                if (serialPort1.IsOpen == true) serialPort1.Close();

            }

         

            private void button8_Click(object sender, EventArgs e)
            {
                UpdateMethod();
                Invalidate();
                Update();
                F2.ShowDialog();
                /*
                Int16 sumai_x_pol = Convert.ToInt16(suma_x_pol);
                Int16 sumai_y_pol = Convert.ToInt16(suma_y_pol);
                Int16 sumai_z_pol = Convert.ToInt16(suma_z_pol);

                    F2.slider1.Value = sumai_x_pol;
                    F2.slider2.Value = sumai_y_pol;
                    F2.slider3.Value = sumai_z_pol;
                    F2.ShowDialog();
                */
            }

            private void numericUpDown1_ValueChanged(object sender, EventArgs e)
            {

            }

            private void tabPage3_Click(object sender, EventArgs e)
            {

            }

            private void chart1_Click(object sender, EventArgs e)
            {

            }

            private void chart2_Click(object sender, EventArgs e)
            {

            }

            private void groupBox24_Enter(object sender, EventArgs e)
            {

            }

            private void button1_Click_1(object sender, EventArgs e)
            {
                MessageBox.Show("Deklinację magnetyczną można sprawdzić dla danego miejsca na ziemi na stronie: http://www.geomag.bgs.ac.uk/data_service/models_compass/wmm_calc.html");
            }

            private void button4_Click(object sender, EventArgs e)
            {
                
            }

            /*private void angle_ValueChanged(object sender, EventArgs e)
            {

                if (angle.Value > 359.9m)
                {
                    angle.Value = 0;
                    return;
                }

                if (angle.Value < 0.0m)
                {
                    angle.Value = 359;
                    return;
                }

                Image oldImage = pictureBox4.Image;
                pictureBox4.Image = Utilities.RotateImage(img, (float)angle.Value);

                if (oldImage != null)
                {
                    oldImage.Dispose();
                }
            }
        */
            private void button2_Click(object sender, EventArgs e)
            {
                AboutBox1 a = new AboutBox1();
                a.ShowDialog();
            }

            private void button9_Click(object sender, EventArgs e)
            {
                tabela pomiar_tabela = new tabela();
                double min_1 = 0;
                double min_2 = 0;
                double min_3 = 0;
                double min_4 = 0;

                double max_1 = 0;
                double max_2 = 0;
                double max_3 = 0;
                double max_4 = 0;

                double srednia_1 = 0;
                double srednia_2 = 0;
                double srednia_3 = 0;
                double srednia_4 = 0;

              



                pomiar_tabela.Show();
                pomiar_tabela.Text = "Tabela pomiarowa";

                
              


                for (int n = 1; n < indeks; n++)
               
                {
                    
                        pomiar_tabela.dataGridView1.Rows.Add(n);
                        pomiar_tabela.dataGridView1.Rows[n].Cells[0].Value = indeks_n[n];
                        pomiar_tabela.dataGridView1.Rows[n].Cells[1].Value = data[n];
                        pomiar_tabela.dataGridView1.Rows[n].Cells[2].Value = godzina[n];
                        pomiar_tabela.dataGridView1.Rows[n].Cells[3].Value = azymut[n];
                        pomiar_tabela.dataGridView1.Rows[n].Cells[4].Value = przechylenie[n];
                        pomiar_tabela.dataGridView1.Rows[n].Cells[5].Value = pochylenie[n];
                        pomiar_tabela.dataGridView1.Rows[n].Cells[6].Value = obrot[n];



                        /*KONWERSJA DANYCH POTRZEBNYCH DO ANALIZY */

                    /*
                    
                        azymut_int[n] = Convert.ToDouble(azymut[n]);
                        przechylenie_int[n] = Convert.ToDouble(przechylenie[n]);
                        pochylenie_int[n] = Convert.ToDouble(pochylenie[n]);
                        obrot_int[n] = Convert.ToDouble(obrot[n]);
                    
                    
*/
                    /*
                    min_1 = azymut_int.Min(); // 
                    min_2 = przechylenie_int.Min();
                    min_3 = pochylenie_int.Min();
                    min_4 = obrot_int.Min();

                    max_1 = azymut_int.Max();
                    max_2 = przechylenie_int.Max();
                    max_3 = pochylenie_int.Max();
                    max_4 = obrot_int.Max();

                    srednia_1 = azymut_int.Average(); // 
                    srednia_2 = przechylenie_int.Average(); //
                    srednia_3 = pochylenie_int.Average(); //
                    srednia_4 = obrot_int.Average(); //



                    pomiar_tabela.textBox1.Text = min_4.ToString();
                    pomiar_tabela.textBox2.Text = min_3.ToString();
                    pomiar_tabela.textBox3.Text = min_2.ToString();
                    pomiar_tabela.textBox39.Text = min_1.ToString();

                    pomiar_tabela.textBox34.Text = max_4.ToString();
                    pomiar_tabela.textBox35.Text = max_3.ToString();
                    pomiar_tabela.textBox36.Text = max_2.ToString();
                    pomiar_tabela.textBox38.Text = max_1.ToString();

                    pomiar_tabela.textBox4.Text = srednia_4.ToString();
                    pomiar_tabela.textBox5.Text = srednia_3.ToString();
                    pomiar_tabela.textBox6.Text = srednia_2.ToString();
                    pomiar_tabela.textBox8.Text = srednia_1.ToString();

                      */
}                      
                     
                }

            private void tabPage5_Click(object sender, EventArgs e)
            {
                
            }

            private void checkBox3_CheckedChanged(object sender, EventArgs e)
            {

            }

            private void label57_Click(object sender, EventArgs e)
            {

            }

            private void pictureBox4_Click(object sender, EventArgs e)
            {

            }

            private void label62_Click(object sender, EventArgs e)
            {

            }

            private void label60_Click(object sender, EventArgs e)
            {

            }

            private void button3_Click(object sender, EventArgs e)
            {
                serialPort1.Write("x");
            }
        }
    }

