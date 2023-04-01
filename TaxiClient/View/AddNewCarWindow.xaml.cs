using System.Windows;
using TaxiClient.ViewModel;

namespace TaxiClient.View
{
    public partial class AddNewCarWindow : Window
    {
        public AddNewCarWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new AddNewCarVM();
        }
    }
}
