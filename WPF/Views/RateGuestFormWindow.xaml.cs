using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using BookingApp.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace BookingApp.WPF.Views
{
    /// <summary>
    /// Interaction logic for RateGuestFormWindow.xaml
    /// </summary>
    public partial class RateGuestFormWindow : Window
    {
        private IAccommodationReservationRepository accommodationReservationRepository;
        private IGuestRatingRepository guestRatingRepository;
        private UnratedGuestViewModel selectedGuest;
        public RateGuestFormWindow(UnratedGuestViewModel selectedGuest)
        {
            
            this.selectedGuest = selectedGuest;
            guestRatingRepository = Injector.CreateInstance<IGuestRatingRepository>();
            accommodationReservationRepository = Injector.CreateInstance<IAccommodationReservationRepository>();
            DataContext = new RateGuestViewModel(selectedGuest.Id, selectedGuest.Name);
            InitializeComponent();
        }
        private void Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void RateGuest_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as RateGuestViewModel;
            guestRatingRepository.Save(new GuestRating(accommodationReservationRepository.GetById(viewModel.Id).AccommodationId, accommodationReservationRepository.GetById(viewModel.Id).UserId, viewModel.Id, viewModel.Cleanliness, viewModel.RuleCompliance, viewModel.Comment));
            UnratedGuestsUserControl.UnratedGuests.Remove(selectedGuest);
            Close();
        }

        private void Border_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            var viewModel = DataContext as RateGuestViewModel;
            viewModel.Cleanliness = 1;
        }
        private void Border_MouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {
            var viewModel = DataContext as RateGuestViewModel;
            viewModel.Cleanliness = 2;
        }
        private void Border_MouseLeftButtonDown_3(object sender, MouseButtonEventArgs e)
        {
            var viewModel = DataContext as RateGuestViewModel;
            viewModel.Cleanliness = 3;
        }
        private void Border_MouseLeftButtonDown_4(object sender, MouseButtonEventArgs e)
        {
            var viewModel = DataContext as RateGuestViewModel;
            viewModel.Cleanliness = 4; 
        }
        private void Border_MouseLeftButtonDown_5(object sender, MouseButtonEventArgs e)
        {
            var viewModel = DataContext as RateGuestViewModel;
            viewModel.Cleanliness = 5;
        }
        private void Border_MouseLeftButtonDown_6(object sender, MouseButtonEventArgs e)
        {
            var viewModel = DataContext as RateGuestViewModel;
            viewModel.RuleCompliance = 1;
        }
        private void Border_MouseLeftButtonDown_7(object sender, MouseButtonEventArgs e)
        {
            var viewModel = DataContext as RateGuestViewModel;
            viewModel.RuleCompliance = 2;
        }
        private void Border_MouseLeftButtonDown_8(object sender, MouseButtonEventArgs e)
        {
            var viewModel = DataContext as RateGuestViewModel;
            viewModel.RuleCompliance = 3;
        }
        private void Border_MouseLeftButtonDown_9(object sender, MouseButtonEventArgs e)
        {
            var viewModel = DataContext as RateGuestViewModel;
            viewModel.RuleCompliance = 4;
        }
        private void Border_MouseLeftButtonDown_10(object sender, MouseButtonEventArgs e)
        {
            var viewModel = DataContext as RateGuestViewModel;
            viewModel.RuleCompliance = 5;
        }
    }
}
