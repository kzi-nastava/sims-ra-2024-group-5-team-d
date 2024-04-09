using BookingApp.Appl.UseCases;
using BookingApp.WPF.Commands;
using BookingApp.WPF.Views;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
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
        public ICommand LogInCommand { get; set; }
        public string Username { get; set; }
        public string Password { private get; set; }

        private SignInService signInService;
        public LogInViewModel() {
            signInService = new SignInService();
            LogInCommand = new RelayParameterCommand(LogIn);
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

    }
}
