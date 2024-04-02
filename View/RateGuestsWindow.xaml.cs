using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Services;
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

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for RateGuestsWindow.xaml
    /// </summary>
    public partial class RateGuestsWindow : Window
    {

        public GuestRatingDTO SelectedGuestRatingDTO { get; set; }
        private UnratedGuestService unratedGuestService;
        public static ObservableCollection<GuestRatingDTO> GuestRatingsObservable { get; set; }
        public RateGuestsWindow(List<GuestRating>guestRatings,List<AccommodationReservation>reservations)
        {
            unratedGuestService = new UnratedGuestService(guestRatings,reservations);
            GuestRatingsObservable = new ObservableCollection<GuestRatingDTO>();
            InitializeComponent();
            DataContext = this;
            Update();
        }
        private void Update()
        {
            unratedGuestService.GetUnratedGuests()
                .ForEach(guestRatingDTO => GuestRatingsObservable.Add(guestRatingDTO));
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void RateGuestButton_Click(object sender, RoutedEventArgs e)
        {
            GiveRateWindow giveRateWindow = new GiveRateWindow(SelectedGuestRatingDTO);
            giveRateWindow.Owner = this;
            giveRateWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            giveRateWindow.ShowDialog();

        }
    }
}
