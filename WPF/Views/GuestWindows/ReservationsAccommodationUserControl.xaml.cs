using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using BookingApp.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace BookingApp.WPF.Views.GuestWindows
{
    /// <summary>
    /// Interaction logic for ReservationsAccommodationUserControl.xaml
    /// </summary>
    public partial class ReservationsAccommodationUserControl : UserControl
    {

        public static ObservableCollection<UserReservationsViewModel> ActiveReservations { get; set; }
        public static ObservableCollection<UserReservationsViewModel> FinishedReservations { get; set; }
        public static ObservableCollection<UserReservationsViewModel> CancelledReservations { get; set; }

        public UserReservationsViewModel SelectedReservation { get; set; }
        public User LoggedInUser { get; set; }
        public AccommodationReservationService repositoryService { get; set; }
        public UserReservationsService reservationsService { get; set; }
        public Accommodation accommodation;
        public AccommodationReservation accommodationReservation { get; set; }
        private AccommodationService accommodationService;
        public ReservationsAccommodationUserControl(User user)
        {
            accommodationService = new AccommodationService();
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;
            accommodation = new Accommodation();
            repositoryService = new AccommodationReservationService();
            reservationsService = new UserReservationsService();
            ActiveReservations = new ObservableCollection<UserReservationsViewModel>();
            FinishedReservations = new ObservableCollection<UserReservationsViewModel>();
            CancelledReservations = new ObservableCollection<UserReservationsViewModel>();
 
            reservationsService.GetActiveReservationsForUser(LoggedInUser)
                .ForEach(r => ActiveReservations.Add(new UserReservationsViewModel(r.Id, accommodationService.GetAccommodationNameById(r.AccommodationId) , accommodationService.GetById(r.AccommodationId).Location, accommodationService.GetById(r.AccommodationId).ImagesPath, accommodationService.GetById(r.AccommodationId).Capacity, r.ReservedFrom, r.ReservedTo,r.IsCancellable(accommodationService.GetById(r.AccommodationId).CancellationDeadline),r.IsRateable())));
            reservationsService.GetFinishedReservationsForUser(LoggedInUser)
                .ForEach(r => FinishedReservations.Add(new UserReservationsViewModel(r.Id, accommodationService.GetAccommodationNameById(r.AccommodationId), accommodationService.GetById(r.AccommodationId).Location, accommodationService.GetById(r.AccommodationId).ImagesPath, accommodationService.GetById(r.AccommodationId).Capacity, r.ReservedFrom, r.ReservedTo, r.IsCancellable(accommodationService.GetById(r.AccommodationId).CancellationDeadline), r.IsRateable())));
            reservationsService.GetCancelledReservationsForUser(LoggedInUser)
                .ForEach(r => CancelledReservations.Add(new UserReservationsViewModel(r.Id, accommodationService.GetAccommodationNameById(r.AccommodationId), accommodationService.GetById(r.AccommodationId).Location, accommodationService.GetById(r.AccommodationId).ImagesPath, accommodationService.GetById(r.AccommodationId).Capacity, r.ReservedFrom, r.ReservedTo, r.IsCancellable(accommodationService.GetById(r.AccommodationId).CancellationDeadline), r.IsRateable())));
        }
        private void MoveReservationClick(object sender, RoutedEventArgs e)
        {
            
            MoveReservationAccommodation moveReservationWindow = new MoveReservationAccommodation(LoggedInUser,SelectedReservation.Id);
            moveReservationWindow.Show();
        }

        private void RateTheOwnerClick(object sender, RoutedEventArgs e)
        {

                OwnerAndAccommodationRatingWindow rateWindow = new OwnerAndAccommodationRatingWindow(LoggedInUser, repositoryService.GetById(SelectedReservation.Id), repositoryService.GetById(SelectedReservation.Id).AccommodationId);
                rateWindow.Show();

        }

        //private void CancelledReservationButton(object sender, RoutedEventArgs e)
        //{
        //            YesNoCancelledReservationWindow yesNoWindow = new YesNoCancelledReservationWindow(SelectedReservation);
        //        yesNoWindow.Show();
        //  }
        

        private void CancelledReservationButton(object sender, RoutedEventArgs e)
        {

                YesNoCancelledReservationWindow yesNoWindow = new YesNoCancelledReservationWindow(SelectedReservation);
                yesNoWindow.Show();


        }



    }
}
