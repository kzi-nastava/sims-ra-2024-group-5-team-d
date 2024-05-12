using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.WPF.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    class OwnerProfileViewModel
    {
        public ICommand ChangeCredentialsCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        private User loggedInUser;
        public ProfileViewModel ProfileViewModel { get; set; }
        private AccommodationRatingService accommodationRatingService; 
        private AccommodationReservationService accommodationReservationService;
        private UserService userService;
        NotifierService notifierService;
        public ProfileAccommodationViewModel MostPopularAccommodation { get; set; }
        public ProfileAccommodationViewModel BestRatedAccommodation { get; set; }
        public ProfileAccommodationViewModel MostBusyAccommodation { get; set; }
        public ProfileCredentialsViewModel ProfileCredentials { get; set; }
        public OwnerProfileViewModel(User user)
        {
            loggedInUser = user;
            accommodationRatingService = new AccommodationRatingService();
            accommodationReservationService = new AccommodationReservationService();
            userService = new UserService();
            notifierService = new NotifierService();
            ProfileViewModel = new ProfileViewModel(user,accommodationRatingService.GetAverageRatingForOwner(user),accommodationReservationService.GetNumberOfReservationsForOwner(user));
            MostPopularAccommodation = new ProfileAccommodationViewModel(accommodationReservationService.GetMostPopularAccommodationForOwner(user));
            BestRatedAccommodation = new ProfileAccommodationViewModel(accommodationRatingService.GetBestRatedAccommodationForOwner(user));
            MostBusyAccommodation = new ProfileAccommodationViewModel(accommodationReservationService.GetMostBusyAccommodationForOwner(user));
            ProfileCredentials = new ProfileCredentialsViewModel(user);

            ChangeCredentialsCommand = new RelayCommand(ChangeCredentials);
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }
        private void ChangeCredentials()
        {
            ProfileCredentials.IsEditing = true;
            ProfileCredentials.IsVisible = false;
        }
        private void Save()
        {
            ProfileCredentials.IsEditing = false;
            ProfileCredentials.IsVisible = true;
            userService.UpdateCredentials(loggedInUser, ProfileCredentials.Username, ProfileCredentials.Password);
            notifierService.ShowSuccess("Credentials updated successfully!");
        }
        private void Cancel()
        {
            ProfileCredentials.Username = loggedInUser.Username;
            ProfileCredentials.Password = loggedInUser.Password;
            ProfileCredentials.IsEditing = false;
            ProfileCredentials.IsVisible = true;
        }
    }
}
