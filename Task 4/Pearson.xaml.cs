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
        public Pearson(int[] Fe, int[] Ft)
        {
            InitializeComponent();
            this.Fe = Fe;
            this.Ft = Ft;

            object[] items = {
                "0.995",
                "0.99",
                "0.975",
                "0.95",
                "0.9",
                "0.75",
                "0.5",
                "0.25",
                "0.1",
                "0.05",
                "0.025",
                "0.01",
                "0.005"
            };

            comboBox.ItemsSource = items;
            comboBox.SelectedItem = "0.05";
        }

        private int[] Fe;

        private int[] Ft;

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


            if (xi <= calcPearson())
            {
                lResult.Content = "Различия отсутствуют.";
            }
            else
            {
                lResult.Content = "Различия значимы на уровне " + comboBox.SelectedValue + "%.";
            }
        }

        private double calcPearson()
        {
            int p = comboBox.SelectedIndex;
            switch (p)
            {
                case 0:
                    return 0.20699;
                case 1:
                    return 0.29711;
                case 2:
                    return 0.48442;
                case 3:
                    return 0.71072;
                case 4:
                    return 1.06362;
                case 5:
                    return 1.92256;
                case 6:
                    return 3.35669;
                case 7:
                    return 5.38527;
                case 8:
                    return 7.77944;
                case 9:
                    return 9.48773;
                case 10:
                    return 11.14329;
                case 11:
                    return 13.27670;
                case 12:
                    return 14.86026;
            }
            return default(double);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            main(Fe, Ft);
        }
    }
}
