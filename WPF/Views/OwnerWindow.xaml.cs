using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
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
using System.Xml.Linq;

namespace BookingApp.WPF.Views
{
    /// <summary>
    /// Interaction logic for OwnerWindow.xaml
    /// </summary>
    public partial class OwnerWindow : Window
    {
        public static ObservableCollection<Accommodation> Accommodations { get; set; }
        public Accommodation selectedAccommodation { get; set; }
        public User LoggedInUser { get; set; }
        private readonly AccommodationRepository _repository;
        private readonly AccommodationReservationService _accommodationReservationService;
        private CheckForUnratedGuestsService CheckForUnratedGuestsService;


        public OwnerWindow(User user)
        {
            InitializeComponent();
            LoggedInUser = user;
            DataContext = this;
            CheckForUnratedGuestsService = new CheckForUnratedGuestsService();
            _repository = new AccommodationRepository();
            _accommodationReservationService = new AccommodationReservationService();
            Accommodations = new ObservableCollection<Accommodation>(_repository.GetByUser(user));
           if(CheckForUnratedGuestsService.CheckForUnratedGuestsByLoggedInUser(LoggedInUser))
            {
                RateLabel.Visibility = Visibility.Visible;
                RateButton.Visibility = Visibility.Visible;
            }
            else
            {
                RateLabel.Visibility = Visibility.Hidden;
                RateButton.Visibility = Visibility.Hidden;
            }
        }

        private void RegisterPropertyButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterAccommodationWindow registerAccommodationWindow = new RegisterAccommodationWindow(LoggedInUser);
            registerAccommodationWindow.ShowDialog();
        }

        private void ShowStatsButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedAccommodation!=null )
            {
                if (_accommodationReservationService.GetByAccommodation(selectedAccommodation).Count != 0)
                {
                    StatsForAccommodationWindow statsForAccommodationWindow = new StatsForAccommodationWindow(LoggedInUser, selectedAccommodation);
                    statsForAccommodationWindow.Owner = this;
                    statsForAccommodationWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    statsForAccommodationWindow.ShowDialog();
                }
                else
                {
                    MessageBox.Show("There are no reservations for this accommodation.");
                }
            }
        }

        private void RateGuestsButton_Click(object sender, RoutedEventArgs e)
        {
            RateGuestsWindow rateGuestsWindow = new RateGuestsWindow(LoggedInUser);
            rateGuestsWindow.Owner = this;
            rateGuestsWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            rateGuestsWindow.ShowDialog();
            if (CheckForUnratedGuestsService.CheckForUnratedGuestsByLoggedInUser(LoggedInUser))
            {
                RateLabel.Visibility = Visibility.Visible;
                RateButton.Visibility = Visibility.Visible;
            }
            else
            {
                RateLabel.Visibility = Visibility.Hidden;
                RateButton.Visibility = Visibility.Hidden;
            }
        }
    }
}
