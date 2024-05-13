using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
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

        private AccommodationService accommodationService;
        private AccommodationReservationService accommodationReservationService;
        private AccommodationRatingService accommodationRatingService;
        private UserService userService;
        NotifierService notifierService;

        private User loggedInUser;
        public ProfileViewModel ProfileViewModel { get; set; }
        public ProfileAccommodationViewModel MostPopularAccommodation { get; set; }
        public ProfileAccommodationViewModel BestRatedAccommodation { get; set; }
        public ProfileAccommodationViewModel MostBusyAccommodation { get; set; }
        public ProfileCredentialsViewModel ProfileCredentials { get; set; }
        public OwnerProfileViewModel(User user)
        {
            InitializeServices();
            loggedInUser = user;
            ProfileViewModel = new ProfileViewModel(user,accommodationRatingService.GetAverageRatingForOwner(user),accommodationReservationService.GetNumberOfReservationsForOwner(user));
            MostPopularAccommodation = new ProfileAccommodationViewModel(accommodationReservationService.GetMostPopularAccommodationForOwner(user));
            BestRatedAccommodation = new ProfileAccommodationViewModel(accommodationService.GetBestRatedAccommodationForOwner(user));
            MostBusyAccommodation = new ProfileAccommodationViewModel(accommodationReservationService.GetMostBusyAccommodationForOwner(user));
            ProfileCredentials = new ProfileCredentialsViewModel(user);

            ChangeCredentialsCommand = new RelayCommand(ChangeCredentials);
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void InitializeServices()
        {
            userService = new UserService(Injector.CreateInstance<IUserRepository>());
            accommodationService = new AccommodationService(Injector.CreateInstance<IAccommodationRepository>(),userService);
            accommodationRatingService = new AccommodationRatingService(Injector.CreateInstance<IAccommodationRatingRepository>(),accommodationService);
            accommodationReservationService = new AccommodationReservationService(Injector.CreateInstance<IAccommodationReservationRepository>(),accommodationService);
            notifierService = new NotifierService();
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
