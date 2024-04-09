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
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ObservableCollection<UserReservationsViewModel> ActiveReservations { get; set; }
        public ObservableCollection<UserReservationsViewModel> FinishedReservations { get; set; }
        public ObservableCollection<UserReservationsViewModel> CancelledReservations { get; set; }

        public UserReservationsViewModel SelectedActiveReservation;
        public User LoggedInUser { get; set; }
        public AccommodationReservationRepository repository { get; set; }
        public UserReservationsService reservationsService { get; set; } 
        public AccommodationReservation accommodationReservation { get; set; }
        private IAccommodationRepository accommodationRepository;
        public ReservationsAccommodationUserControl(User user)
        {
            accommodationRepository=Injector.CreateInstance<IAccommodationRepository>();
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;
            DataContext = this;
            SelectedActiveReservation = new UserReservationsViewModel();
            accommodationReservation = new AccommodationReservation();
            repository = new AccommodationReservationRepository();
            reservationsService = new UserReservationsService();
            ActiveReservations = new ObservableCollection<UserReservationsViewModel>();
            FinishedReservations = new ObservableCollection<UserReservationsViewModel>();
            CancelledReservations = new ObservableCollection<UserReservationsViewModel>();
            reservationsService.GetActiveReservationsForUser(LoggedInUser,accommodationReservation)
                .ForEach(r => ActiveReservations.Add(new UserReservationsViewModel(r.Id,accommodationRepository.GetAccommodationNameById(r.AccommodationId) , accommodationRepository.GetById(r.AccommodationId).Location, accommodationRepository.GetById(r.AccommodationId).ImagesPath, accommodationRepository.GetById(r.AccommodationId).Capacity, r.ReservedFrom, r.ReservedTo)));
            reservationsService.GetFinishedReservationsForUser(LoggedInUser, accommodationReservation)
                .ForEach(r => FinishedReservations.Add(new UserReservationsViewModel(r.Id, accommodationRepository.GetAccommodationNameById(r.AccommodationId), accommodationRepository.GetById(r.AccommodationId).Location, accommodationRepository.GetById(r.AccommodationId).ImagesPath, accommodationRepository.GetById(r.AccommodationId).Capacity, r.ReservedFrom, r.ReservedTo)));
            reservationsService.GetCancelledReservationsForUser(LoggedInUser, accommodationReservation)
                .ForEach(r => CancelledReservations.Add(new UserReservationsViewModel(r.Id, accommodationRepository.GetAccommodationNameById(r.AccommodationId), accommodationRepository.GetById(r.AccommodationId).Location, accommodationRepository.GetById(r.AccommodationId).ImagesPath, accommodationRepository.GetById(r.AccommodationId).Capacity, r.ReservedFrom, r.ReservedTo)));
        }
        private void MoveReservationClick(object sender, RoutedEventArgs e)
        {
            MoveReservationAccommodation moveReservationWindow = new MoveReservationAccommodation();
            moveReservationWindow.Show();
        }

        private void RateTheOwner(object sender, RoutedEventArgs e)
        {
            OwnerAndAccommodationRatingWindow rateWindow = new OwnerAndAccommodationRatingWindow();
            rateWindow.Show();
        }
    }
}
