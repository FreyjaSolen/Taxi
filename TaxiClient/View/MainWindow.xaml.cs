using System.Windows;
using TaxiClient.ViewModel;

namespace TaxiClient
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new MainWindowVM();
        }
    }
}
