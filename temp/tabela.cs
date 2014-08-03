using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class tabela : Form
    {
        public tabela()
        {
            InitializeComponent();
        }


        //public string[] odstep = new string[" "];

        private void button1_Click(object sender, EventArgs e)
        {
            wykres wykres = new wykres();
            string odstep = " ";
            int licznik = dataGridView1.Rows.Count - 1;
            int poczatek = 0;
            int koniec = 0;
           
             int wart_skoku = Convert.ToInt16(textBox9.Text);
            
            for (int i = 0; i < licznik; i++)
            {
                odstep = dataGridView1.Rows[i].Cells[2].Value.ToString();
                if (maskedTextBox1.Text == odstep)
                {
                    poczatek = i;
                    break;
                }
            }

            for (int x = 1; x < licznik; x++)
            {
                odstep = dataGridView1.Rows[x].Cells[2].Value.ToString();
                if (maskedTextBox2.Text == odstep)
                {
                    koniec = x;
                    continue;
                }
            }
            wykres.chart1.Titles["Title1"].Text = "Diagram pomiarów azymutu";
            wykres.chart1.ChartAreas["ChartArea1"].AxisX.Title = "Czas [h:min]";
            wykres.chart1.ChartAreas["ChartArea1"].AxisY.Title = "Wartość azymutu [°]";

            wykres.chart2.Titles["Title1"].Text = "Dryf żyroskopu względem upływającego czasu";
            wykres.chart2.ChartAreas["ChartArea1"].AxisX.Title = "Czas [h:min]";
            wykres.chart2.ChartAreas["ChartArea1"].AxisY.Title = "Wartości kątów przechyleń [°]";
            
            wykres.Text = "Wykresy zobrazowujące wyniki pomiarów z tabeli";
            



            for (int a = poczatek; a <= koniec; a = a + wart_skoku)
            //{
              
            //for(int a = 1; a<30; a++)
            {
            double kompas = Convert.ToDouble(dataGridView1.Rows[a].Cells[3].Value);
                double giro_przech = Convert.ToDouble(dataGridView1.Rows[a].Cells[4].Value);
                double giro_poch = Convert.ToDouble(dataGridView1.Rows[a].Cells[5].Value);
                double giro_obr = Convert.ToDouble(dataGridView1.Rows[a].Cells[6].Value);

                wykres.chart1.Series["Azymut"].Points.AddXY(dataGridView1.Rows[a].Cells[2].Value, kompas);

                wykres.chart2.Series["Przechylenie"].Points.AddXY(dataGridView1.Rows[a].Cells[2].Value, giro_przech);
                wykres.chart2.Series["Pochylenie"].Points.AddXY(dataGridView1.Rows[a].Cells[2].Value, giro_poch);
                wykres.chart2.Series["Obrót"].Points.AddXY(dataGridView1.Rows[a].Cells[2].Value, giro_obr);





                //scrollowanie wykresu kompasu
                wykres.chart1.ChartAreas["ChartArea1"].AxisY.ScaleView.Zoomable = true;
                wykres.chart1.ChartAreas["ChartArea1"].AxisY2.ScaleView.Size.Equals(kompas);
                wykres.chart1.ChartAreas["ChartArea1"].CursorY.AutoScroll = true;
                wykres.chart1.ChartAreas["ChartArea1"].CursorY.IsUserSelectionEnabled = true;
                //*****************************


                //scrollowanie wykresu żyroskopu
                wykres.chart2.ChartAreas["ChartArea1"].AxisY.ScaleView.Zoomable = true;
                wykres.chart2.ChartAreas["ChartArea1"].AxisY2.ScaleView.Size.Equals(giro_obr);
                wykres.chart2.ChartAreas["ChartArea1"].CursorY.AutoScroll = true;
                wykres.chart2.ChartAreas["ChartArea1"].CursorY.IsUserSelectionEnabled = true;
                //*****************************
            }

            if (checkBox1.Checked == true)
            {
                wykres.chart1.Series["Azymut"].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
                wykres.chart1.Series["Azymut"].MarkerColor = Color.Green;
                wykres.chart1.Series["Azymut"].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
                wykres.chart1.Series["Azymut"].MarkerColor = Color.Blue;
                //wykres.chart1.Series["Azymut"].MarkerImageTransparentColor.ToKnownColor();
            }
            else
            {
                wykres.chart1.Series["Azymut"].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.None;
                wykres.chart1.Series["Azymut"].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.None;

            }

            wykres.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int numer = dataGridView1.RowCount;

         //   saveFileDialog1.Filter = "Pliki typu (.txt)";

            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                StreamWriter zapisuj = new StreamWriter(saveFileDialog1.FileName);

                
                zapisuj.WriteLine(numer.ToString());

                for (int a = 1; a < numer; a++)
                {
                    //zapisuj.Write(dataGridView1.Rows[a].Cells[0].Value);
                    //zapisuj.Write(", ");
                    //zapisuj.Write(dataGridView1.Rows[a].Cells[1].Value);
                    //zapisuj.Write("; ");
                    zapisuj.Write(dataGridView1.Rows[a].Cells[2].Value);
                    zapisuj.Write("; ");
                    zapisuj.Write(dataGridView1.Rows[a].Cells[3].Value);
                    zapisuj.Write("; ");
                    zapisuj.Write(dataGridView1.Rows[a].Cells[4].Value);
                    zapisuj.Write("; ");
                    zapisuj.Write(dataGridView1.Rows[a].Cells[5].Value);
                    zapisuj.Write("; ");
                    zapisuj.Write(dataGridView1.Rows[a].Cells[6].Value);
                    zapisuj.Write("; ");

                    zapisuj.Close();
                }

            }
        }
    }
}
