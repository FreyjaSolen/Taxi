using System.Windows;
using TaxiClient.ViewModel;

namespace TaxiClient.View
{
    public partial class ManagerWindow : Window
    {
        public ManagerWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new ManagerWindowVM();
        }
    }
}
