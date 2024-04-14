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
using System.Windows;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    public class UnratedGuestsViewModel
    {
        public ICommand RateGuestCommand { get; set; }
        public static ObservableCollection<UnratedGuestViewModel> UnratedGuests { get; set; }
        public UnratedGuestViewModel SelectedGuest { get; set; }
        private UserService userService;
        private AccommodationService accommodationService;
        private User loggedInUser;
        private UnratedGuestService unratedGuestService;
        public UnratedGuestsViewModel(User user)
        {
            RateGuestCommand = new RelayCommand(RateGuest);
            accommodationService = new AccommodationService();
            userService = new UserService();
            unratedGuestService = new UnratedGuestService();
            loggedInUser = user;
            UnratedGuests = new ObservableCollection<UnratedGuestViewModel>();
            unratedGuestService.GetUnratedGuests(loggedInUser)
                                .ForEach(unratedGuest => UnratedGuests.Add
                                (new UnratedGuestViewModel(unratedGuest.Id, userService.GetById(unratedGuest.UserId).FullName, unratedGuest.ReservedFrom, unratedGuest.ReservedTo, accommodationService.GetById(unratedGuest.AccommodationId).Location, accommodationService.GetAccommodationNameById(unratedGuest.AccommodationId)))
                                );
        }
        private void RateGuest()
        {
            if (SelectedGuest != null)
            {
                RateGuestFormWindow rateFormWindow = new RateGuestFormWindow(SelectedGuest);
               // Window parentWindow = Window.GetWindow(this);
                //rateFormWindow.Owner = parentWindow;
                rateFormWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                rateFormWindow.ShowDialog();
            }
        }
    }
}
