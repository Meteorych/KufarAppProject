using System.Windows;

namespace KufarAppProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var kufar = new KufarApi();
        }

        private void Task1_Click(object sender, RoutedEventArgs e)
        {
            var window = new PlotsWindow();
            window.Show();
        }

        private void Task2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Task3_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}