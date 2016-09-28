using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

namespace Launcher
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            comboBox.SelectedIndex = 0;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Hide();

            int c = comboBox.SelectedIndex + 1;

            switch (c)
            {
                case 1:
                    var app1 = new Task1.MainWindow();
                    app1.ShowDialog();
                    break;
                case 2:
                    var app2 = new Task2.MainWindow();
                    app2.ShowDialog();
                    break;
                case 3:
                    var app3 = new Task_3.MainWindow();
                    app3.ShowDialog();
                    break;
                case 4:
                    var app4 = new Task_4.MainWindow();
                    app4.ShowDialog();
                    break;
                case 5:
                    var app5 = new GameOfLife.MainWindow();
                    app5.ShowDialog();
                    break;
            }

            Show();
        }

        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }
    }
}
