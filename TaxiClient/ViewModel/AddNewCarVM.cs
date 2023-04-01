using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using TaxiClient.Command;
using TaxiClient.Exceptions;
using TaxiClient.Model;
using TaxiRepository.Models;
using TaxiRepository.Repository;

namespace TaxiClient.ViewModel
{
    public class AddNewCarVM : INotifyPropertyChanged
    {
        private CarItem carItemVM;

        public AddNewCarVM()
        {
            carItemVM = new CarItem();
        }

        public CarItem CarItemVM
        {
            get { return carItemVM; }
            set
            {
                carItemVM = value;
                CarPropertyChanged("CarItemVM");
            }
        }

        private SimpleCommand applyCommand;
        public SimpleCommand ApplyCommand
        {
            get
            {
                return applyCommand ??
                  (applyCommand = new SimpleCommand(obj =>
                  {
                      try
                      {
                          if (CarItemVM.AutoModel != null && CarItemVM.Driver != null)
                          {
                              addCar();
                          }
                          else
                          {
                              throw new NullFields();
                          }
                          closeWindow();
                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show(ex.Message);
                      }                     
                  }));
            }
        }

        private SimpleCommand cancelCommand;
        public SimpleCommand CancelCommand
        {
            get
            {
                return cancelCommand ??
                  (cancelCommand = new SimpleCommand(obj =>
                  {
                      closeWindow();
                  }));
            }
        }

        private void addCar()
        {
            CarsRepository carsRepository = new CarsRepository("taxi_local");
            CarRecord carRecord = new CarRecord(CarItemVM.ID, CarItemVM.AutoModel, CarItemVM.Driver, CarItemVM.IsEnabled);
            try
            {
                carsRepository.AddCarRecord(carRecord);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void closeWindow()
        {
            foreach (System.Windows.Window window in System.Windows.Application.Current.Windows)
            {
                if (window.DataContext == this)
                {
                    window.Close();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void CarPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
