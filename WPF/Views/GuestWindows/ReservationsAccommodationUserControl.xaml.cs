using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Repositories;
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
        public ObservableCollection<Accommodation> Accommodations { get; set; }
        public ObservableCollection<AccommodationReservation> Reservations { get; set; }
        public User LoggedInUser { get; set; }
        private readonly AccommodationRepository repository;
        private readonly AccommodationReservationRepository repositoryReservation;
        public AccommodationReservation SelectedReservation { get; set; }
        public Accommodation Accommodation { get; set; }
        public ReservationsAccommodationUserControl(User user, Accommodation accommodation)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;
            Accommodation = accommodation;
            DataContext = this;
            repository = new AccommodationRepository();
            //repositoryReservation = new AccommodationReservationRepository();
            Accommodations = new ObservableCollection<Accommodation>(repository.GetAll());
            //Reservations = new ObservableCollection<AccommodationReservation>(repositoryReservation.GetByAccommodation(Accommodation));

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
