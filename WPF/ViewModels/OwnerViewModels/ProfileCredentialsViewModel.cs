using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    class ProfileCredentialsViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string username;
        private string password;
        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Username"));
            }
        }
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Password"));
            }
        }
        private bool isEditing=false;
        public bool IsEditing
        {
            get { return isEditing; }
            set
            {
                isEditing = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsEditing"));
            }
        }
        private bool isVisible=true;
        public bool IsVisible
        {
            get { return isVisible; }
            set
            {
                isVisible = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsVisible"));
            }
        }
        public ProfileCredentialsViewModel(User user)
        {
            Username = user.Username;
            Password = user.Password;
        }
    }
}
