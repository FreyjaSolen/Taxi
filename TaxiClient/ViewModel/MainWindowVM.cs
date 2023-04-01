using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using TaxiClient.Command;
using TaxiClient.Exceptions;
using TaxiClient.View;
using TaxiRepository.Repository;

namespace TaxiClient.ViewModel
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        private string login;
        private string password;

        private UserRepository userRepository;

        public MainWindowVM()
        {
            userRepository = new UserRepository("taxi_local");
        }

        public string Login
        {
            get { return login; }
            set
            {
                login = value;
                NotifyPropertyChanged("Login");
            }
        }
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                NotifyPropertyChanged("Password");
            }
        }

        private SimpleCommand loginCommand;
        public SimpleCommand LoginCommand
        {
            get
            {
                return loginCommand ??
                  (loginCommand = new SimpleCommand(obj =>
                  {
                      try
                      {
                          if (Login != null && Password != null)
                          {
                              userCheck();
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
        private SimpleCommand helpCommand;
        public SimpleCommand HelpCommand
        {
            get
            {
                return helpCommand ??
                  (helpCommand = new SimpleCommand(obj =>
                  {
                      string message = "Welcome to the program Taxi service\n" +
                      "To get started, enter your username and password. ";
                      MessageBox.Show(message);
                  }));
            }
        }

        private void userCheck()
        {
            try
            {
                string newPassword = GetHash(Password);
                int id = userRepository.getID(Login, newPassword);

                if (id != -1)
                {
                    if (isManager(id))
                    {
                        makeManagerWindow();
                    }
                    else
                    {
                        makeUserWindow(id);
                    }                  
                }
                else
                {
                    throw new InvalideUser();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool isManager(int id)
        {
            return userRepository.IsManager(id);
        }
        private void makeManagerWindow()
        {
            ManagerWindow w = new ManagerWindow();
            w.Show();
            Application.Current.Windows[0].Close();
        }
        private void makeUserWindow(int ID)
        {
            ClientWindow w = new ClientWindow(ID);
            w.Show();
            Application.Current.Windows[0].Close();
        }
        private void closeProgram()
        {
            System.Windows.Application.Current.Shutdown();
        }
        private static string GetHash(string input)
        {
            //SHA512
            var bytes = System.Text.Encoding.UTF8.GetBytes(input);
            using (var hash = System.Security.Cryptography.SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);

                var hashedInputStringBuilder = new System.Text.StringBuilder(128);
                foreach (var b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("X2"));
                return hashedInputStringBuilder.ToString();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
