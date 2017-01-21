using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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

        private int ExperimentsCount = 1000;

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

            for (int i = 0; i < ExperimentsCount; i++)
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


            var style = new Style(typeof(DataGridCell));
            style.Setters.Add(new Setter(ToolTipProperty,"Промежутки между приходами клиентов (в минутах)"));
            data.Columns[1].CellStyle= style;
            style = new Style(typeof(DataGridCell));
            style.Setters.Add(new Setter(ToolTipProperty,"Длительности обслуживания (в минутах)"));
            data.Columns[2].CellStyle = style;
            style = new Style(typeof(DataGridCell));
            style.Setters.Add(new Setter(ToolTipProperty, "Условное время прихода клиента"));
            data.Columns[3].CellStyle = style;
            style = new Style(typeof(DataGridCell));
            style.Setters.Add(new Setter(ToolTipProperty, "Момент начала обслуживания"));
            data.Columns[4].CellStyle = style;
            style = new Style(typeof(DataGridCell));
            style.Setters.Add(new Setter(ToolTipProperty, "Момент конца обслуживания"));
            data.Columns[5].CellStyle = style;
            style = new Style(typeof(DataGridCell));
            style.Setters.Add(new Setter(ToolTipProperty, "Длительность времени, проведённого клиентом в системе в целом"));
            data.Columns[6].CellStyle = style;
            style = new Style(typeof(DataGridCell));
            style.Setters.Add(new Setter(ToolTipProperty, "Длительность времени, проведённого клиентом в ожидании обслуживания"));
            data.Columns[7].CellStyle = style;
            style = new Style(typeof(DataGridCell));
            style.Setters.Add(new Setter(ToolTipProperty, "Время, проведённое системой в ожидании клиентов"));
            data.Columns[8].CellStyle = style;

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



            label.Content = "gср = " + Data.G.Average();

            label1.Content = "hср = " + Data.H.Average();

            List<int> x = new List<int>();
            List<int> y = new List<int>();


            foreach (DataRow j in info.Rows)
            {
                x.Add(Convert.ToInt32(j.ItemArray[0]));
                y.Add(Convert.ToInt32(j.ItemArray[1]));
            }

            Draw(x,y);




        }

        private dData execExperiment(int n)
        {
            List<int> a, b, c, d, e, f, g, h;

            a = new List<int>();
            b = new List<int>();
            c = new List<int>();
            d = new List<int>();
            e = new List<int>();
            f = new List<int>();
            g = new List<int>();
            h = new List<int>();

            Random r = new Random(DateTime.Now.Millisecond+1);

            aMax = Convert.ToInt32(textBox.Text);
            bMax = Convert.ToInt32(textBox_Copy.Text);

            for (int i = 0; i < ExperimentsCount; i++)
            {


                a.Add(r.Next(1, aMax));

                b.Add(r.Next(1, bMax));

                if (i == 0)
                {
                    c.Add(0);
                    d.Add(0);
                    e.Add(b[i]);
                    f.Add(e[i] - c[i]);
                    g.Add(0);
                    h.Add(0);
                }
                else
                {
                    c.Add(c[i - 1] + a[i]);
                    d.Add(Math.Max(c[i], e[i - 1]));
                    e.Add(d[i] + b[i]);
                    f.Add(e[i] - c[i]);
                    g.Add(f[i] - b[i]);
                    h.Add(d[i] - e[i - 1]);
                }

            }

            return new dData(a,b,c,d,e,f,g,h);
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

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            IsEnabled = false;

            dData datamain = new dData(Data.A,Data.B, Data.C, Data.D, Data.E, Data.F, Data.G,Data.H);

            dData data = execExperiment(ExperimentsCount);
            Fisher f = new Fisher(Data.H.ToArray(),data.H.ToArray());
            f.Show();

            IsEnabled = true;

        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            IsEnabled = false;

            dData data = execExperiment(ExperimentsCount);

            Student s = new Student(Data.H.ToArray(),data.H.ToArray());
            s.Show();
            IsEnabled = true;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            IsEnabled = false;


            List<int> Second = new List<int>();
            Random r = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < ExperimentsCount; i++)
            {
                Second.Add(r.Next(1,aMax));
            }

            Pearson w = new Pearson(Data.A.ToArray(), Second.ToArray());
            
            w.Show();

            IsEnabled = true;
        }
    }
}
