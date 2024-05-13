using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
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

        private UserService userService;
        private AccommodationService accommodationService;
        private UnratedGuestService unratedGuestService;

        public static ObservableCollection<UnratedGuestViewModel> UnratedGuests { get; set; }
        public UnratedGuestViewModel SelectedGuest { get; set; }
        private User loggedInUser;
        public UnratedGuestsViewModel(User user)
        {
            InitializeServices();
            RateGuestCommand = new RelayParameterCommand(RateGuest);
            loggedInUser = user;
            UnratedGuests = new ObservableCollection<UnratedGuestViewModel>();
            unratedGuestService.GetUnratedGuests(loggedInUser)
                                .ForEach(unratedReservation => UnratedGuests.Add
                                (new UnratedGuestViewModel(unratedReservation.Id, userService.GetById(unratedReservation.UserId).FullName, unratedReservation.ReservedFrom, unratedReservation.ReservedTo, accommodationService.GetById(unratedReservation.AccommodationId).Location, accommodationService.GetAccommodationNameById(unratedReservation.AccommodationId),userService.GetById(unratedReservation.UserId).AvatarPath))
                                );
        }

        private void InitializeServices()
        {
            userService = new UserService(Injector.CreateInstance<IUserRepository>());
            accommodationService = new AccommodationService(Injector.CreateInstance<IAccommodationRepository>(),userService);
            unratedGuestService = new UnratedGuestService(accommodationService);
        }

        private void RateGuest(object parameter)
        {
            if (parameter != null)
            {
                SelectedGuest = parameter as UnratedGuestViewModel;
                RateGuestFormWindow rateFormWindow = new RateGuestFormWindow(SelectedGuest);
                rateFormWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                rateFormWindow.ShowDialog();
            }
        }
    }
}
