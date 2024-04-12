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
        public Accommodation Accommodation { get; set; }
        public User LoggedInUser { get; set; }
        private readonly AccommodationReservationService _repositoryService;
        public ObservableCollection<string> Date { get; set; }
        public ObservableCollection<AccommodationStat> AccommodationStats { get; set; }
        private string selectedYear;
        private AccommodationStatsService accommodationStatsService;
        public StatsForAccommodationWindow(User loggedInUser, Accommodation selectedAccommodation)
        {
            InitializeComponent();
            accommodationStatsService = new AccommodationStatsService();
            LoggedInUser = loggedInUser;
            Accommodation = selectedAccommodation;
            _repositoryService = new AccommodationReservationService();
            DataContext = this;
            Date = new ObservableCollection<string>();
            AccommodationStats = new ObservableCollection<AccommodationStat>();
            InitializeComboBox();
            Update();
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Update()
        {
            accommodationStatsService.GetAccommodationStats(selectedYear, Accommodation).ForEach(stat => AccommodationStats.Add(stat));
            MostReservationsLabel.Content = accommodationStatsService.FindMostBusy(selectedYear, AccommodationStats);
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
        //DA LI ZA OVO NOVI SERVIS
        private void InitializeComboBox()
        {
            List<AccommodationReservation> Reservations =new List<AccommodationReservation>(_repositoryService.GetByAccommodation(Accommodation));
            Date.Add("All years");
            comboBox.SelectedIndex = 0;
            selectedYear = "All years";
            accommodationStatsService. SortReservations(Reservations);
            int LastBusyYear = Reservations[Reservations.Count - 1].ReservedTo.Year;
            int FirstBusyYear = Reservations[0].ReservedFrom.Year;
            for (int i = LastBusyYear; i >= FirstBusyYear; i--)
                Date.Add(i.ToString());
        }
       

            
    }
}
