using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
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
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for RateGuestsWindow.xaml
    /// </summary>
    public partial class RateGuestsWindow : Window
    {

        private List<GuestRating> GuestRatings;
        private List<Reservation> Reservations;
        private AccommodationRepository accommodationRepository;
        public GuestRatingDTO SelectedGuestRatingDTO { get; set; }

        public static ObservableCollection<GuestRatingDTO> GuestRatingsObservable { get; set; }
        public RateGuestsWindow(List<GuestRating>guestRatings,List<Reservation>reservations)
        {
            accommodationRepository = new AccommodationRepository();
            this.GuestRatings = guestRatings;
            this.Reservations = reservations;
            GuestRatingsObservable = new ObservableCollection<GuestRatingDTO>();
            InitializeComponent();
            DataContext = this;
            Update();
        }
        private void Update()
        {
            foreach (Reservation reservation in Reservations)
            {
                if (!IsGuestFromReservationRated(reservation))
                {
                    if (IsGuestRateable(reservation) && !IsCanceled(reservation))
                        GuestRatingsObservable.Add(new GuestRatingDTO(accommodationRepository.GetAccommodationNameById(reservation.AccommodationId), reservation.ReservedFrom, reservation.ReservedTo, reservation.UserId, reservation.Id, reservation.AccommodationId));
                }
            }
        }

        private static bool IsCanceled(Reservation reservation)
        {
            return reservation.Cancelled == 1;
        }

        private static bool IsGuestRateable(Reservation reservation)
        {
            
            return (DateTime.Now - reservation.ReservedTo).TotalDays <= 5 && (DateTime.Now>reservation.ReservedTo);
        }

        private bool IsGuestFromReservationRated(Reservation reservation)
        {
            return GuestRatings.Any(guestRating => guestRating.ReservationId == reservation.Id);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void RateGuestButton_Click(object sender, RoutedEventArgs e)
        {
            GiveRateWindow giveRateWindow = new GiveRateWindow(SelectedGuestRatingDTO);
            giveRateWindow.Owner = this;
            giveRateWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            giveRateWindow.ShowDialog();

        }
    }
}
