using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TaxiClient.Model
{
    public class Ride : INotifyPropertyChanged
    {
        private string startPoint;
        private string endPoint;
        private DateTime expTime;
        private double expPrice;

        public Ride() 
        {
            startPoint = "";
            endPoint = "";
            expTime = new DateTime();
            expTime = DateTime.Now;
            expPrice = 0;
        }

        public string StartPoint
        {
            get { return startPoint; }
            set
            {
                startPoint = value;
                NotifyPropertyChanged("StartPoint");
            }
        }
        public string EndPoint
        {
            get { return endPoint; }
            set
            {
                endPoint = value;
                NotifyPropertyChanged("EndPoint");
            }
        }
        public DateTime ExpTime
        {
            get { return expTime; }
            set
            {
                expTime = value;
                NotifyPropertyChanged("ExpTime");
            }
        }
        public double ExpPrice
        {
            get { return expPrice; }
            set
            {
                expPrice = value;
                NotifyPropertyChanged("ExpPrice");
            }
        }

        public void cleanField()
        {
            StartPoint = "";
            EndPoint = "";
            ExpPrice = 0;
            ExpTime = DateTime.Now;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
