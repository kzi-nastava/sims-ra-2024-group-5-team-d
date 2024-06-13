using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.Commands;
using BookingApp.WPF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection.Metadata;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels
{
    public class LogInViewModel
    {
        public ICommand FastLogInCommand { get; set; }
        public ICommand LogInCommand { get; set; }
        public string Username { get; set; }
        public string Password { private get; set; }

        private SignInService signInService;
        public ObservableCollection<UserViewModel> Users { get; set; }
        private UserService userService;
        private string variable="";
        public LogInViewModel() {
            InitializeServices();
            signInService = new SignInService();
            LogInCommand = new RelayParameterCommand(LogIn);
            FastLogInCommand = new RelayParameterCommand(FastLogIn);
            Users = new ObservableCollection<UserViewModel>();
            Debug.WriteLine(GetMacAddress());
            userService.GetUsersWithSameMacAdress(GetMacAddress()).ForEach(user=>Users.Add(new UserViewModel(user)));
           // Environment.GetEnvironmentVariable(variable);
            Debug.WriteLine(variable);
        }

        private void InitializeServices()
        {
            userService = new UserService(Injector.CreateInstance<IUserRepository>());
        }

        private string GetMacAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            string macAddress = string.Empty;

            foreach (NetworkInterface nic in nics)
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    macAddress = nic.GetPhysicalAddress().ToString();
                    break;
                }
            }
            return macAddress;
        }


        public event EventHandler? CanExecuteChanged;

        private void LogIn(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            Password = passwordBox.Password;
            if(signInService.CkeckCredentials(Username, Password)=="Success")
            {

            }
        }
        private void FastLogIn(object parameter)
        {
            UserViewModel user = parameter as UserViewModel;
            if (user!=null)
            {
                signInService.MacLogin(userService.GetById(user.Id));
            }
        }

    }
}
