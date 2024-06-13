using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using BookingApp.WPF.ViewModels;
using BookingApp.WPF.Views.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
        public static ObservableCollection<OwnerRatingGuestViewModel> OwnerRatingsGuest { get; set; }

        public UserReservationsViewModel SelectedReservation { get; set; }
        public OwnerRatingGuestViewModel SelectedRating {  get; set; }
        public User LoggedInUser { get; set; }
        public AccommodationReservationService accommodationReservationService { get; set; }
        public UserReservationsService reservationsService { get; set; }
        private UserService userService;
        public Accommodation accommodation;
        public GuestRatingService guestRatingService;
        public AccommodationRatingService accommodationRatingService;
        public AccommodationReservation accommodationReservation { get; set;  }
        public AccommodationService accommodationService;
        NotifierService notifierService;
        public ReservationsAccommodationUserControl(User user)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;
            accommodation = new Accommodation();
            accommodationService = new AccommodationService();
            accommodationReservationService = new AccommodationReservationService();
            guestRatingService = new GuestRatingService();
            userService = new UserService();
            accommodationRatingService = new AccommodationRatingService();
            reservationsService = new UserReservationsService();
            ActiveReservations = new ObservableCollection<UserReservationsViewModel>();
            FinishedReservations = new ObservableCollection<UserReservationsViewModel>();
            CancelledReservations = new ObservableCollection<UserReservationsViewModel>();
            OwnerRatingsGuest = new ObservableCollection<OwnerRatingGuestViewModel>();
            notifierService = new NotifierService();

            reservationsService.GetActiveReservationsForUser(LoggedInUser)
                .ForEach(r => ActiveReservations.Add(new UserReservationsViewModel(r.Id, accommodationService.GetAccommodationNameById(r.AccommodationId) , accommodationService.GetById(r.AccommodationId).Location, accommodationService.GetById(r.AccommodationId).ImagesPath, accommodationService.GetById(r.AccommodationId).Capacity, r.ReservedFrom, r.ReservedTo,r.IsCancellable(accommodationService.GetById(r.AccommodationId).CancellationDeadline),r.IsRateable())));
            reservationsService.GetFinishedReservationsForUser(LoggedInUser)
                .ForEach(r => FinishedReservations.Add(new UserReservationsViewModel(r.Id, accommodationService.GetAccommodationNameById(r.AccommodationId), accommodationService.GetById(r.AccommodationId).Location, accommodationService.GetById(r.AccommodationId).ImagesPath, accommodationService.GetById(r.AccommodationId).Capacity, r.ReservedFrom, r.ReservedTo, r.IsCancellable(accommodationService.GetById(r.AccommodationId).CancellationDeadline), r.IsRateable() && accommodationRatingService.IsReservationRated(r.Id))));
            reservationsService.GetCancelledReservationsForUser(LoggedInUser)
                .ForEach(r => CancelledReservations.Add(new UserReservationsViewModel(r.Id, accommodationService.GetAccommodationNameById(r.AccommodationId), accommodationService.GetById(r.AccommodationId).Location, accommodationService.GetById(r.AccommodationId).ImagesPath, accommodationService.GetById(r.AccommodationId).Capacity, r.ReservedFrom, r.ReservedTo, r.IsCancellable(accommodationService.GetById(r.AccommodationId).CancellationDeadline), r.IsRateable())));
            guestRatingService.GetAllRatingsForGuest(user).ForEach(rating =>
                OwnerRatingsGuest.Add(new OwnerRatingGuestViewModel(userService.GetById(accommodationService.GetById(rating.AccommodationId).Owner.Id).AvatarPath, accommodationReservationService.GetById(rating.ReservationId).ReservedFrom, accommodationReservationService.GetById(rating.ReservationId).ReservedTo, rating.Comment, accommodationService.GetById(rating.AccommodationId).Name, accommodationService.GetById(rating.AccommodationId).Location.ToString(), accommodationService.GetById(rating.AccommodationId).Capacity, userService.GetById(accommodationService.GetById(rating.AccommodationId).Owner.Id).FullName, rating.CleanlinessRating, rating.RuleComplianceRating)));

        }
        private void MoveReservationClick(object sender, RoutedEventArgs e)
        {
            
            MoveReservationAccommodation moveReservationWindow = new MoveReservationAccommodation(LoggedInUser,SelectedReservation.Id);
            moveReservationWindow.Show();
        }

        private void RateTheOwnerClick(object sender, RoutedEventArgs e)
        {

                OwnerAndAccommodationRatingWindow rateWindow = new OwnerAndAccommodationRatingWindow(LoggedInUser, accommodationReservationService.GetById(SelectedReservation.Id), accommodationReservationService.GetById(SelectedReservation.Id).AccommodationId);
                rateWindow.ShowDialog();
                FinishedReservations.Clear();
                reservationsService.GetFinishedReservationsForUser(LoggedInUser)
                .ForEach(r => FinishedReservations.Add(new UserReservationsViewModel(r.Id, accommodationService.GetAccommodationNameById(r.AccommodationId), accommodationService.GetById(r.AccommodationId).Location, accommodationService.GetById(r.AccommodationId).ImagesPath, accommodationService.GetById(r.AccommodationId).Capacity, r.ReservedFrom, r.ReservedTo, r.IsCancellable(accommodationService.GetById(r.AccommodationId).CancellationDeadline), r.IsRateable() && accommodationRatingService.IsReservationRated(r.Id))));      
        }
        private void CancelledReservationButton(object sender, RoutedEventArgs e)
        {
                YesNoCancelledReservationWindow yesNoWindow = new YesNoCancelledReservationWindow(SelectedReservation);
                yesNoWindow.Show();

        }

        private void ShowDetails(object sender, RoutedEventArgs e)
        {

            OwnerRateGuestWindow ratingWindow = new OwnerRateGuestWindow(SelectedRating);
            ratingWindow.Show();

        }
        private void PDFActiveReservations_Click(object sender, RoutedEventArgs e)
        {
            notifierService.ShowInformation("PFD report is being downloaded.");
            string pdfPath;
            PDFGenerator pdfGenerator = new PDFGenerator();
            pdfPath = pdfGenerator.CreateGuestActiveReservationsPdf(ActiveReservations);
            notifierService.ShowSuccess("PDF report downloaded successfully!");
        }

        private void PDFCancelledReservations(object sender, RoutedEventArgs e)
        {
            notifierService.ShowInformation("PFD report is being downloaded.");
            string pdfPath;
            PDFGenerator pdfGenerator = new PDFGenerator();
            pdfPath = pdfGenerator.CreateGuestCancelledReservationsPdf(CancelledReservations);      
            notifierService.ShowSuccess("PDF report downloaded successfully!");
        }
    }
}
