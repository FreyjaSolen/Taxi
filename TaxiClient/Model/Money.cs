using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TaxiClient.Model
{
    public class Money : INotifyPropertyChanged
    {
        double sum;
        private int discount;

        public Money() 
        {
            sum = 0;
            discount = 0;
        }

        public double Sum
        {
            get { return sum; }
            set
            {
                sum = value;
                NotifyPropertyChanged("Sum");
            }
        }
        public int Discount
        {
            get { return discount; }
            set
            {
                discount = value;
                NotifyPropertyChanged("Discount");
            }
        }

        public void calcDisc()
        {
            if (sum > 1000)
            {
                discount = 10;
            }
            else if (sum > 500)
            {
                discount = 5;
            }
            else discount = 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
