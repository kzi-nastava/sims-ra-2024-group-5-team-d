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
        private string avatarPath;
        private string macAddress;
        public UserRegistrationViewModel(string avatarPath)
        {
            this.avatarPath = avatarPath;
            userService =new UserService();
            TypeCommand = new RelayParameterCommand(TypeClick);
            RegisterCommand = new RelayParameterCommand(RegisterUser);
            macAddress = GetMacAddress();
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
                if (Type == Domain.Models.UserType.Tourist)
                    userService.Save(new User(Username, Password, Type, FullName, PersonalId, DateOnly.Parse(BirhtDate), avatarPath, macAddress, null));
                else
                    userService.Save(new User(Username, Password, Type, FullName, PersonalId, DateOnly.Parse(BirhtDate), avatarPath, macAddress, false));
        }

    }
}
