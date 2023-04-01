using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TaxiClient.Model
{
    public class CarItem : INotifyPropertyChanged
    {
        private int id;
        private string autoModel;
        private string driver;
        private bool isEnabled;

        public CarItem()
        {
            id = -1;
            isEnabled = true;
        }
        public CarItem(int id, string autoModel, string driver, bool isEnabled)
        {
            this.id = id;
            this.autoModel = autoModel;
            this.driver = driver;
            this.isEnabled = isEnabled;
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        public string AutoModel
        {
            get { return autoModel; }
            set
            {
                autoModel = value;
                NotifyPropertyChanged("AutoModel");
            }
        }
        public string Driver
        {
            get { return driver; }
            set
            {
                driver = value;
                NotifyPropertyChanged("Driver");
            }
        }
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                NotifyPropertyChanged("IsEnabled");
            }
        }

        public override string ToString()
        {
            return $"Car: {AutoModel} Driver: {Driver}";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
