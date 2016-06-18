using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Task_4
{
    /// <summary>
    /// Логика взаимодействия для Fisher.xaml
    /// </summary>
    public partial class Fisher : Window
    {
        public Fisher(int[] x, int[] y)
        {
            InitializeComponent();

            this.x = x;
            this.y = y;

            main(x,y);
        }

        private int[] x, y;

        public void main(int[] x, int[] y)
        {
            DataTable table = new DataTable("Fisher");

            table.Columns.Add("i");
            table.Columns.Add("X");
            table.Columns.Add("Y");

            int n = x.Length;

            for (int i = 0; i < n; i++)
            {
                DataRow dr = table.NewRow();

                dr.ItemArray = new object[]
                {
                    i+1,
                    x[i],
                    y[i]
                };

                table.Rows.Add(dr);
            }

            DataRow dr1 = table.NewRow();

            dr1.ItemArray = new object[]
            {
                "D()",
                D(x),
                D(y)
            };

            table.Rows.Add(dr1);

            dataGrid.ItemsSource = table.DefaultView;

            double F;

            if (D(x) > D(y))
            {
                F = D(x)/D(y);
            }
            else
            {
                F = D(y)/D(x);
            }

            lF.Content = "Fэмп = " + F;

            if (F <= 5.05)
            {
                label.Content = "Принимается нулевая гипотеза.";
            }
            else
            {
                label.Content = "Принимается альтернативная гипотеза.";
            }


        }

        private double D(int[] x)
        {
            List<double> diff = new List<double>();

            double xa = x.Average();

            for (int i = 0; i < x.Length; i++)
            {
                diff.Add(Math.Pow(x[i]-xa,2));
            }

            return diff.Sum()/x.Count();
        }
    }
}
