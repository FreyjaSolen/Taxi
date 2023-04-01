using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using TaxiClient.Command;
using TaxiClient.Model;
using TaxiClient.View;
using TaxiRepository.Models;
using TaxiRepository.Repository;

namespace TaxiClient.ViewModel
{
    public class ManagerWindowVM : INotifyPropertyChanged
    {
        private CarItem selectedCarItem;
        private ObservableCollection<CarItem> cars;

        private CarsRepository carsRepository;

        public ManagerWindowVM()
        {
            try
            {
                Cars = addCarItems();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public CarItem SelectedCarItem
        {
            get { return selectedCarItem; }
            set
            {
                selectedCarItem = value;
                MngClPropertyChanged("SelectedCarItem");
            }
        }
        public ObservableCollection<CarItem> Cars
        {
            get
            {
                return cars;
            }
            set
            {
                cars = value;
                MngClPropertyChanged("Cars");
            }
        }

        private SimpleCommand addCarCommand;
        public SimpleCommand AddCarCommand
        {
            get
            {
                return addCarCommand ??
                  (addCarCommand = new SimpleCommand(obj =>
                  {
                      try
                      {
                          AddNewCarWindow w = new AddNewCarWindow();
                          w.Show();
                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show(ex.Message);
                      }
                  }));
            }
        }

        private SimpleCommand updateCommand;
        public SimpleCommand UpdateCommand
        {
            get
            {
                return updateCommand ??
                  (updateCommand = new SimpleCommand(obj =>
                  {
                      try
                      {
                          Cars = addCarItems();
                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show(ex.Message);
                      }
                  }));
            }
        }

        private SimpleCommand enabledCommand;
        public SimpleCommand EnabledCommand
        {
            get
            {
                return enabledCommand ??
                  (enabledCommand = new SimpleCommand(obj =>
                  {
                      try
                      {
                          string quession = $"Do you really want to change the status car with ID {SelectedCarItem.ID}?";
                          MessageBoxResult result = MessageBox.Show(quession, "Confirm action", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                          if (result == MessageBoxResult.Yes)
                          {
                              ChangeEnabling();
                          }
                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show(ex.Message);
                      }
                  },
                (obj) => SelectedCarItem != null));
            }
        }

        private SimpleCommand helpCommand;
        public SimpleCommand HelpCommand
        {
            get
            {
                return helpCommand ??
                  (helpCommand = new SimpleCommand(obj =>
                  {
                      string message = "Welcome to the program.\n" +
                      "To add a new machine, click the button \"Add new car and driver\".\n" +
                      "To change the machine status, press the button \"Сhange availability\".\n" +
                      "Thank you for using our service.";
                      MessageBox.Show(message);
                  }));
            }
        }

        private SimpleCommand changeUserCommand;
        public SimpleCommand ChangeUserCommand
        {
            get
            {
                return changeUserCommand ??
                  (changeUserCommand = new SimpleCommand(obj =>
                  {
                      makeMainWindow();
                  }));
            }
        }

        private SimpleCommand exitCommand;
        public SimpleCommand ExitCommand
        {
            get
            {
                return exitCommand ??
                  (exitCommand = new SimpleCommand(obj =>
                  {
                      closeProgram();
                  }));
            }
        }

        private ObservableCollection<CarItem> addCarItems()
        {
            carsRepository = new CarsRepository("taxi_local");
            ObservableCollection<CarItem> carsItem = new ObservableCollection<CarItem>();

            try
            {
                List<CarRecord> cr = carsRepository.getCars();

                foreach (var item in cr)
                {
                    CarItem current = new CarItem(item.id, item.Car, item.Driver, item.IsEnable);

                    carsItem.Add(current);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return carsItem;
        }

       private void ChangeEnabling()
        {
            carsRepository = new CarsRepository("taxi_local");

            try
            {
                carsRepository.SetCarEnabled(SelectedCarItem.ID, !SelectedCarItem.IsEnabled);

                foreach (var item in Cars)
                {
                    if(item.ID == SelectedCarItem.ID)
                    {
                        item.IsEnabled = !SelectedCarItem.IsEnabled;
                        return;
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void makeMainWindow()
        {
            MainWindow w = new MainWindow();
            w.Show();
            Application.Current.Windows[0].Close();
        }

        private void closeProgram()
        {
            System.Windows.Application.Current.Shutdown();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void MngClPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
