using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.WPF.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace BookingApp.WPF.Views
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
