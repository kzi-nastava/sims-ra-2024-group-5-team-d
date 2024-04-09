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
        private IAccommodationReservationRepository accommodationReservationRepository;
        public YesNoCancelledReservationWindow(UserReservationsViewModel reservation)
        {
            InitializeComponent();
            userReservationsViewModel = reservation;
            DataContext = this;
            accommodationReservationRepository = Injector.CreateInstance<IAccommodationReservationRepository>();
            accommodationReservation = accommodationReservationRepository.GetById(reservation.Id);
        }

        private void YesCancelReservationButton(object sender, RoutedEventArgs e)
        {
            
            accommodationReservation.Cancelled = 1;
            ReservationsAccommodationUserControl.ActiveReservations.Remove(userReservationsViewModel);
            accommodationReservationRepository.Update(accommodationReservation);
            Close();

        }

        private void NoCancelReservationButton(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
