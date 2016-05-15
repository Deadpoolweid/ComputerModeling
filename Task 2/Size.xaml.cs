using System.Windows;

namespace Task2
{
    /// <summary>
    /// Логика взаимодействия для Size.xaml
    /// </summary>
    public partial class Size
    {
        public Size(int n)
        {
            InitializeComponent();
            for (var i = 2; i < n; i++)
            {
                if (n%i == 0)
                {
                    Combobox.Items.Add(i);
                }
            }
            Combobox.SelectedIndex = 0;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
