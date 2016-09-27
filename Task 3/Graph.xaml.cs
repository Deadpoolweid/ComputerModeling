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
using Task3;

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

            var numbers = new List<double>();

            var numberRepeats = new Dictionary<double,int>();

            foreach (var number in x)
            {
                if (!numbers.Exists(a => a == number))
                {
                    numbers.Add(number);
                    numberRepeats.Add(number,1);
                }
                else
                {
                    numberRepeats[number]++;
                }
            }

            

            var ClashCount = new List<int>();

            foreach (var number in numbers)
            {
                ClashCount.Add(numberRepeats[number]);
            }

            

            for (var i = 0; i < numbers.Count; i++)
            {
                points.Add(new Coord(numbers[i], ClashCount[i]));
            }

            var dataSourceList = new List<ObservableCollection<Coord>> { points };
            //LineSeries.ItemsSource = points;

            Chart.DataContext = dataSourceList;
        }
    }
}
