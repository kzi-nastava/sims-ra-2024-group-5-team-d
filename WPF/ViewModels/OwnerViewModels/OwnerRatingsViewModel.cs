using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
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
        public ObservableCollection<OwnerRatingViewModel> OwnerRatings { get; set; }
        
        private AccommodationRatingService accommodationRatingService;
        private AccommodationService accommodationService;
        public OwnerRatingViewModel SelectedOwnerRating { get; set; }
        private UserService userService;

        public OwnerRatingsViewModel(User user) {

            accommodationService = new AccommodationService();
            userService = new UserService();
            ShowDetailsCommand = new RelayParameterCommand(ShowDetails);
            accommodationRatingService = new AccommodationRatingService();
            OwnerRatings = new ObservableCollection<OwnerRatingViewModel>();
            accommodationRatingService.GetAllRatingsForOwner(user).ForEach(rating => 
                OwnerRatings.Add(new OwnerRatingViewModel(userService.GetById(rating.GuestId), rating, accommodationService.GetById(rating.AccommodationId))));
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
