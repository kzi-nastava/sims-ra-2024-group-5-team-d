using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.Commands;
using BookingApp.WPF.Views.OwnerView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    public class OwnerRatingsViewModel
    {
        public ICommand ShowDetailsCommand { get; set; }


        private AccommodationRatingService accommodationRatingService;
        private AccommodationService accommodationService;
        private UserService userService;

        public ObservableCollection<OwnerRatingViewModel> OwnerRatings { get; set; }
        public OwnerRatingViewModel SelectedOwnerRating { get; set; }

        public OwnerRatingsViewModel(User user) {
            InitializeServices();
            ShowDetailsCommand = new RelayParameterCommand(ShowDetails);
            OwnerRatings = new ObservableCollection<OwnerRatingViewModel>();
            accommodationRatingService.GetAllRatingsForOwner(user).ForEach(rating => 
                OwnerRatings.Add(new OwnerRatingViewModel(userService.GetById(rating.GuestId), rating, accommodationService.GetById(rating.AccommodationId))));
        }

        private void InitializeServices()
        {
            userService = new UserService(Injector.CreateInstance<IUserRepository>());
            accommodationService = new AccommodationService(Injector.CreateInstance<IAccommodationRepository>(),userService);
            GuestRatingService guestRatingService = new GuestRatingService(Injector.CreateInstance<IGuestRatingRepository>(), accommodationService);
            accommodationRatingService = new AccommodationRatingService(Injector.CreateInstance<IAccommodationRatingRepository>(),accommodationService,guestRatingService);
        }

        public void ShowDetails(object parameter)
        {
            if (parameter != null)
            {
                SelectedOwnerRating = (OwnerRatingViewModel)parameter;
                ShowDetailedReviewWindow details = new ShowDetailedReviewWindow(SelectedOwnerRating.Id);
                details.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                details.ShowDialog();
            }
        }
    }
}
