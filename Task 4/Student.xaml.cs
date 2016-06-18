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
    /// Логика взаимодействия для Student.xaml
    /// </summary>
    public partial class Student : Window
    {
        public Student(int[] x, int[] y)
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
                "Avg",
                x.Average(),
                y.Average()
            };

            table.Rows.Add(dr1);

            dataGrid.ItemsSource = table.DefaultView;

            double t = (Math.Abs(x.Average()-y.Average()))/(diffD());

            label.Content = "Разность отклонений - " + diffD();

            label1.Content = "tэмп = " + t;

            if (t >= 2.5706)
            {
                lResult.Content = "Гипотеза равенства средних отклонена.";
            }
            else
            {
                lResult.Content = "Гипотеза равенства средних принята.";
            }


        }

        /// <summary>
        /// Расчёт стандартной ошибки разности арифметических средних
        /// </summary>
        /// <returns>Стандартная ошибка разности арифметических средних</returns>
        double diffD()
        {
            double A, B;

            int nx = x.Length;
            int ny = y.Length;

            double xa = x.Average();
            double ya = y.Average();

            List<double> Ax = new List<double>();

            for (int i = 0; i < nx; i++)
            {
                Ax.Add(Math.Pow(x[i]-xa,2));
            }

            List<double> Ay = new List<double>();

            for (int i = 0; i < ny; i++)
            {
                Ay.Add(Math.Pow(y[i]-ya,2));
            }

            A = Ax.Sum() + Ay.Sum();

            B = (1d/nx + 1d/ny)/(nx+ny-2);

            return Math.Sqrt(A*B);
        }
    }
}
