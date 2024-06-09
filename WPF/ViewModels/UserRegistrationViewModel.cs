using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels
{
    public class UserRegistrationViewModel
    {
        public ICommand RegisterCommand { get; set; }
        public ICommand TypeCommand { get; set; }
        public string FullName { get; set; }
        public UserType Type { get; set; }
        public string BirhtDate { get; set; }
        public string PersonalId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        private UserService userService;
        private SignInService signInService;
        private string avatarPath;
        private string macAddress;
        public UserRegistrationViewModel(string avatarPath)
        {
            InitializeServices();
            this.avatarPath = avatarPath;
            TypeCommand = new RelayParameterCommand(TypeClick);
            RegisterCommand = new RelayParameterCommand(RegisterUser);
            macAddress = GetMacAddress();
            
        }

        private void InitializeServices()
        {
            userService = new UserService(Injector.CreateInstance<IUserRepository>());
            signInService = new SignInService();
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
        private void TypeClick(object parameter)
        {
            Type = (UserType)Enum.Parse(typeof(UserType), parameter.ToString());
            Debug.WriteLine(Type);
        }
        public void RegisterUser(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            Password = passwordBox.Password;
            User user;
                if (Type == Domain.Models.UserType.Tourist)
                    user=userService.Save(new User(Username, Password, Type, FullName, PersonalId, DateOnly.Parse(BirhtDate), avatarPath, macAddress, null,true));
                else
                    user=userService.Save(new User(Username, Password, Type, FullName, PersonalId, DateOnly.Parse(BirhtDate), avatarPath, macAddress, false, true));
            signInService.MacLogin(user,true);
        }

    }
}
