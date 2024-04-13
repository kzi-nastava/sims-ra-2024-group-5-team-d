using BookingApp.Domain.Models;
using BookingApp.WPF.Commands;
using BookingApp.WPF.Views.TouristView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels
{
    public class LiveTourViewModel
    {
        public User User;
        public ICommand PastToursTabCommand { get; private set; }
        public LiveTourViewModel(User user) 
        {
            User = user;
            PastToursTabCommand = new RelayCommand(SwitchToPastTours);
        }

        public void SwitchToPastTours()
        {
            TouristHomeWindow.contentControl.Content = new PastToursUserControl(User);
        }
    }
}
