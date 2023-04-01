using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class ClientWindowVM : INotifyPropertyChanged
    {
        private const int COAST_OF_MIN = 5;
        private const string NAME_OF_BASE = "taxi_local";
        private bool readyToGo = true;

        private int id;
        private Money userDisc;
        private Ride ride;
        private CarsRepository carsRepository;
        private RecordRepository recordRepository;
 
        private CarItem selectedCarItem;

        private ObservableCollection<CarItem> cars { get; set; }

        public ClientWindowVM(int ID)
        {
            try
            {
                id = ID;
                userDisc = new Money();
                Cars = addCarItems();
                calculateMoney(id);
                ride = new Ride();               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public bool ReadyToGo
        {
            get { return readyToGo; }

            set
            {
                if (readyToGo == value)
                {
                    return;
                }

                readyToGo = value;
                OnPropertyChanged("ReadyToGo");
            }
        }
        public Ride UserRide
        {
            get { return ride; }
            set
            {
                if (readyToGo == true)
                {
                    return;
                }
                ride = value;
                OnPropertyChanged("UserRide");
            }
        }
        public Money UserDisc
        {
            get { return userDisc; }
            set
            {
                userDisc = value;
                OnPropertyChanged("UserDisc");
            }
        }
        public CarItem SelectedCarItem
        {
            get { return selectedCarItem; }
            set
            {
                selectedCarItem = value;
                OnPropertyChanged("SelectedCarItem");
            }
        }

        private SimpleCommand goCommand;
        public SimpleCommand GoCommand
        {
            get
            {
                return goCommand ??
                  (goCommand = new SimpleCommand(obj =>
                  {
                      double realPrice = generatePrice();
                      addOrder(realPrice);
                      updateAllDate();
                  },
                (obj) => readyToGo == false));
            }
        }
        private SimpleCommand calcPriceCommand;
        public SimpleCommand CalcPriceCommand
        {
            get
            {
                return calcPriceCommand ??
                  (calcPriceCommand = new SimpleCommand(obj =>
                  {            
                      try
                      {
                          if (UserRide.StartPoint.Length > 0 && UserRide.EndPoint.Length > 0)
                          {
                              calcExpCoast();
                          }
                          else
                          {
                              throw new NullFields();
                          }
                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show(ex.Message);
                      }
                  },
                (obj) => SelectedCarItem != null && 
                UserRide.StartPoint != null && 
                UserRide.EndPoint !=null &&
                readyToGo == true));
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
                      updateAllDate();
                  }));
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
                      "Select a machine from the drop down menu. \n" +
                      "Enter the start and end station. \n" +
                      "Enter your ecxpected time of arrival. \n" +
                      "Click calculate price. After that, the selection items will become inactive and press the \"Go\" button. \n" +
                      "To reset, click the \"Update\" button.\n" +
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

        public ObservableCollection<CarItem> Cars
        {
            get
            {
                return cars;
            }
            set
            {
                cars = value;
                OnPropertyChanged("Cars");
            }
        }

        private void addOrder(double realPrice)
        {
            recordRepository = new RecordRepository(NAME_OF_BASE);

            Record r = new Record(id, SelectedCarItem.ID, 
                UserRide.StartPoint, UserRide.EndPoint, realPrice);
            
            try
            {              
                recordRepository.setRecord(r);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void updateAllDate()
        {
            cleanOrder();
            calculateMoney(id);
            SelectedCarItem = null;
            ReadyToGo = true;
        }
        private double generatePrice()
        {
            double price = UserRide.ExpPrice;

            Random chance = new Random();
            int result = chance.Next(0, 100);

            if (result > 90)
            {
                price += price * 0.2;
                MessageBox.Show("Because of bad weather conditions, \nthe price of the trip has been increased by 20%\n" +
                    $"Total trip price: {Math.Round(price)} ");
                return Math.Round(price);
            }
            else if(result > 70)
            {
                price += price * 0.1;
                MessageBox.Show("Because of traffic jams, the price of the trip is increased by 10%\n" +
                    $"Total trip price: {Math.Round(price)} ");
                return Math.Round(price);
            }
            MessageBox.Show($"Total trip price: {Math.Round(price)}");
            return Math.Round(price);
        }
        private void cleanOrder()
        {
            UserRide.cleanField();
        }
        private void calcExpCoast()
        {
            DateTime current = DateTime.Now;
            try
            {
                TimeSpan interval = UserRide.ExpTime - current;
                if (interval.Minutes > 9)
                {
                    if (UserDisc.Discount > 0)
                    {
                        UserRide.ExpPrice = (interval.Minutes * COAST_OF_MIN) - ((interval.Minutes * COAST_OF_MIN) * (UserDisc.Discount/100));
                    }
                    else
                    {
                        UserRide.ExpPrice = interval.Minutes * COAST_OF_MIN;
                    }                  
                    ReadyToGo = false;
                }
                else
                {
                    throw new InvalidTime();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private double getUsersSumOrders(int user_id)
        {
            double sum = 0;
            try
            {
                recordRepository = new RecordRepository(NAME_OF_BASE);
                sum = recordRepository.getSumOrders(user_id);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return sum;
        }
        private void calculateMoney(int ID)
        {
            userDisc.Sum = getUsersSumOrders(ID);
            userDisc.calcDisc();
        }
        private ObservableCollection<CarItem> addCarItems()
        {
            carsRepository = new CarsRepository(NAME_OF_BASE);
            ObservableCollection<CarItem> carsItem = new ObservableCollection<CarItem>();

            try
            {
                List<CarRecord> cr = carsRepository.getEnabledCars();

                foreach (var item in cr)
                {
                    CarItem current = new CarItem(item.id, item.Car, item.Driver, item.IsEnable);

                    carsItem.Add(current);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return carsItem;
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
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
