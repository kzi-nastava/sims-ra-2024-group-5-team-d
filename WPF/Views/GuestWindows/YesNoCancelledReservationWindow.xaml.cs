using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.ViewModels;
using System;
using System.Collections.Generic;
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

namespace BookingApp.WPF.Views.GuestWindows
{
    /// <summary>
    /// Interaction logic for YesNoCancelledReservationWindow.xaml
    /// </summary>
    public partial class YesNoCancelledReservationWindow : Window
    {
        private AccommodationReservation accommodationReservation;
        private UserReservationsViewModel userReservationsViewModel;
        private AccommodationReservationService reservationService;
        public NotificationsService notificationsService;
        private AccommodationService accommodationService;
        NotifierService notifierService;

        public YesNoCancelledReservationWindow(UserReservationsViewModel reservation)
        {
            InitializeComponent();
            userReservationsViewModel = reservation;
            DataContext = this;
            reservationService = new AccommodationReservationService();
            accommodationReservation = reservationService.GetById(reservation.Id);
            notificationsService = new NotificationsService();
            accommodationService = new AccommodationService();
            notifierService = new NotifierService();
        }

        private void YesCancelReservationButton(object sender, RoutedEventArgs e)
        {
            
            accommodationReservation.Cancelled = 1;
            ReservationsAccommodationUserControl.ActiveReservations.Remove(userReservationsViewModel);
            reservationService.Update(accommodationReservation);
            notificationsService.CreateNotification(accommodationService.GetById(reservationService.GetById(userReservationsViewModel.Id).AccommodationId).Owner.Id, userReservationsViewModel.Id,Domain.Models.Type.CANCEL);
            Close();
            notifierService.ShowSuccess("Reservation cancelled successfully!");
        }

        private void NoCancelReservationButton(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
