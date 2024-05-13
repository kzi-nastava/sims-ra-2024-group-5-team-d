using BookingApp.WPF.Commands;
using BookingApp.WPF.Views;
using BookingApp.WPF.Views.GuestWindows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels.GuestViewModels
{

    public class OwnerRateGuestViewModel
    {
        public ICommand CloseCommand { get; set; }
        public OwnerRatingGuestViewModel OwnerRatingGuestViewModel{ get; set; }
        public OwnerRateGuestWindow OwnerRateGuestWindow { get; set; }
        public OwnerRateGuestViewModel(OwnerRatingGuestViewModel ownerRatingGuestViewModel, OwnerRateGuestWindow ownerRateGuestWindow)
        {
            OwnerRateGuestWindow = ownerRateGuestWindow;
            OwnerRatingGuestViewModel = ownerRatingGuestViewModel;
            CloseCommand = new RelayCommand(Close);
            OwnerRateGuestWindow = ownerRateGuestWindow;
        }

        private void Close()
        {
            if (OwnerRateGuestWindow != null)
            {
                OwnerRateGuestWindow.Close();
            }
        }
    }
}
