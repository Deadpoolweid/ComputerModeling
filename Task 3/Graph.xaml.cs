using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Task_3
{
    /// <summary>
    /// Логика взаимодействия для Graph.xaml
    /// </summary>
    public partial class Graph : Window
    {
        public Graph()
        {
            InitializeComponent();
        }

        public Graph(List<double> n, List<double> x)
        {
            InitializeComponent();
            Draw(x,n);
        }

        void Draw(List<double> n, List<double> x)
        {
            var points = new ObservableCollection<Coord>();

            for (var i = 0; i < n.Count; i++)
            {
                points.Add(new Coord(n[i], x[i]));
            }

            var dataSourceList = new List<ObservableCollection<Coord>> { points };
            //LineSeries.ItemsSource = points;

            Chart.DataContext = dataSourceList;
        }
    }
}
