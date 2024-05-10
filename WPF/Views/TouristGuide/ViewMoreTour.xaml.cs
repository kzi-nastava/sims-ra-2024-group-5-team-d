using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using BookingApp.WPF.ViewModels;
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
            tourReservations.ForEach(tourReservation => voucher = voucherService.Save(new Voucher(voucherService.NextId(), DateTime.Now.AddYears(1), VOUCHERTYPE.CANCELEDTOUR, tourReservation.User)));
            tourReservations.ForEach(tourReservation => tourReservationService.DeleteTourReservation(tourReservation));
            tourRealisationService.DeleteTourRealisationById(SelectedTourRealisation.Id);
            TourRealisations.Remove(SelectedTourRealisation);
            notificationService.SendVoucherNotification(voucher.User.Id,voucher.Id);   
        }
    }
}
