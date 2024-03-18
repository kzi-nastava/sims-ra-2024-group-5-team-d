using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
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

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for StatsForAccommodationWindow.xaml
    /// </summary>
    public partial class StatsForAccommodationWindow : Window
    {
        public List<Reservation> Reservations { get; set; }
        public Accommodation Accommodation { get; set; }
        public User LoggedInUser { get; set; }
        private readonly ReservationRepository _repository;
        public ObservableCollection<string> Date { get; set; }
        public ObservableCollection<AccommodationStat> AccommodationStats { get; set; }
        private string selectedYear;
        public StatsForAccommodationWindow(User loggedInUser, Accommodation selectedAccommodation)
        {
            InitializeComponent();
            LoggedInUser = loggedInUser;
            Accommodation = selectedAccommodation;
            _repository = new ReservationRepository();
            DataContext = this;
            Date = new ObservableCollection<string>();
            AccommodationStats = new ObservableCollection<AccommodationStat>();
            Reservations = new List<Reservation>(_repository.GetByAccommodation(Accommodation));
            if(Reservations.Count!=0)
                Update();
            else
            {
                MessageBox.Show("There are no reservations for this accommodation");
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Update()
        {
            InitializeComboBox();
            ShowAccommodationStats();
        }

        private void comboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (comboBox.SelectedItem != null)
            {
                selectedYear = comboBox.Text;
                AccommodationStats.Clear();
                ShowAccommodationStats();
            }
        }
        private void ShowAccommodationStats()
        {
            if (Reservations.Count > 0)
            {
                if (selectedYear == "All years")
                    ShowYearlyAccommodationStats();
                else
                    ShowMonthlyAccommodationStats();
            }
        }

        private void ShowMonthlyAccommodationStats()
        {
            GetReservationsForSelectedYear();
            SortReservations();
            GetMonthlyAccommodationStats();
            FindAndShowMostBusy();
        }

        private void ShowYearlyAccommodationStats()
        {
            Reservations = _repository.GetByAccommodation(Accommodation);
            SortReservations();
            GetYearlyAccommodationStats();
            FindAndShowMostBusy();
        }

        private void SortReservations()
        {
            Reservations.Sort((r1, r2) => r1.ReservedFrom.CompareTo(r2.ReservedFrom));

        }

        private void GetReservationsForSelectedYear()
        {
            Reservations = _repository.GetByAccommodation(Accommodation)
                .Where(reservation => IsReservationInSelectedYear(reservation, Convert.ToInt32(selectedYear))).ToList();
        }

        private bool IsReservationInSelectedYear(Reservation reservation, int selectedYear)
        {
            return reservation.ReservedFrom.Year == selectedYear || reservation.ReservedTo.Year == selectedYear;
        }

        private void InitializeComboBox()
        {
            Date.Add("All years");
            comboBox.SelectedIndex = 0;
            selectedYear = "All years";
            SortReservations();
            int LastBusyYear = Reservations[Reservations.Count - 1].ReservedTo.Year;
            int FirstBusyYear = Reservations[0].ReservedFrom.Year;
            for (int i = LastBusyYear; i >= FirstBusyYear; i--)
                Date.Add(i.ToString());
        }
        private void FindAndShowMostBusy()
        {
            AccommodationStat mostBusy = GetMostBusy();
            ShowMostBusy(mostBusy);

        }

        private void ShowMostBusy(AccommodationStat mostBusy)
        {
            if (selectedYear == "All years")
                MostReservationsLabel.Content = "Most visited year is: " + mostBusy.RowHeader.Split(' ')[1] + "   with busyness of: " + mostBusy.Busyness;
            else
                MostReservationsLabel.Content = "Most visited month is: " + mostBusy.RowHeader.Split(' ')[1] + " with busyness of: " + mostBusy.Busyness;
        }

        private AccommodationStat GetMostBusy()
        {
            return AccommodationStats.OrderByDescending(aS => aS.Busyness).First();

        }

        private void GetYearlyAccommodationStats()
        {
            int LastBusyYear = Reservations[Reservations.Count - 1].ReservedTo.Year;
            int FirstBusyYear = Reservations[0].ReservedFrom.Year;
            for (int i = LastBusyYear; i >= FirstBusyYear; i--)
            {
                AccommodationStats.Add(CreateAccommodationStatForYear(i));
            }
        }
        private void GetMonthlyAccommodationStats()
        {
            for (int i = 1; i <= 12; i++)
            {
                AccommodationStats.Add(CreateAccommodationStatForMonth(i));
            }
        }
        private AccommodationStat CreateAccommodationStatForYear(int year)
        {
            AccommodationStat accommodationStat = new AccommodationStat
            {
                NumberOfReservations = Reservations.Count(r => r.ReservedFrom.Year == year),
                NumberOfCancelledReservations = Reservations.Where(r => r.ReservedFrom.Year == year).Sum(r => r.Cancelled),
                NumberOfRecommendedRenovations = Reservations.Where(r => r.ReservedFrom.Year == year).Sum(r => r.RecommendedRenovation),
                NumberOfRescheduledReservations = Reservations.Where(r => r.ReservedFrom.Year == year).Sum(r => r.RescheduledReservation),
                Busyness = CalculateYearlyBusyness(year),
                RowHeader = $"Year: {year}"
            };

            return accommodationStat;
        }
        private AccommodationStat CreateAccommodationStatForMonth(int month)
        {
            AccommodationStat accommodationStat = new AccommodationStat
            {
                NumberOfReservations = Reservations.Where(r=> r.ReservedFrom.Year.ToString() == selectedYear).Count(r => r.ReservedFrom.Month == month),
                NumberOfCancelledReservations = Reservations.Where(r => r.ReservedFrom.Month == month && r.ReservedFrom.Year.ToString() == selectedYear).Sum(r => r.Cancelled),
                NumberOfRecommendedRenovations = Reservations.Where(r => r.ReservedFrom.Month == month && r.ReservedFrom.Year.ToString() == selectedYear).Sum(r => r.RecommendedRenovation),
                NumberOfRescheduledReservations = Reservations.Where(r => r.ReservedFrom.Month == month && r.ReservedFrom.Year.ToString() == selectedYear).Sum(r => r.RescheduledReservation),
                Busyness = CalculateMonthlyBusyness(month),
                RowHeader = $"Month: {CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month)}"
            };

            return accommodationStat;
        }

        private double CalculateYearlyBusyness(int year)
        {
            int numberOfDaysInYear = GetNumberOfDaysInYear(year);
            int numberOfReservationsinYear = GetNumberOfReservedDaysInYear(year);
            double busyness = (double)numberOfReservationsinYear / numberOfDaysInYear;
            return busyness;
        }
        private double CalculateMonthlyBusyness(int month)
        {
            int numberOfDaysInMonth = GetNumberOfDaysInMonth(month);
            int numberOfReservationsInMonth = GetNumberOfReservationsInMonth(month);
            double busyness = (double)numberOfReservationsInMonth / numberOfDaysInMonth;
            return busyness;
        }
        private int GetNumberOfDaysInYear(int currentYear)
        {
            return DateTime.IsLeapYear(currentYear) ? 366 : 365;
        }
        private int GetNumberOfDaysInMonth(int month)
        { 
            return DateTime.DaysInMonth(Convert.ToInt32(selectedYear), month);
        }
        private int GetNumberOfReservedDaysInYear(int currentYear)
        {
            return Reservations.Where(r => (IsReservationInSelectedYear(r, currentYear)) && IsNotCancelled(r))
                .Sum(r =>
                {
                    if (r.ReservedFrom.Year == r.ReservedTo.Year)
                        return (r.ReservedTo - r.ReservedFrom).Days;
                    else if (r.ReservedFrom.Year != currentYear && r.ReservedTo.Year == currentYear)
                        return (r.ReservedTo - new DateTime(r.ReservedTo.Year, 1, 1)).Days;
                    else if(r.ReservedTo.Year != currentYear && r.ReservedFrom.Year == currentYear)
                        return (new DateTime(r.ReservedFrom.Year, 12, 31) - r.ReservedFrom).Days;
                    else 
                        return 0;
                });
        }

        private bool IsNotCancelled(Reservation r)
        {
            return r.Cancelled == 0;
        }

        private int GetNumberOfReservationsInMonth(int month)
        {

            return Reservations.Where(r => (IsReservationInSelectedMonth(r, month)) && IsNotCancelled(r))
                .Sum(r => {
                    if (r.ReservedFrom.Year.ToString() != selectedYear && r.ReservedFrom.Month == month)
                        return 0;
                    else if (r.ReservedTo.Month == month && r.ReservedFrom.Month != month)
                        return (r.ReservedTo - new DateTime(r.ReservedTo.Year, r.ReservedTo.Month, 1)).Days;
                    else if (r.ReservedFrom.Month == month && r.ReservedTo.Month != month)
                        return (new DateTime(r.ReservedFrom.Year, r.ReservedFrom.Month, DateTime.DaysInMonth(r.ReservedFrom.Year, r.ReservedFrom.Month)) - r.ReservedFrom).Days;
                    else if (r.ReservedFrom.Month == r.ReservedTo.Month)
                        return (r.ReservedTo - r.ReservedFrom).Days;
                    else return 0;
                });
        }
        private bool IsReservationInSelectedMonth(Reservation reservation,int month)
        {
            return reservation.ReservedFrom.Month == month || reservation.ReservedTo.Month == month;
        }
            
    }
}
