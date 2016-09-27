using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Task_3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            textBox3.Text = Math.Pow(2, 32).ToString();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            int n = Convert.ToInt16(textBox.Text);
            loading.Maximum = n;
            loading.Value = 0;
            if (comboBox.SelectedIndex == 0)
            {
                Rand r = new Rand(Convert.ToDouble(textBox1.Text), Convert.ToDouble(textBox2.Text), Convert.ToDouble(textBox3.Text));
                r.x = double.Parse(tx.Text);
                r.y = double.Parse(ty.Text);
                for (int i = 0; i < n; i++)
                {
                    Data.AddRand(r.Next());
                    loading.Value++;
                }
            }
            else if (comboBox.SelectedIndex == 1)
            {
                FRand r = new FRand(Convert.ToInt32(textBox1.Text),Convert.ToInt32(textBox.Text));
                for (int i = 0; i < n; i++)
                {
                    Data.AddRandF(r.Next());
                    loading.Value++;
                }
            }
            else
            {
                PI pi = new PI(Convert.ToInt32(textBox1.Text), Convert.ToInt32(textBox2.Text));
                for (int i = 0; i < n; i++)
                {
                    Data.AddPi(pi.Value);
                    pi.Calculate();
                    loading.Value++;
                }
            }

            listUpdate();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Data.Clear((Lists)comboBox.SelectedIndex);
            listUpdate();
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!this.IsLoaded)
            {
                return;
            }
            int s = comboBox.SelectedIndex;
            if (s == 0)
            {
                label1.Content = "a =";
                textBox1.Text = "1664525";

                label2.Content = "c =";
                textBox2.Text = "1013904223";

                textBox3.IsEnabled = true;
                label3.Content = "m =";
                textBox3.Text = Math.Pow(2, 32).ToString(CultureInfo.CurrentCulture);
            }
            else if (s == 1)
            {
                label1.Content = "a =";
                textBox1.Text = "17";

                label2.Content = "b =";
                textBox2.Text = "5";

                textBox3.IsEnabled = false;
                label3.Content = "";
                textBox3.Text = "";
            }
            else
            {
                label1.Content = "r =";
                textBox1.Text = "10000";

                label2.Content = "N =";
                textBox2.Text = "100000";

                textBox3.IsEnabled = false;
                label3.Content = "";
                textBox3.Text = "";
            }

            listUpdate();
        }

        void listUpdate()
        {
            int s = comboBox.SelectedIndex;
            ListBox.Items.Clear();
            if (s == 0)
            {
                foreach (var e in Data.rand)
                {
                    ListBox.Items.Add(e);
                }
            }
            else if (s == 1)
            {
                foreach (var e in Data.randF)
                {
                    ListBox.Items.Add(e);
                }
            }
            else
            {
                foreach (var e in Data.pi)
                {
                    ListBox.Items.Add(e);
                }
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            int s = comboBox.SelectedIndex;
            Graph g;
            int n;

            List<double> nList = new List<double>();

            if (s == 0)
            {
                for (int i = 0; i < Data.rand.Count; i++)
                {
                    nList.Add(i + 1);
                }
                g = new Graph(Data.rand, nList);
            }
            else if (s == 1)
            {
                for (int i = 0; i < Data.randF.Count; i++)
                {
                    nList.Add(i + 1);
                }
                g = new Graph(Data.randF, nList);
            }
            else
            {
                for (int i = 0; i < Data.pi.Count; i++)
                {
                    nList.Add(i + 1);
                }
                g = new Graph(Data.pi, nList);
            }

            g.ShowDialog();

            IsEnabled = true;
        }
    }
}
