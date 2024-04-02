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

        private List<AccommodationReservation> reservedDatesForAccommodation;
        public User LoggedInUser { get; set; }
        private readonly AccommodationRepository _repository;
        private readonly AccommodationReservationRepository _reservationRepository;
        public ObservableCollection<KeyValuePair<DateTime, DateTime>> AvailableDates { get; set; }
        public KeyValuePair<DateTime, DateTime> SelectedDate { get; set; }

        public ReservationsWindow(User user, Accommodation selectedAccommmodation)  
        {
            _reservationRepository = new AccommodationReservationRepository();
            reservedDatesForAccommodation = _reservationRepository.GetByAccommodation(selectedAccommmodation);
            LoggedInUser = user;
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
            CheckAvailableDatesInGivenRange();   
        }
        private void CheckAvailableDatesInGivenRange()
        {
            FindReservedDatesInRange();
            if (reservedDatesForAccommodation.Count != 0)
                FindAndShowAvailableDates();
            else
                ShowAvailableDates(fromDate, toDate);
            if (AvailableDates.Count() != 0)
                ShowReservationControls();
            else
            {
                FindandShowAvailableDatesForExtendendRange();
                ShowReservationControls();
                NotAvailableLabel.Visibility = Visibility.Visible;
            }
        }

        private void FindandShowAvailableDatesForExtendendRange()
        {
            while (AvailableDates.Count < 5)
            {
                AvailableDates.Clear();
                FromDate = fromDate.AddDays(-1);
                ToDate = toDate.AddDays(+1);
                FindReservedDatesInRange();
                FindAndShowAvailableDates();
            }
            if (AvailableDates.Count > 5)
                AvailableDates.RemoveAt(AvailableDates.Count - 1);
        }

        private void FindAndShowAvailableDates()
        {
            DateTime newFromDate = fromDate;
            DateTime newToDate;
            foreach (AccommodationReservation reservedDates in reservedDatesForAccommodation)
            {
                if (reservedDates.ReservedFrom < fromDate)
                {
                    newFromDate = reservedDates.ReservedTo;
                    continue;
                }
                newToDate = reservedDates.ReservedFrom;
                ShowAvailableDates(newFromDate, newToDate);
                newFromDate = reservedDates.ReservedTo;

            }
            if (newFromDate < ToDate)
                ShowAvailableDates(newFromDate, toDate);
        }

        private void ShowReservationControls()
        {
            NumberOfPeopleLabel.Visibility = Visibility.Visible;
            NumberOfPeopleTextBox.Visibility = Visibility.Visible;
            ReserveButton.Visibility = Visibility.Visible;
            ReserveButton.IsEnabled = false;
        }

        private void ShowAvailableDates(DateTime fromDate,DateTime toDate)
        {
            DateTime LastAvailableDate = toDate.AddDays(-numberOfDays);
            for (; fromDate <= LastAvailableDate; fromDate = fromDate.AddDays(1))
            {
                AvailableDates.Add(MakeAvailableDatesPair(fromDate));
            }
        }

        private KeyValuePair<DateTime, DateTime> MakeAvailableDatesPair(DateTime start)
        {
            return new KeyValuePair<DateTime, DateTime>(start, start.AddDays(numberOfDays));
        }

        private void FindReservedDatesInRange()
        {
            reservedDatesForAccommodation = _reservationRepository.GetByAccommodation(Accommodation);
            reservedDatesForAccommodation.RemoveAll(reservation => IsReservationOutOfRange(reservation) || IsReservationCancelled(reservation));
            reservedDatesForAccommodation.Sort((r1, r2) => r1.ReservedFrom.CompareTo(r2.ReservedFrom));
        }

        private static bool IsReservationCancelled(AccommodationReservation reservation)
        {
            return reservation.Cancelled == 1;
        }

        private  bool IsReservationOutOfRange(AccommodationReservation reservation)
        {
            return fromDate > reservation.ReservedTo || toDate < reservation.ReservedFrom;
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
