using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Repositories;
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

namespace BookingApp.WPF.Views
{
    /// <summary>
    /// Interaction logic for StatsForAccommodationWindow.xaml
    /// </summary>
    public partial class StatsForAccommodationWindow : Window
    {
        public List<AccommodationReservation> Reservations { get; set; }
        public Accommodation Accommodation { get; set; }
        public User LoggedInUser { get; set; }
        private readonly AccommodationReservationRepository _repository;
        public ObservableCollection<string> Date { get; set; }
        public ObservableCollection<AccommodationStat> AccommodationStats { get; set; }
        private string selectedYear;
        private ShowAccommodationStatsService ShowAccommodationStatsService;
        public StatsForAccommodationWindow(User loggedInUser, Accommodation selectedAccommodation)
        {
            InitializeComponent();
            ShowAccommodationStatsService = new ShowAccommodationStatsService();
            LoggedInUser = loggedInUser;
            Accommodation = selectedAccommodation;
            _repository = new AccommodationReservationRepository();
            DataContext = this;
            Date = new ObservableCollection<string>();
            AccommodationStats = new ObservableCollection<AccommodationStat>();
            Reservations = new List<AccommodationReservation>(_repository.GetByAccommodation(Accommodation));
            if (Reservations.Count != 0)
            {
                InitializeComboBox();
                Update();
            }
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
            //DA LI OVO DA RAZDVOJIM NA 2 SERVISA TIPA SERVIS ZA
            //MONTHLY STATS I SERVIS ZA YEARLY STATS KOJI POZIVAJU TRECI SERVIS UKOLIKO
            //IMAJU ZAJEDNICKE FUNKCIJE?????
           //OVE 2 FUNKCIJE MOGU DA SE UPROSTE DA SE U SERVISU PROVERAVA SELECTEDYEAR I DA SE POZOVE ODGOVARAJUCA FUNKCIJA CAK SU I 90% ISTE
            if (selectedYear == "All years")
            {
                Reservations = ShowAccommodationStatsService.ShowYearlyAccommodationStats(selectedYear, Accommodation);
                ShowAccommodationStatsService.GetYearlyAccommodationStats(Reservations).ForEach(stat => AccommodationStats.Add(stat));
            }
            else
            {
                Reservations = ShowAccommodationStatsService.ShowMonthlyAccommodationStats(selectedYear, Accommodation);
                ShowAccommodationStatsService.GetMonthlyAccommodationStats(Reservations, selectedYear).ForEach(stat => AccommodationStats.Add(stat));
            }
            MostReservationsLabel.Content = ShowAccommodationStatsService.FindMostBusy(selectedYear, AccommodationStats);
        }

        private void comboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (comboBox.SelectedItem != null)
            {
                selectedYear = comboBox.Text;
                AccommodationStats.Clear();
                Update();            
            }
        }

        private void SortReservations()
        {
            Reservations.Sort((r1, r2) => r1.ReservedFrom.CompareTo(r2.ReservedFrom));

        }
        //DA LI ZA OVO NOVI SERVIS
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
       

            
    }
}
