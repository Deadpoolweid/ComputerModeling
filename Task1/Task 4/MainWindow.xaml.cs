using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
using Task1;
using Task4;
using Random = System.Random;

namespace Task_4
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private int aMax, bMax;

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Data.Clear();

            DataTable table = new DataTable();

            table.Columns.Add("№");
            table.Columns.Add("A");
            table.Columns.Add("B");
            table.Columns.Add("C");
            table.Columns.Add("D");
            table.Columns.Add("E");
            table.Columns.Add("F");
            table.Columns.Add("G");
            table.Columns.Add("H");

            Random r = new Random(DateTime.Now.Millisecond);

            aMax = Convert.ToInt32(textBox.Text);
            bMax = Convert.ToInt32(textBox_Copy.Text);

            for (int i = 0; i < 5; i++)
            {


                Data.A.Add(r.Next(1, aMax));

                Data.B.Add(r.Next(1, bMax));

                if (i == 0)
                {
                    Data.C.Add(0);
                    Data.D.Add(0);
                    Data.E.Add(Data.B[i]);
                    Data.F.Add(Data.E[i] - Data.C[i]);
                    Data.G.Add(0);
                    Data.H.Add(0);
                }
                else
                {
                    Data.C.Add(Data.C[i-1] + Data.A[i]);
                    Data.D.Add(Math.Max(Data.C[i], Data.E[i-1]));
                    Data.E.Add(Data.D[i] + Data.B[i]);
                    Data.F.Add(Data.E[i] - Data.C[i]);
                    Data.G.Add(Data.F[i]-Data.B[i]);
                    Data.H.Add(Data.D[i] - Data.E[i-1]);
                }

                table.Rows.Add(i + 1, Data.A[i], Data.B[i], Data.C[i], Data.D[i], Data.E[i], Data.F[i],
                    Data.G[i], Data.H[i]);
            }

            data.ItemsSource = table.DefaultView;

            int min = Data.H.Min();

            int max = Data.H.Max();

            var info = new DataTable();

            info.Columns.Add("Значение");
            info.Columns.Add("Плотность");

            info.TableName = "H";


            for (int i = min; i <= max; i++)
            {
                info.Rows.Add(i,Data.H.Count(z => z.Equals(i)));
            }



            dgInfo.ItemsSource = info.DefaultView;



            label.Content = "g среднее = " + Data.G.Average();

            label1.Content = "h среднее = " + Data.H.Average();

            List<int> x = new List<int>();
            List<int> y = new List<int>();


            foreach (DataRow j in info.Rows)
            {
                x.Add(Convert.ToInt32(j.ItemArray[0]));
                y.Add(Convert.ToInt32(j.ItemArray[1]));
            }

            Draw(x,y);




        }

        void Draw(List<int> n, List<int> x)
        {
            KeyValuePair<int,int>[] pairs = new KeyValuePair<int, int>[n.Count];

            for (var i = 0; i < n.Count; i++)
            {
                pairs[i] = new KeyValuePair<int, int>(n[i],x[i]);
            }


            //LineSeries.ItemsSource = points;

            BarSeries.ItemsSource = pairs;

            //Chart.DataContext = dataSourceList;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            IsEnabled = false;


            List<int> Second = new List<int>();
            Random r = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < 5; i++)
            {
                Second.Add(r.Next(1,aMax));
            }

            Pearson w = new Pearson(Data.A.ToArray(), Second.ToArray());
            
            w.Show();

            IsEnabled = true;
        }
    }
}
