using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace GameOfLife
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MapSize = (int)sMapSize.Value;
            sMapSize.ValueChanged += sMapSize_OnValueChanged;
            Speed = (int) sSpeed.Value;
            sSpeed.ValueChanged+= SSpeedOnValueChanged;
            Map = new bool[MapSize,MapSize];
            Timer = new Timer(OnTimerTick,this,0,1000);
        }

        private void SSpeedOnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> routedPropertyChangedEventArgs)
        {
            init();
        }

        void OnTimerTick(object state)
        {
            var window = state as MainWindow;
            this.Dispatcher.Invoke(() =>
            {
                window.Title = DateTime.Now.ToLongTimeString();
            });
            
        }

        void OnTimerTickPlay(object state)
        {
            GoStep();
            Dispatcher.Invoke(DrawMap);
        }

        private System.Threading.Timer Timer;


        /// <summary>
        /// Размер карты
        /// </summary>
        int MapSize = 10;

        private int Speed;

        /// <summary>
        /// Количество итераций
        /// </summary>
        private int NumberOfIterations;

        /// <summary>
        /// Массив карты
        /// </summary>
        bool[,] Map;


        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            lSize.Content = "Size : " + MapSize;
            lStep.Content = "Step : " + NumberOfIterations.ToString();
            init();
        }

        /// <summary>
        /// Инициализация карты
        /// </summary>
        private void init()
        {
            MapSize = (int)sMapSize.Value;
            lSize.Content = "Size : " + MapSize;
            Speed = (int) sSpeed.Value;
            lSpeed.Content = "Speed : " + Speed;
            Map = new bool[MapSize, MapSize];

            ugMap.Children.Clear();
            NumberOfIterations = 0;

            for (int i = 0; i < MapSize; i++)
            {
                for (int j = 0; j < MapSize; j++)
                {
                    var rectangle = new Rectangle();
                    rectangle.Fill = Brushes.Bisque;
                    rectangle.Stroke = Brushes.DarkSlateGray;
                    rectangle.Tag = $"{j}:{i}";
                    rectangle.Width = ugMap.Width / (MapSize);
                    rectangle.Height = ugMap.Height / MapSize;
                    ugMap.Children.Add(rectangle);

                    Map[j, i] = false;
                }
            }
        }

        private void UgMap_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                ChangeCell(sender,true);
            }
        }

        private string _previousCell;

        private void ChangeCell(object sender, bool isMouseMoving = false)
        {
            var uniformGrid = sender as UniformGrid;
            var Items = uniformGrid.Children;
            foreach (var item in Items)
            {
                var rectangle = item as Rectangle;
                if (rectangle.IsMouseOver)
                {
                    if (isMouseMoving && _previousCell == rectangle.Tag.ToString())
                    {
                        return;
                    }
                    rectangle.Fill = Equals(rectangle.Fill, Brushes.Bisque)?Brushes.Green:Brushes.Bisque;
                    string coords = rectangle.Tag.ToString();
                    int x = int.Parse(coords.Split(':')[0]);
                    int y = int.Parse(coords.Split(':')[1]);
                    Map[x, y] = !Map[x,y];

                    _previousCell = coords;
                }
            }
        }

        private void sMapSize_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            init();
        }

        private void UgMap_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ChangeCell(sender);

        }

        private void Clear_OnClick(object sender, RoutedEventArgs e)
        {
            var Items = ugMap.Children;
            foreach (var item in Items)
            {
                var rectangle = item as Rectangle;
                rectangle.Fill = Brushes.Bisque;
                string coords = rectangle.Tag.ToString();
                int x = int.Parse(coords.Split(':')[0]);
                int y = int.Parse(coords.Split(':')[1]);
                Map[x, y] = false;
            }
        }

        void DrawMap()
        {
            var Items = ugMap.Children;
            foreach (var item in Items)
            {
                var rectangle = item as Rectangle;
                string coords = rectangle.Tag.ToString();
                int x = int.Parse(coords.Split(':')[0]);
                int y = int.Parse(coords.Split(':')[1]);
                rectangle.Fill = Map[x,y] ? Brushes.Green : Brushes.Bisque;
            }

            lStep.Content = "Step : " + CurrentIteration;
        }

        private bool IsStarted = false;

        private void Start_OnClick(object sender, RoutedEventArgs e)
        {
            if (IsStarted)
            {
                IsStarted = false;
                CurrentIteration = 0;
                bStart.Content = "Play";
                Timer.Dispose();
                Timer = new Timer(OnTimerTick,this,0,1000);
                return;
            }

            bStart.Content = "Stop";
            IsStarted = true;
            Timer.Dispose();
            Timer = new Timer(OnTimerTickPlay,null,0,Speed);
        }

        private void Step_OnClick(object sender, RoutedEventArgs e)
        {
            GoStep();

            DrawMap();
        }

        void GoStep()
        {
            var MapAfterStep = new bool[MapSize,MapSize];

            for (int j = 0; j < MapSize; j++)
            {
                for (int i = 0; i < MapSize; i++)
                {
                    MapAfterStep[j,i] = Map[j, i];
                }
            }

            for (int j = 0; j < MapSize; j++)
            {
                for (int i = 0; i < MapSize; i++)
                {
                    int numberOfNeighbours = 0;

                    bool[] list =
                    {
                        Map[Convert(j - 1), Convert(i - 1)],
                        Map[j, Convert(i - 1)],
                        Map[Convert(j + 1), Convert(i - 1)],
                        Map[Convert(j - 1), i],
                        Map[Convert(j + 1), i],
                        Map[Convert(j - 1), Convert(i + 1)],
                        Map[j, Convert(i + 1)],
                        Map[Convert(j + 1), Convert(i + 1)]
                    };
                    List<bool> listOfNeighbours = new List<bool>(list);

                    numberOfNeighbours = listOfNeighbours.Count(item => item);

                    if (numberOfNeighbours == 3)
                    {
                        MapAfterStep[j, i] = true;
                    }
                    else if (numberOfNeighbours>3 || numberOfNeighbours<2)
                    {
                        MapAfterStep[j, i] = false;
                    }

                }
            }

            Map = MapAfterStep;
            CurrentIteration++;
        }

        private int CurrentIteration = 0;

        int Convert(int number)
        {
            if (number == -1)
            {
                return MapSize - 1;
            }
            if (number == MapSize)
            {
                return 0;
            }
            return number;
        }
    }
}
