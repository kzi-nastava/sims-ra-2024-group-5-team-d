using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Repositories;
using BookingApp.WPF.Commands;
using BookingApp.WPF.ViewModels.TourViewModels.TourGuideViewModels;
using BookingApp.WPF.Views.OwnerView;
using BookingApp.WPF.Views.TouristGuide;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace BookingApp.WPF.ViewModels
{
    public class FinishedTourViewModel
    {
        public TourViewModel SelectedTour { get; set; }
        public SeriesCollection AgeStatistics { get; set; }
        private User LoggedInUser { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand ClickCommand { get; set; }
        public ObservableCollection<TourGuestRatingViewModel> TourGuestRating { get; set;}
        public TourGuestRatingViewModel SelectedRating { get; set; }
        public TourRatingService tourRatingService { get; set; }
        
        public TourReservationService tourReservationService { get; set; }
        public TourGuestService tourGuestService { get; set; }
        public TourRealisationService tourRealisationService { get; set; }
        public UserService userService { get; set; }
        public CheckPointService checkPointService { get; set; }
        public int Under18 { get; set; }
        public int Between18And50 { get; set; }
        public int Over50 { get; set; }
        public int Sum { get; set; }

        public FinishedTourViewModel(TourViewModel tour, User user) 
        {
            LoggedInUser = user;
            SelectedTour = tour;
            BackCommand = new RelayCommand(BackButton);
            ClickCommand = new RelayCommand(Click);
            tourGuestService = new TourGuestService();
            CalculateAgeStatistics();
            InitializeAgeStatistics();
            tourRatingService = new TourRatingService();
            tourReservationService = new TourReservationService();
            tourRealisationService = new TourRealisationService();
            checkPointService = new CheckPointService();
            userService = new UserService();
            TourGuestRating = GetReviews();
            
        }

        private void BackButton()
        {
            SideBar.contentControlW.Content = new FinishedToursWindow(LoggedInUser);
        }

        private void Click()
        {
            if (SelectedRating != null)
            {
                TourRating rating = tourRatingService.GetTourRatingById(SelectedRating.Id);
                rating.IsValid = false;
                tourRatingService.UpdateTourRating(rating);
                {
                    for (int i = 0; i < TourGuestRating.Count; i++)
                    {
                        if (TourGuestRating[i].Id == SelectedRating.Id)
                        {
                            TourGuestRating[i].IsNotValid = true;
                        }
                    }
                }
            }
        }

        private void CalculateAgeStatistics()
        {
            Under18 = 0;
            Between18And50 = 0;
            Over50 = 0;

            var tourGuests = tourGuestService.GetTourGuestsOnTour(SelectedTour.Id);

            foreach (var guest in tourGuests)
            {
                if (guest.Years < 18)
                {
                    Under18++;
                }
                else if (guest.Years <= 50)
                {
                    Between18And50++;
                }
                else
                {
                    Over50++;
                }
            }
            Sum = Under18 + Between18And50 + Over50;
        }
        private void InitializeAgeStatistics()
        {
            AgeStatistics = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "<18",
                    Values = new ChartValues<ObservableValue> {new ObservableValue (Under18)},
                    DataLabels = true,
                    Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6A9A"))
                },
                new PieSeries
                {
                    Title = "18-50",
                    Values = new ChartValues<ObservableValue> {new ObservableValue (Between18And50) },
                    DataLabels = true,
                    Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A375E1"))
                },
                new PieSeries
                {
                    Title = "50>",
                    Values = new ChartValues<ObservableValue> {new ObservableValue (Over50) },
                    DataLabels = true,
                    Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD900"))
                },
            };
        }
        public ObservableCollection<TourGuestRatingViewModel> GetReviews()
        {
            List<TourRating> tourRatings = tourRatingService.GetAllTourRatings();
            ObservableCollection<TourGuestRatingViewModel> ratings = new ObservableCollection<TourGuestRatingViewModel>();
            foreach (var rating in tourRatings)
            {
                TourReservation reservation = tourReservationService.GetById(rating.TourReservationId);
                TourRealisation realisation = tourRealisationService.GetTourRealisationById(reservation.TourRealisationId);
                User user = userService.GetById(reservation.User.Id);
                TourGuest guest = tourGuestService.GetTourGuestByPersonalId(user.PersonalId,reservation.Id);
                if (realisation.Id != -1 && realisation.TourId == SelectedTour.Id && guest.TourReservationId == reservation.Id)
                {
                    TourGuestRatingViewModel tourGuestRating = new TourGuestRatingViewModel(rating.Id, guest.FullName, guest.Years, checkPointService.GetById(guest.CheckPointId).Name, realisation.StartTime, rating.TouristLanguage, rating.TouristKnowladge, rating.TourAmusement, rating.Comment,rating.IsValid);
                    ratings.Add(tourGuestRating);
                }
            }
            return ratings;
        }

        


    }

}
