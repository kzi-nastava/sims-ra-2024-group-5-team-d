using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Repositories;
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

namespace BookingApp.WPF.Views
{
    /// <summary>
    /// Interaction logic for ReservationsWindow.xaml
    /// </summary>
    public partial class ReservationsWindow : Window 
    {

        private DateTime fromDate=DateTime.UtcNow;
        public DateTime FromDate
        {
            get { return fromDate; }
            set
            {
                fromDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime toDate=DateTime.UtcNow;
        public DateTime ToDate
        {
            get { return toDate; }
            set
            {
                toDate = value;
                OnPropertyChanged();
            }
        }
        private int numberOfPeople;
        public int NumberOfPeople
        {
            get => numberOfPeople;
            set
            {
                if (value != numberOfPeople)
                {
                    numberOfPeople = value;
                    EnableReserveButton();
                    OnPropertyChanged();
                }
            }
        }
        private int numberOfDays;
        public int NumberOfDays
        {
            get => numberOfDays;
            set
            {
                if (value != numberOfDays)
                {
                    if (value < Accommodation.MinStay)
                    {
                        DateCheck.IsEnabled = false;
                    }
                    else
                    {
                        DateCheck.IsEnabled = true;
                    }
                    numberOfDays = value;
                    OnPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private Accommodation accommodation;
        public Accommodation Accommodation
        {
            get => accommodation;
            set
            {
                if (value != accommodation)
                {
                    accommodation = value;
                    OnPropertyChanged();
                }
            }
        }
        private readonly AccommodationReservationRepository _reservationRepository;
        public User LoggedInUser { get; set; }
        private readonly AccommodationRepository _repository;
        public ObservableCollection<KeyValuePair<DateTime, DateTime>> AvailableDates { get; set; }
        public KeyValuePair<DateTime, DateTime> SelectedDate { get; set; }
        private AvailableDatesForReservationService AvailableDatesForReservationService;

        public ReservationsWindow(User user, Accommodation selectedAccommmodation)  
        {

            _reservationRepository = new AccommodationReservationRepository();
            LoggedInUser = user;
            AvailableDatesForReservationService = new AvailableDatesForReservationService();
            AvailableDates = new ObservableCollection<KeyValuePair<DateTime, DateTime>>();
            Accommodation = selectedAccommmodation;
            DataContext = this;
            InitializeComponent();

        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DateCheckClick(object sender, RoutedEventArgs e)
        {
            AvailableDates.Clear();
            NotAvailableLabel.Visibility = Visibility.Collapsed;
            AvailableDatesForReservationService.CheckAvailableDatesInGivenRange(fromDate,toDate,numberOfDays,Accommodation).ForEach( availableDate=> AvailableDates.Add(availableDate));
            if (AvailableDates.Count() != 0)
                ShowReservationControls();
            else
            {
                AvailableDatesForReservationService.FindandShowAvailableDatesForExtendendRange(fromDate, toDate, numberOfDays, Accommodation).ForEach(availableDate => AvailableDates.Add(availableDate));
                ShowReservationControls();
                NotAvailableLabel.Visibility = Visibility.Visible;
            }
        }
      
        private void ShowReservationControls()
        {
            NumberOfPeopleLabel.Visibility = Visibility.Visible;
            NumberOfPeopleTextBox.Visibility = Visibility.Visible;
            ReserveButton.Visibility = Visibility.Visible;
            ReserveButton.IsEnabled = false;
        }

        private void EnableReserveButton()
        {
            bool isAvailableDateSelected = SelectedDate.Key != null && SelectedDate.Value != null;
            bool isNumberOfPeopleValid = numberOfPeople <= Accommodation.Capacity && numberOfPeople != 0;
            if (isAvailableDateSelected && isNumberOfPeopleValid)
                ReserveButton.IsEnabled = true;
            else
                ReserveButton.IsEnabled = false;
        }
        private void ReserveAccommodation(object sender, RoutedEventArgs e)
        {
            AccommodationReservation newReservation = new AccommodationReservation(Accommodation.Id, LoggedInUser.Id, SelectedDate.Key, SelectedDate.Value);
            AccommodationReservation savedAccommodation = _reservationRepository.Save(newReservation);
            Close();
        }

        private void AvailabilityDateGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableReserveButton();
        }
    }
}
