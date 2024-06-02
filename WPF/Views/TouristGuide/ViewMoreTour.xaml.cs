using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using BookingApp.WPF.ViewModels;
using Svg;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

namespace BookingApp.WPF.Views.TouristGuide
{
    /// <summary>
    /// Interaction logic for ViewMoreTour.xaml
    /// </summary>
    public partial class ViewMoreTour : UserControl
    {
        public DateTime SelectedDateTime { get; set; }  
        public TourViewModel SelectedTour { get; set; }
        public TourRealisationViewModel SelectedTourRealisation { get; set; }
        public ObservableCollection<TourRealisationViewModel> TourRealisations { get; set; }
        private TourRealisationService tourRealisationService;
        private TourReservationService tourReservationService;
        private VoucherService voucherService;
        private NotificationsService notificationService;
        private User LoggedInUser { get; set; }


        public ViewMoreTour(TourViewModel selectedTour,User user)
        {
            InitializeComponent();
            DataContext = this;
            tourRealisationService = new TourRealisationService();
            TourRealisations = new ObservableCollection<TourRealisationViewModel>();
            SelectedTour = selectedTour;
            tourRealisationService.GetTourRealisationsByTourId(SelectedTour.Id).ForEach(t => { TourRealisations.Add(new TourRealisationViewModel(t.Id, t.StartTime, t.TourId, t.AvailableSeats,t.IsCancellable(), t.User, t.IsFinished)); });
            tourReservationService = new TourReservationService();
            notificationService = new NotificationsService();
            LoggedInUser = user;
            SelectedDateTime = DateTime.Now;
            voucherService = new VoucherService();
        }

        private void AddTourRealizationButton_Click(object sender, RoutedEventArgs e)
        {
            NewTourRealizationPopup.IsOpen = true;
        }
        private void SaveNewTourRealization_Click(object sender, RoutedEventArgs e)
        {
            DateTime selectedDateTime = SelectedDateTime;
            TourRealisation tourRealisation = new TourRealisation(selectedDateTime,SelectedTour.Id,SelectedTour.Capacity, LoggedInUser);
            tourRealisationService.Save(tourRealisation);
            TourRealisationViewModel tourRealisationViewModel = new TourRealisationViewModel(tourRealisation.Id,selectedDateTime, SelectedTour.Id, SelectedTour.Capacity, tourRealisation.IsCancellable(), LoggedInUser,tourRealisation.IsFinished);
            TourRealisations.Add(tourRealisationViewModel);
            NewTourRealizationPopup.IsOpen = false;
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            SideBar.contentControlW.Content = new AllToursWindow(LoggedInUser);
        }
        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            if (HelpPopup.IsOpen)
                HelpPopup.IsOpen = false;
            else
                HelpPopup.IsOpen = true;
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Voucher voucher = new Voucher();
            List<TourReservation> tourReservations = tourReservationService.GetAll().Where(tourReservation => tourReservation.TourRealisationId == SelectedTourRealisation.Id).ToList();
            tourReservations.ForEach(tourReservation => voucher = voucherService.Save(new Voucher(voucherService.NextId(), DateTime.Now.AddYears(1), VOUCHERTYPE.CANCELEDTOUR, tourReservation.User, LoggedInUser)));
            tourReservations.ForEach(tourReservation => tourReservationService.DeleteTourReservation(tourReservation));
            tourRealisationService.DeleteTourRealisationById(SelectedTourRealisation.Id);
            TourRealisations.Remove(SelectedTourRealisation);
            notificationService.SendVoucherNotification(voucher.User.Id,voucher.Id);   
        }
        private async void StartDemo(object sender, RoutedEventArgs e)
        {
            await Task.Delay(500);
            PlusButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#218C89"));
            await Task.Delay(500);
            PlusButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00FFFFFF"));
            // Highlight the DateTimePicker and show the popup
            NewTourRealizationPopup.IsOpen = true;

            await Task.Delay(500);

            // Open the DateTimePicker dropdown and select a date
            DateTimePicker.IsOpen = true;
            await Task.Delay(1000);
            DateTimePicker.Value = DateTime.Now.AddDays(1);
            await Task.Delay(1000);
            DateTimePicker.IsOpen = false;
            SaveButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#218C89"));
            await Task.Delay(1000);
            SaveButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2FC8BF"));
            await Task.Delay(1000);
            TourRealisation tourRealisation = new TourRealisation(DateTime.Now.AddDays(1), SelectedTour.Id, SelectedTour.Capacity, LoggedInUser);
            TourRealisations.Add(new TourRealisationViewModel(tourRealisation.Id, DateTime.Now.AddDays(1), SelectedTour.Id, SelectedTour.Capacity, tourRealisation.IsCancellable(), LoggedInUser, tourRealisation.IsFinished));
            NewTourRealizationPopup.IsOpen = false;
            await Task.Delay(1000);
            System.Windows.MessageBox.Show("End of demo", "Demonstration");
            SideBar.contentControlW.Content = new ViewMoreTour(SelectedTour, LoggedInUser);
        }
    }
}
