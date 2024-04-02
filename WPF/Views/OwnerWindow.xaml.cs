using BookingApp.Domain.Models;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace BookingApp.WPF.Views
{
    /// <summary>
    /// Interaction logic for OwnerWindow.xaml
    /// </summary>
    public partial class OwnerWindow : Window
    {
        public static ObservableCollection<Accommodation> Accommodations { get; set; }
        public Accommodation selectedAccommodation { get; set; }
        public User LoggedInUser { get; set; }
        private List<GuestRating> GuestRatingsByLoggedInUser;
        private readonly AccommodationRepository _repository;
        private readonly AccommodationReservationRepository _reservationRepository;
        private readonly GuestRatingRepository _guestRatingRepository;
        private List<AccommodationReservation> ReservationsForLoggedInUserAccommodations;


        public OwnerWindow(User user)
        {
            InitializeComponent();
            LoggedInUser = user;
            DataContext = this;
            _reservationRepository = new AccommodationReservationRepository();
            _guestRatingRepository = new GuestRatingRepository();
            _repository = new AccommodationRepository();
            Accommodations = new ObservableCollection<Accommodation>(_repository.GetByUser(user));
            CheckForUnratedGuestsByLoggedInUser();
        }

        private List<GuestRating> GetAllGuestRatingsByLoggedInUser()
        {
            return _guestRatingRepository.GetAll().Where(guestRating => IsRatedByLoggedInUser(guestRating)).ToList();
        }

        private  bool IsRatedByLoggedInUser(GuestRating guestRating)
        {
            return Accommodations.Any(accommodation => accommodation.Id == guestRating.AccommodationId);
        }

        private void CheckForUnratedGuestsByLoggedInUser() {
            GuestRatingsByLoggedInUser = GetAllGuestRatingsByLoggedInUser();
            FindReservationsForLoggedInUserAccommodations();
            if (AreThereUnratedGuests())
            {
                RateLabel.Visibility = Visibility.Visible;
                RateButton.Visibility = Visibility.Visible;
            }
            else
            {
                RateLabel.Visibility = Visibility.Hidden;
                RateButton.Visibility = Visibility.Hidden;
            }
        }
        private bool AreThereUnratedGuests()
        {
            List<AccommodationReservation> potentialUnratedGuests = ReservationsForLoggedInUserAccommodations.Where(reservation => !IsReservationCanceled(reservation) && IsReservationRateable(reservation)).ToList();
            return !potentialUnratedGuests.All(p => IsGuestRated(p));
        }

        private bool IsGuestRated(AccommodationReservation p)
        {
            return GuestRatingsByLoggedInUser.Any(q => q.ReservationId == p.Id);
        }

        private bool IsReservationRateable(AccommodationReservation reservation)
        {
            return (DateTime.Now - reservation.ReservedTo).Days >= 0 && (DateTime.Now - reservation.ReservedTo).Days <= 5;
        }

        private bool IsReservationCanceled(AccommodationReservation reservation)
        {
            return reservation.Cancelled == 1;
        }

        private void FindReservationsForLoggedInUserAccommodations()
        {
            ReservationsForLoggedInUserAccommodations = _reservationRepository.GetAll()
            .Where(reservation => IsReservationForLoggedInUserAccommodation(reservation)).ToList();
        }

        private bool IsReservationForLoggedInUserAccommodation(AccommodationReservation reservation)
        {
            return Accommodations.Any(accommodation => accommodation.Id == reservation.AccommodationId);
        }

        private void RegisterPropertyButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterAccommodationWindow registerAccommodationWindow = new RegisterAccommodationWindow(LoggedInUser);
            registerAccommodationWindow.ShowDialog();
        }

        private void ShowStatsButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedAccommodation!=null)
            {
                StatsForAccommodationWindow statsForAccommodationWindow = new StatsForAccommodationWindow(LoggedInUser, selectedAccommodation);
                statsForAccommodationWindow.Owner = this;
                statsForAccommodationWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                statsForAccommodationWindow.ShowDialog();
            }
        }

        private void RateGuestsButton_Click(object sender, RoutedEventArgs e)
        {
            RateGuestsWindow rateGuestsWindow = new RateGuestsWindow(GuestRatingsByLoggedInUser, ReservationsForLoggedInUserAccommodations);
            rateGuestsWindow.Owner = this;
            rateGuestsWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            rateGuestsWindow.ShowDialog();
            CheckForUnratedGuestsByLoggedInUser();
        }
    }
}
