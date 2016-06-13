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
    /// Логика взаимодействия для Pearson.xaml
    /// </summary>
    public partial class Pearson : Window
    {
        public Pearson()
        {
            InitializeComponent();
        }

        public void main(int[] Fe, int[] Ft)
        {
            DataTable table = new DataTable("Pearson");

            table.Columns.Add("i");
            table.Columns.Add("fэ");
            table.Columns.Add("fт");
            table.Columns.Add("fэ-fт");
            table.Columns.Add("(fэ-fт)^2");
            table.Columns.Add("(fэ-fт)^2fт");

            double xi = 0;

            for (int i = 0; i < Fe.Count(); i++)
            {
                DataRow dr = table.NewRow();

                dr.ItemArray = new object[]
                {
                    i+1,
                    Fe[i],
                    Ft[i],
                    Fe[i]-Ft[i],
                    Math.Pow(Fe[i]-Ft[i],2),
                    Math.Pow(Fe[i]-Ft[i],2)/Ft[i]
                };

                xi += Math.Pow(Fe[i] - Ft[i], 2)/Ft[i];
                table.Rows.Add(dr);
            }

            DataRow dr1 = table.NewRow();

            dr1.ItemArray = new object[]
            {
                "Xэмп",
                "",
                "",
                "",
                "",
                xi
            };

            table.Rows.Add(dr1);

            dataGrid.ItemsSource = table.DefaultView;
        }
    }
}
