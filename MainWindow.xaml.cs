using System.Windows;

namespace KufarAppProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PlotsWindow _plotsWindow;
        private KufarApi _kufarApi;

        public MainWindow()
        {
            InitializeComponent();
            _kufarApi = new KufarApi();
            _plotsWindow = new PlotsWindow(_kufarApi);
        }

        private void Task1_Click(object sender, RoutedEventArgs e)
        {
            _plotsWindow.Show();
        }

        private void Task2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Task3_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}