using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.DataVisualization.Charting;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        string[] data = { "(n, n’)", "(n, 2n)", "(n, 3n)", "(n, g)", "(n, p)", "(n, d)", "(n, t)", "(n, a)" };
        string[] data2 = { "b + распад", "b - распад", "(n, n’)", "(n, 2n)", "(n, 3n)", "(n, g)", "(n, p)", "(n, d)", "(n, t)", "(n, a)" };
        string[] data3 = { "Выберите..." };

        double XMin = 0;/// Левая граница графика
        double XMax = 500;/// Правая граница графика
        double Step = 100;/// Шаг графика
        // Массив значений X - общий для обоих графиков
        double[] x;
        // Два массива Y - по одному для каждого графика
        double[] y1;
        double[] y2;

        public Form1()
        {
            InitializeComponent();

            chart1.ChartAreas[0].AxisX.Minimum = XMin;
            chart1.ChartAreas[0].AxisX.Maximum = XMax;
            chart1.ChartAreas[0].AxisY.Minimum = -200000;
            chart1.ChartAreas[0].AxisY.Maximum = 1200000;

            // Определяем шаг сетки
            chart1.ChartAreas[0].AxisX.MajorGrid.Interval = Step;
            chart1.ChartAreas[0].AxisY.MajorGrid.Interval = 200000;
            chart1.ChartAreas[0].AxisY.Interval = 200000;

            comboBox1.SelectedIndex = 0; //чтобы сразу было выбран первый индекс
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;// запрет редактирования
            comboBox2.SelectedIndex = 0; //чтобы сразу было выбран первый индекс
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;// запрет редактирования

            comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox1.SelectedIndexChanged += new EventHandler(cb1_SelectedIndexChanged);
            comboBox2.SelectedIndexChanged += new EventHandler(cb2_SelectedIndexChanged);
        }

        Graphics g;
        Graphics g2;
        //Graphics gr;
       
        Pen myWind = new Pen(Color.Black, 2);
        void cb1_SelectedIndexChanged(object sender, EventArgs e)
        {
            g = CreateGraphics();//создание объекта Graphics
            g.Clear(System.Drawing.ColorTranslator.FromHtml("#f0f0f0"));//очистка области рисования и заполнение цветом
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            label4.Text = ""; label5.Text = ""; label6.Text = "";//очистка текстовых меток
            label7.Text = "";label8.Text = "";label9.Text = "";//очистка текстовых меток
            label10.Text = "";label11.Text = "";//очистка текстовых меток
            string a1 = comboBox1.SelectedItem.ToString();
            if ((a1.IndexOf('-')) != -1)
            {
                g = CreateGraphics();
                g.DrawEllipse(myWind, 300, 80, 100, 100);

                label4.Text = a1.Substring(0, a1.IndexOf('-'));
                label5.Text = a1.Substring(a1.IndexOf('-') + 1);
            }
            if (comboBox1.SelectedItem.ToString() == "Au-197")
            {
                comboBox2.Items.Clear();
                comboBox2.Items.AddRange(data);
            }
            else if (comboBox1.SelectedItem.ToString() == "Выберите...")
            {
                comboBox2.Items.Clear();
                comboBox2.Items.AddRange(data3);
                comboBox2.SelectedIndex = 0;
            }
            else
            {
                comboBox2.Items.Clear();
                comboBox2.Items.AddRange(data2);
            }

        }

        void cb2_SelectedIndexChanged(object sender, EventArgs e)
        {
            g2 = CreateGraphics();
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            label6.Text = "";
            label7.Text = "";
            label8.Text = "";
            label9.Text = "";
            label10.Text = "";
            label11.Text = "";
            string a1 = comboBox1.SelectedItem.ToString();
            if ((a1.IndexOf('-')) != -1)
            {
                g2 = CreateGraphics();
                g2.DrawEllipse(myWind, 500, 80, 100, 100);

                myWind.CustomStartCap = new AdjustableArrowCap(6, 6);///стрелка
                g2.DrawArc(myWind, 350, 40, 200, 200, 23, 133);

                Pen pen = new Pen(Color.Black, 2);
                pen.CustomEndCap = new AdjustableArrowCap(6, 6);///стрелка
                g2.DrawArc(pen, 350, 40, 200, 200, -35, -110);

                label8.Text = comboBox2.SelectedItem.ToString();

                string b = a1.Substring(0, a1.IndexOf('-'));
                int a = int.Parse(a1.Substring(a1.IndexOf('-') + 1));
                string r = comboBox2.SelectedItem.ToString();

                string[] s = { "", "", "" };
                s = DeterminationOfDaughterNuclide(b, a, r);

                label7.Text = s[0];
                label6.Text = s[1];

                if (s[2] != "0") {label9.Text = s[2]; }
                else { label10.Text = "Невозможно найти обратную реакцию!!!"; };
            }
        }

        private string [] DeterminationOfDaughterNuclide(string b, int a, string r)
        {
            string[] s = { "", "", ""};

            Dictionary<string, string> Nucl = new Dictionary<string, string>();
            Nucl = new Dictionary<string, string>()
                {
                    { "77", "Ir"},
                    { "78", "Pt"},
                    { "79", "Au"},
                    { "80", "Hg"}
                };
            
            if (a == 197)
            {
                Dictionary<string, int> Reac = new Dictionary<string, int>();
                Reac = new Dictionary<string, int>()
                {
                    {"(n, n’)", 0},
                    {"(n, 2n)", -1},
                    {"(n, 3n)", -2},
                    {"(n, g)", 1},
                    {"(n, p)", -1000},
                    {"(n, d)", -1001},
                    {"(n, t)", -1002},
                    {"(n, a)", -2003}
                };
                string GetKeyByValue(int value)
                {
                    foreach (var recordOfDictionary in Reac)
                    {
                        if (recordOfDictionary.Value.Equals(value))
                            return recordOfDictionary.Key;
                    }
                    return "0";
                };
                int z = 79;
                int za = z * 1000 + a;
                int dza = 0;
                Reac.TryGetValue(r, out dza);
                int d = za + dza;
                z = d / 1000;
                a = d % 1000;

                Nucl.TryGetValue(z.ToString(), out s[0]);
                s[1] = a.ToString();

                int d2 = za - d;
                s[2] = GetKeyByValue(d2);
            }
            else
            {
                Dictionary<string, int> Reac = new Dictionary<string, int>();
                Reac = new Dictionary<string, int>()
                {
                    {"b + распад", -1000},
                    {"b - распад", 1000},
                    {"(n, n’)", 0},
                    {"(n, 2n)", -1},
                    {"(n, 3n)", -2},
                    {"(n, g)", 1},
                    {"(n, p)", -1000},
                    {"(n, d)", -1001},
                    {"(n, t)", -1002},
                    {"(n, a)", -2003}
                };
                string GetKeyByValue(int value)
                {
                    foreach (var recordOfDictionary in Reac)
                    {
                        if (recordOfDictionary.Value.Equals(value))
                            return recordOfDictionary.Key;
                    }
                    return "0";
                };
                int z = 79;
                int za = z * 1000 + a;
                int dza = 0;
                Reac.TryGetValue(r, out dza);
                int d = za + dza;
                z = d / 1000;
                a = d % 1000;

                Nucl.TryGetValue(z.ToString(), out s[0]);
                s[1] = a.ToString();

                int d2 = za - d;
                s[2] = GetKeyByValue(d2);
            }
            return s;
        }

        private void CalcFunction(double k1, double k2)/// Расчёт значений графика
        {
            // Количество точек графика
            int count = (int)Math.Ceiling((XMax - XMin) / Step) + 1;
            // Создаём массивы нужных размеров
            x = new double[count];
            y1 = new double[count];
            y2 = new double[count];
            long a = 1000000;
            //double k1 = 0.00000004616;
            //double k2 = 0.0000000000007206;
            // Расчитываем точки для графиков функции
            for (int i = 0; i < count; i++)
            {
                // Вычисляем значение X
                x[i] = XMin + Step * i;
                double t = x[i] * 24 * 3600;
                // Вычисляем значение функций в точке X
                y1[i] = ((a * k2) / (k1 + k2)) + (((a * k1) / (k1 + k2)) * Math.Pow(Math.E, (-(k1 + k2) * t)));
                y2[i] = ((a * k1) / (k1 + k2)) - (((a * k1) / (k1 + k2)) * Math.Pow(Math.E, (-(k1 + k2) * t)));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (label9.Text == "")
            {
                label11.Text = "Невозможно построить график!!!";
            }
            else
            {
                double k1;
                double k2 = 0;

                if (label5.Text == "197")
                {
                    Dictionary<string, double> Reac197 = new Dictionary<string, double>();
                    Reac197 = new Dictionary<string, double>()
                    {
                        {"(n, n’)", 0.0000000269},
                        {"(n, 2n)", 0.00000000000589},
                        {"(n, g)", 0.0000000485}
                    };
                    Reac197.TryGetValue(label8.Text, out k1);

                    if (label7.Text == "Au")
                    {
                        if (label6.Text == "197")
                        {
                            Reac197 = new Dictionary<string, double>()
                            {
                                {"(n, n’)", 0.0000000269},
                                {"(n, 2n)", 0.00000000000589},
                                {"(n, g)", 0.0000000485}
                            };
                            Reac197.TryGetValue(label9.Text, out k2);
                        }
                        else
                        {
                            Dictionary<string, double> SRAU = new Dictionary<string, double>();
                            SRAU = new Dictionary<string, double>()
                            {
                                { "191", 0.0000568},
                                { "192", 0.000039},
                                { "193", 0.0000109},
                                { "194", 0.00000506},
                                { "195", 0.000000431},
                                { "196", 0.0000000934},
                                { "198", 0.00000298},
                                { "199", 0.00000256},
                                { "200", 0.000238687},
                                { "201", 0.000444325},
                                { "202", 0.02406761},
                                { "203", 0.011552453},
                                { "204", 0.017415758},
                                { "205", 0.022359586},
                                { "190", 0.0002699},
                                { "206", 0.014748}
                            };

                            SRAU.TryGetValue(label6.Text, out k2);
                        }
                    }
                    else if (label7.Text=="Pt")
                    {
                        Dictionary<string, double> SRPt = new Dictionary<string, double>();
                        SRPt = new Dictionary<string, double>()
                            {
                                { "191", 0.0000028},
                                { "192", 0},
                                { "193", 0},
                                { "194", 0},
                                { "195", 0},
                                { "196", 0},
                                { "197", 0.00000968},
                                { "198", 0},
                                { "199", 0.0003726},
                                { "200", 0.0000154},
                                { "201", 0.00462},
                                { "202", 0.0000043759},
                                { "203", 0.0315},
                                { "204", 0.0672958}
                            };

                        SRPt.TryGetValue(label6.Text, out k2);
                    }
                    else if (label7.Text == "Hg")
                    {
                        Dictionary<string, double> SRHg = new Dictionary<string, double>();
                        SRHg = new Dictionary<string, double>()
                        {
                                { "191", 0.0002357643},
                                { "192", 0.0000396992},
                                { "193", 0.0000506687},
                                { "194", 0},
                                { "195", 0.000018285},
                                { "196", 0},
                                { "197", 0.0000030019},
                                { "198", 0},
                                { "199", 0},
                                { "200", 0},
                                { "201", 0},
                                { "202", 0},
                                { "203", 0},
                                { "204", 0},
                                { "205", 0.002247559}
                        };

                        SRHg.TryGetValue(label6.Text, out k2);
                    }
                }
                else
                {
                    Dictionary<string, double> SRAU = new Dictionary<string, double>();
                    SRAU = new Dictionary<string, double>()
                    {
                        { "191", 0.0000568},
                        { "192", 0.000039},
                        { "193", 0.0000109},
                        { "194", 0.00000506},
                        { "195", 0.000000431},
                        { "196", 0.0000000934},
                        { "198", 0.00000298},
                        { "199", 0.00000256},
                        { "200", 0.000238687},
                        { "201", 0.000444325},
                        { "202", 0.02406761},
                        { "203", 0.011552453},
                        { "204", 0.017415758},
                        { "205", 0.022359586},
                        { "190", 0.0002699},
                        { "206", 0.014748}
                    };

                    SRAU.TryGetValue(label5.Text, out k1);

                    if (label7.Text == "Au")
                    {
                        if (label6.Text == "197")
                        {
                            Dictionary<string, double> Reac197 = new Dictionary<string, double>();
                            Reac197 = new Dictionary<string, double>()
                            {
                                {"(n, n’)", 0.0000000269},
                                {"(n, 2n)", 0.00000000000589},
                                {"(n, g)", 0.0000000485}
                            };
                            Reac197.TryGetValue(label9.Text, out k2);
                        }
                        else
                        {
                            SRAU = new Dictionary<string, double>()
                            {
                                { "191", 0.0000568},
                                { "192", 0.000039},
                                { "193", 0.0000109},
                                { "194", 0.00000506},
                                { "195", 0.000000431},
                                { "196", 0.0000000934},
                                { "198", 0.00000298},
                                { "199", 0.00000256},
                                { "200", 0.000238687},
                                { "201", 0.000444325},
                                { "202", 0.02406761},
                                { "203", 0.011552453},
                                { "204", 0.017415758},
                                { "205", 0.022359586},
                                { "190", 0.0002699},
                                { "206", 0.014748}
                            };

                            SRAU.TryGetValue(label6.Text, out k2);
                        }
                    }
                    else if (label7.Text == "Pt")
                    {
                        Dictionary<string, double> SRPt = new Dictionary<string, double>();
                        SRPt = new Dictionary<string, double>()
                            {
                                { "191", 0.0000028},
                                { "192", 0},
                                { "193", 0},
                                { "194", 0},
                                { "195", 0},
                                { "196", 0},
                                { "197", 0.00000968},
                                { "198", 0},
                                { "199", 0.0003726},
                                { "200", 0.0000154},
                                { "201", 0.00462},
                                { "202", 0.0000043759},
                                { "203", 0.0315},
                                { "204", 0.0672958}
                            };

                        SRPt.TryGetValue(label6.Text, out k2);
                    }
                    else if (label7.Text == "Hg")
                    {
                        Dictionary<string, double> SRHg = new Dictionary<string, double>();
                        SRHg = new Dictionary<string, double>()
                        {
                                { "191", 0.0002357643},
                                { "192", 0.0000396992},
                                { "193", 0.0000506687},
                                { "194", 0},
                                { "195", 0.000018285},
                                { "196", 0},
                                { "197", 0.0000030019},
                                { "198", 0},
                                { "199", 0},
                                { "200", 0},
                                { "201", 0},
                                { "202", 0},
                                { "203", 0},
                                { "204", 0},
                                { "205", 0.002247559}
                        };

                        SRHg.TryGetValue(label6.Text, out k2);
                    }
                }
                // Расчитываем значения точек графиков функций
                CalcFunction(k1, k2);
                // Добавляем вычисленные значения в графики
                chart1.Series[0].Points.DataBindXY(x, y1);
                chart1.Series[1].Points.DataBindXY(x, y2);

            }
        }
    }
}
