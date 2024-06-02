using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
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

namespace BookingApp.WPF.Views.GuestWindows
{
    /// <summary>
    /// Interaction logic for AccommodationUserControl.xaml
    /// </summary>
    public partial class AccommodationUserControl : UserControl
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private AccommodationViewModel accommodation;
        public AccommodationViewModel Accommodation
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
        private DateTime fromDate = DateTime.UtcNow;
        public DateTime FromDate
        {
            get { return fromDate; }
            set
            {
                fromDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime toDate = DateTime.UtcNow;
        public DateTime ToDate
        {
            get { return toDate; }
            set
            {
                toDate = value;
                OnPropertyChanged();
            }
        }
        private int numberOfPeople=1;
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
        private int numberOfDays=1;
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
        public string ImagesPath { get; set; }
        public string AccommodationName { get; set; }
        public int Capacity { get; set; }
        public int MinStay { get; set; }
        public Location Location { get; set; }
        public double AverageRating { get; set; }
        public DateTime CancellationDeadline { get; set; }

        public User LoggedInUser;
        private readonly AccommodationReservationService _reservationService;
        private readonly AccommodationRepository _repository;
        private readonly ContentControl contentControl;
        NotifierService notifierService;

        public ObservableCollection<KeyValuePair<DateTime, DateTime>> AvailableDates { get; set; }
        public KeyValuePair<DateTime, DateTime> SelectedDate { get; set; }
        private AvailableDatesForReservationService AvailableDatesForReservationService;
        private readonly SuperUserService superUserService;
        private AccommodationService accommodationService;

        public AccommodationUserControl(User user, AccommodationViewModel selectedAccommmodation)
        {
            InitializeComponent();
            LoggedInUser = user;
            _reservationService = new AccommodationReservationService();
            _repository = new AccommodationRepository();
            AvailableDatesForReservationService = new AvailableDatesForReservationService();
            AvailableDates = new ObservableCollection<KeyValuePair<DateTime, DateTime>>();
            Accommodation = selectedAccommmodation;
            superUserService = new SuperUserService();
            accommodationService = new AccommodationService();
            notifierService = new NotifierService();

            AccommodationName = accommodationService.GetAccommodationNameById(selectedAccommmodation.Id);
            ImagesPath = accommodationService.GetById(selectedAccommmodation.Id).ImagesPath;
            Location = accommodationService.GetById(selectedAccommmodation.Id).Location;
            Capacity = accommodationService.GetById(selectedAccommmodation.Id).Capacity;
            MinStay = accommodationService.GetById(selectedAccommmodation.Id).MinStay;
            AverageRating = accommodationService.GetById(selectedAccommmodation.Id).AverageRating;
            CancellationDeadline = fromDate.AddDays(-accommodationService.GetById(selectedAccommmodation.Id).CancellationDeadline);

            DataContext = this;

        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Grid grid = FindName("Grid") as Grid;


            if (grid != null)
            {

                foreach (var control in grid.Children)
                {
                    if (control is TextBox textBox)
                    {
                        textBox.Text = string.Empty;
                    }
                    else if (control is DatePicker datePicker)
                    {
                        datePicker.SelectedDate = null;
                    }
                    else if (control is DataGrid dataGrid)
                    {
                        dataGrid.ItemsSource = null;
                    }
                }
            }
        }
        private void DateCheckClick(object sender, RoutedEventArgs e)
        {
            AvailableDates.Clear();
            NotAvailableLabel.Visibility = Visibility.Collapsed;
            AvailableDatesForReservationService.GetAvailableDatesInGivenRange(fromDate, toDate, numberOfDays, _repository.GetById(Accommodation.Id)).ForEach(availableDate => AvailableDates.Add(availableDate));
            if (AvailableDates.Count() != 0)
                ShowReservationControls();
            else
            {
                AvailableDatesForReservationService.FindandShowAvailableDatesForExtendendRange(fromDate, toDate, numberOfDays, _repository.GetById(Accommodation.Id)).ForEach(availableDate => AvailableDates.Add(availableDate));
                ShowReservationControls();
                NotAvailableLabel.Visibility = Visibility.Visible;
            }
        }

        private void ShowReservationControls()
        {
            NumberOfPeopleLabel.Visibility = Visibility.Visible;
            NumberOfPeopleTextBox.Visibility = Visibility.Visible;
            TextMessage.Visibility = Visibility.Visible;
            TextCapacity.Visibility = Visibility.Visible;
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
            AccommodationReservation newReservation = new AccommodationReservation(Accommodation.Id, LoggedInUser.Id, SelectedDate.Key, SelectedDate.Value, numberOfPeople);
            AccommodationReservation savedAccommodation = _reservationService.Save(newReservation);
            if (LoggedInUser.IsSuper())
            {
                superUserService.IsDiscountUsed(LoggedInUser);
            }
            notifierService.ShowSuccess("Reservation accepted");
            GuestWindow.contentControl.Content = new AccommodationUserControl(LoggedInUser, Accommodation);
        }

        private void AvailabilityDateGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableReserveButton();
        }

        private void OpenGallery_Click(object sender, RoutedEventArgs e)
        {
            GuestWindow.contentControl.Content = new AccommodationGalleryUserControl(LoggedInUser, Accommodation);
        }
    }
}
