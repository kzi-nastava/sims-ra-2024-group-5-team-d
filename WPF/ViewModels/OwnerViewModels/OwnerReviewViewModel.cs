using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.WPF.Commands;
using BookingApp.WPF.Views.OwnerView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    public class OwnerReviewViewModel
    {
        public ICommand RateGuestCommand { get; set; }
        public ICommand OwnerRatesCommand { get; set; }
        public static ObservableCollection<UnratedGuestViewModel> UnratedGuests { get; set; }
        private UnratedGuestService unratedGuestService;
        private User LoggedInUser;
        private UserService userService;
        public OwnerReviewViewModel(User user) 
        {
            userService = new UserService();
            RateGuestCommand = new RelayCommand(RateGuest);
            OwnerRatesCommand = new RelayCommand(OwnerRates);
            LoggedInUser = user;
            unratedGuestService = new UnratedGuestService();
            UnratedGuests = new ObservableCollection<UnratedGuestViewModel>();
            unratedGuestService.GetUnratedGuests(LoggedInUser)
                                .ForEach(unratedGuest => UnratedGuests.Add(new UnratedGuestViewModel(userService.GetById(unratedGuest.UserId).FullName, userService.GetById(unratedGuest.UserId).AvatarPath)));
        }
        private void RateGuest()
        {
            UnratedGuestsUserControl unratedGuestsWindow = new UnratedGuestsUserControl(LoggedInUser);
            OwnerMainWindow.contentControl.Content = unratedGuestsWindow;
        }
        private void OwnerRates()
        {
            OwnerRatingsUserControl ownerRatingsUserControl = new OwnerRatingsUserControl(LoggedInUser);
            OwnerMainWindow.contentControl.Content = ownerRatingsUserControl;
        }
    }
}
