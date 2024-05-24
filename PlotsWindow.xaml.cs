using KufarAppProject.ApiClasses;
using System.Windows;

namespace KufarAppProject
{
    /// <summary>
    /// Interaction logic for PlotsWindow.xaml
    /// </summary>
    public partial class PlotsWindow : Window
    {
        private readonly KufarApi _kufarApi;
        private readonly PlotsWindowModel _model;

        public PlotsWindow(KufarApi kufarApi)
        {
            _kufarApi = kufarApi;
            _model = new PlotsWindowModel(_kufarApi);
            DataContext = _model;
            InitializeComponent();
        }
    }
}
