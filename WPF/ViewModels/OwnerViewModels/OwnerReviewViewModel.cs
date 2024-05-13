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
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    public class OwnerReviewViewModel
    {
        public ICommand RateGuestCommand { get; set; }
        public ICommand OwnerRatesCommand { get; set; }

        private UserService userService;
        private AccommodationRatingService accommodationRatingService;
        private UnratedGuestService unratedGuestService;

        public static ObservableCollection<UnratedGuestViewModel> UnratedGuests { get; set; }
        private User LoggedInUser;
        public string Star1 { get; set; }
        public string Star2 { get; set; }
        public string Star3 { get; set; }
        public string Star4 { get; set; }
        public string Star5 { get; set; }


        public int Stars5 { get; set; }
        public int Stars4 { get; set; }
        public int Stars3 { get; set; }       
        public int Stars2 { get; set; }
        public int Stars1 { get; set; }
        public int NumberOfReviews { get; set; }
        public double AverageRating { get; set; }
        private List<string> starPaths;
        public OwnerReviewViewModel(User user) 
        {
            InitializeServices();
            RateGuestCommand = new RelayCommand(RateGuest);
            OwnerRatesCommand = new RelayCommand(OwnerRates);
            LoggedInUser = user;
            starPaths = new List<string>();
            UnratedGuests = new ObservableCollection<UnratedGuestViewModel>();
            unratedGuestService.GetUnratedGuests(LoggedInUser)
                                .ForEach(unratedGuest => UnratedGuests.Add(new UnratedGuestViewModel(userService.GetById(unratedGuest.UserId).FullName, userService.GetById(unratedGuest.UserId).AvatarPath)));
            NumberOfReviews=accommodationRatingService.GetNumberOfRatingsForOwner(user);
            Stars5=accommodationRatingService.NumberOf5StarRatingsForOwner(user);
            Stars4=accommodationRatingService.NumberOf4StarRatingsForOwner(user);
            Stars3=accommodationRatingService.NumberOf3StarRatingsForOwner(user);
            Stars2=accommodationRatingService.NumberOf2StarRatingsForOwner(user);
            Stars1=accommodationRatingService.NumberOf1StarRatingsForOwner(user);
            if (NumberOfReviews != 0)
            {
                Stars5 = Stars5 * 100 / NumberOfReviews;
                Stars4 = Stars4 * 100 / NumberOfReviews;
                Stars3 = Stars3 * 100 / NumberOfReviews;
                Stars2 = Stars2 * 100 / NumberOfReviews;
                Stars1 = Stars1 * 100 / NumberOfReviews;
            }
            AverageRating=accommodationRatingService.GetAverageRatingForOwner(user);
            if(AverageRating is double.NaN)
                AverageRating=0;
            double averageRating = AverageRating;
            while (averageRating >= 1)
            {
                starPaths.Add("../../../Resources/Images/OwnerImages/StarFull.png");
                averageRating--;
            }
            if (averageRating > 0.25)
                starPaths.Add("../../../Resources/Images/OwnerImages/StarHalfFull.png");
            while (starPaths.Count < 5)
                starPaths.Add("../../../Resources/Images/OwnerImages/idemo.png");
            Star1 = starPaths[0];
            Star2 = starPaths[1];
            Star3 = starPaths[2];
            Star4 = starPaths[3];
            Star5 = starPaths[4];
        }

        private void InitializeServices()
        {
            userService = new UserService(Injector.CreateInstance<IUserRepository>());
            AccommodationService accommodationService = new AccommodationService(Injector.CreateInstance<IAccommodationRepository>(), userService);
            unratedGuestService = new UnratedGuestService(accommodationService);
            accommodationRatingService = new AccommodationRatingService(Injector.CreateInstance<IAccommodationRatingRepository>(),accommodationService);
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
