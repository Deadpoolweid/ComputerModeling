using System;
using System.Collections.Generic;
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
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            int n = Convert.ToInt16(textBox.Text);
            loading.Maximum = n;
            loading.Value = 0;
            if (comboBox.SelectedIndex == 0)
            {
                Rand r = new Rand();
                for (int i = 0; i < n; i++)
                {
                    listBox.Items.Add(r.Next());
                    loading.Value++;
                }
            }
            else
            {
                FRand r = new FRand();
                for (int i = 0; i < n; i++)
                {
                    listBox.Items.Add(r.Next());
                    loading.Value++;
                }
            }



        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            listBox.Items.Clear();
        }
    }
}
