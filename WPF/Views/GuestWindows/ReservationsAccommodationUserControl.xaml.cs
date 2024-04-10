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
        private bool _cancellationEnabled;
        public bool CancellationEnabled
        {
            get { return _cancellationEnabled; }
            set
            {
                if (_cancellationEnabled != value)
                {
                    _cancellationEnabled = value;
                    OnPropertyChanged(nameof(CancellationEnabled));
                }
            }
        }
        private bool _rateEnabled;
        public bool RateEnabled
        {
            get { return _rateEnabled; }
            set
            {
                if (_rateEnabled != value)
                {
                    _rateEnabled = value;
                    OnPropertyChanged(nameof(RateEnabled));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public static ObservableCollection<UserReservationsViewModel> ActiveReservations { get; set; }
        public static ObservableCollection<UserReservationsViewModel> FinishedReservations { get; set; }
        public static ObservableCollection<UserReservationsViewModel> CancelledReservations { get; set; }

        public UserReservationsViewModel SelectedReservation { get; set; }
        public User LoggedInUser { get; set; }
        public AccommodationReservationRepository repository { get; set; }
        public UserReservationsService reservationsService { get; set; }
        public Accommodation accommodation;
        public AccommodationReservation accommodationReservation { get; set; }
        private IAccommodationRepository accommodationRepository;
        private IAccommodationReservationRepository accommodationReservationRepository;
        public ReservationsAccommodationUserControl(User user)
        {
            accommodationReservationRepository=Injector.CreateInstance<IAccommodationReservationRepository>();
            accommodationRepository=Injector.CreateInstance<IAccommodationRepository>();
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;
            accommodation = new Accommodation();
            repository = new AccommodationReservationRepository();
            reservationsService = new UserReservationsService();
            ActiveReservations = new ObservableCollection<UserReservationsViewModel>();
            FinishedReservations = new ObservableCollection<UserReservationsViewModel>();
            CancelledReservations = new ObservableCollection<UserReservationsViewModel>();
 
            reservationsService.GetActiveReservationsForUser(LoggedInUser)
                .ForEach(r => ActiveReservations.Add(new UserReservationsViewModel(r.Id,accommodationRepository.GetAccommodationNameById(r.AccommodationId) , accommodationRepository.GetById(r.AccommodationId).Location, accommodationRepository.GetById(r.AccommodationId).ImagesPath, accommodationRepository.GetById(r.AccommodationId).Capacity, r.ReservedFrom, r.ReservedTo)));
            reservationsService.GetFinishedReservationsForUser(LoggedInUser)
                .ForEach(r => FinishedReservations.Add(new UserReservationsViewModel(r.Id, accommodationRepository.GetAccommodationNameById(r.AccommodationId), accommodationRepository.GetById(r.AccommodationId).Location, accommodationRepository.GetById(r.AccommodationId).ImagesPath, accommodationRepository.GetById(r.AccommodationId).Capacity, r.ReservedFrom, r.ReservedTo)));
            reservationsService.GetCancelledReservationsForUser(LoggedInUser)
                .ForEach(r => CancelledReservations.Add(new UserReservationsViewModel(r.Id, accommodationRepository.GetAccommodationNameById(r.AccommodationId), accommodationRepository.GetById(r.AccommodationId).Location, accommodationRepository.GetById(r.AccommodationId).ImagesPath, accommodationRepository.GetById(r.AccommodationId).Capacity, r.ReservedFrom, r.ReservedTo)));
        }
        private void MoveReservationClick(object sender, RoutedEventArgs e)
        {
            
            MoveReservationAccommodation moveReservationWindow = new MoveReservationAccommodation();
            moveReservationWindow.Show();
        }

        private void RateTheOwnerClick(object sender, RoutedEventArgs e)
        {
            if (accommodationReservationRepository.GetById(SelectedReservation.Id).IsRateable())
            {
                OwnerAndAccommodationRatingWindow rateWindow = new OwnerAndAccommodationRatingWindow(LoggedInUser, accommodationReservationRepository.GetById(SelectedReservation.Id), accommodationReservationRepository.GetById(SelectedReservation.Id).AccommodationId);
                rateWindow.Show();
                RateEnabled = true;
            }
            else
            {
                RateEnabled = false;
            }
        }

        //private void CancelledReservationButton(object sender, RoutedEventArgs e)
        //{
        //            YesNoCancelledReservationWindow yesNoWindow = new YesNoCancelledReservationWindow(SelectedReservation);
        //        yesNoWindow.Show();
        //  }
        

        private void CancelledReservationButton(object sender, RoutedEventArgs e)
        {
            if (repository.IsCancellable(accommodation , accommodationReservationRepository.GetById(SelectedReservation.Id))) //accommodation kako sam pozvalaaaa
            {
                YesNoCancelledReservationWindow yesNoWindow = new YesNoCancelledReservationWindow(SelectedReservation);
                yesNoWindow.Show();
                CancellationEnabled = true;
            }
            else
            {
                CancellationEnabled = false;
            }
        }



    }
}
