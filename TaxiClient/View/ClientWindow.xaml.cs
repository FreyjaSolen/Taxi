using System.Windows;
using TaxiClient.ViewModel;

namespace TaxiClient.View
{
    public partial class ClientWindow : Window
    {
        public ClientWindow(int ID)
        {
            InitializeComponent();

            DataContext = new ClientWindowVM(ID);
        }
    }
}
