using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.Commands;
using BookingApp.WPF.Views.TouristView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Runtime.CompilerServices;

namespace BookingApp.WPF.ViewModels
{
    public class LiveTourViewModel
    {
        User User { get; set; }
        public ICommand PastToursTabCommand { get; private set; }

        public ICommand VouchersTabCommand { get; private set; }

        public List<CheckPoint> CheckPoints { get; set; }
        public TourReservation Reservation { get; set; }
        public Tour Tour { get; set; }
        public ObservableCollection<TourAttendeeViewModel> Attendees { get; set; }
        private TourRealisationService realisationService;
        private TourReservationService reservationService;
        private TourGuestService guestService;
        private TourService tourService;
        private CheckPointService checkPointService;

        public bool IsTourLive { get; set; }
        public LiveTourViewModel(User user) 
        {
            User = user;

            guestService = new TourGuestService();
            checkPointService = new CheckPointService();
            tourService = new TourService();
            reservationService = new TourReservationService();
            realisationService = new TourRealisationService();

            Attendees = new ObservableCollection<TourAttendeeViewModel>();
            CheckPoints = new List<CheckPoint>();

            PastToursTabCommand = new RelayCommand(SwitchToPastTours);
            VouchersTabCommand = new RelayCommand(SwitchToVouchers);

            Reservation = reservationService.GetLiveTourReservation(user.Id);
            if(Reservation != null )
            {
                IsTourLive = true;
                
                foreach (TourGuest guest in guestService.GetAllTourGuests())
                {
                    if(guest.TourReservationId == Reservation.Id)
                    {
                        TourAttendeeViewModel attendee = new TourAttendeeViewModel(guest.Id, guest.FullName);
                        if(guest.CheckPointId == -1)
                        {
                            attendee.IsPresent = false;
                            attendee.CheckpointName = "";
                        }
                        else
                        {
                            attendee.IsPresent = true;
                            attendee.CheckpointName = checkPointService.GetById(guest.CheckPointId).Name;
                        }
                        Attendees.Add(attendee);
                    }
                }               

                Tour = tourService.GetById(realisationService.GetTourRealisationById(Reservation.TourRealisationId).TourId);
                foreach (CheckPoint cp in checkPointService.GetAllCheckPointsByTourId(Tour.Id))
                {
                    CheckPoints.Add(cp);
                }
                CreateProgressSteps();
            }

        }

        public void SwitchToPastTours()
        {
            TouristHomeWindow.contentControl.Content = new PastToursUserControl(User);
        }

        public void SwitchToVouchers()
        {
            TouristHomeWindow.contentControl.Content = new VouchersUserControl(User);
        }

        private void CreateProgressSteps()
        {
            int numberOfSteps = CheckPoints.Count;
            double stepValue = YourToursUserControl.progressBar.Maximum / (double)(numberOfSteps - 1);
            double stepWidth = (YourToursUserControl.progressBar.Width - (numberOfSteps - 1) * 40) / (double)(numberOfSteps - 1);
            double textBoxStep = (YourToursUserControl.progressBar.Width - (numberOfSteps - 1) * 135) / (double)(numberOfSteps - 1);

            for (int i = 0; i < numberOfSteps; i++)
            {
                Ellipse ellipse = new Ellipse
                {
                    Width = 40,
                    Height = 40,
                    Fill = CheckPoints[i].IsChecked ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2FC8BF")) : Brushes.AntiqueWhite,
                    Margin = new Thickness(i == 0 ? 20 : stepWidth, 5, 0, 0),
                    Cursor = Cursors.Hand
                };

                if (CheckPoints[i].IsChecked)
                    YourToursUserControl.progressBar.Value = i * stepValue;

                TextBox textbox = new TextBox
                {
                    Text = CheckPoints[i].Name,
                    BorderBrush = Brushes.Transparent,
                    IsReadOnly = true,
                    Width = 135,
                    Height = 40,
                    Background = Brushes.Transparent,
                    FontSize = 12,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    FontWeight = FontWeights.DemiBold,
                    FontStyle = FontStyles.Normal,
                    FontFamily = new FontFamily("Segoe UI"),
                    Margin = new Thickness(i == 0 ? -25 : textBoxStep, 45, 0, 10)
                };

                YourToursUserControl.namesContainer.Children.Add(textbox);
                YourToursUserControl.progressContainer.Children.Add(ellipse);
            }
        }
    }
}
