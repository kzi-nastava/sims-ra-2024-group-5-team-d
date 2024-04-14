using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using BookingApp.WPF.Views.TouristGuide;
using ExCSS;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;
using BookingApp.WPF.Commands;

namespace BookingApp.WPF.ViewModels.TourViewModels.TourGuideViewModels
{
    public class TourLiveViewModel : INotifyPropertyChanged
    {
        public ICommand BackCommand { get; set; }
        public ICommand FinishTourCommand { get; set; }
        public ICommand CheckClickCommand { get; set; }
        public User LoggedInUser { get; set; }
        public TourRealisationViewModel tourRealisation { get; set; }
        public TourViewModel tour { get; set; }
        public ObservableCollection<TourGuestViewModel> TourGuests { get; set; }
        public ObservableCollection<CheckPointViewModel> CheckPoints { get; set; }
        public TourGuestService tourGuestService { get; set; }

        public TourRealisationService tourRealisationService;
        public CheckPointService checkPointService;

        public TourLiveViewModel(User user, TourRealisationViewModel tourRealisationViewModel, TourViewModel tourViewModel)
        {
            LoggedInUser = user;
            BackCommand = new RelayCommand(BackButton_Click);
            FinishTourCommand = new RelayCommand(FinishTourButton_Click);
            CheckClickCommand = new RelayCommand(Check_Click);
            tourRealisation = tourRealisationViewModel;
            tour = tourViewModel;
            CheckPoints = new ObservableCollection<CheckPointViewModel>();
            TourGuests = new ObservableCollection<TourGuestViewModel>();
            tourGuestService = new TourGuestService();
            checkPointService = new CheckPointService();
            tourGuestService.GetTourGuestsOnTourRealisation(tourRealisation.Id).ForEach(t => TourGuests.Add(new TourGuestViewModel(t.Id, t.FullName, t.Years, t.TourReservationId, t.CheckPointId)));
            checkPointService.GetAllCheckPointsByTourId(tour.Id).ForEach(cp => CheckPoints.Add(new CheckPointViewModel(cp.Id, cp.Name, cp.TourId, cp.IsChecked)));
            CheckPoints[0].IsChecked = true;
            CheckPoint cp = checkPointService.GetById(CheckPoints[0].Id);
            cp.IsChecked = true;
            checkPointService.Update(cp);
        }

        private CheckPointViewModel selectedCheckPoint;
        public CheckPointViewModel SelectedCheckPoint
        {
            get => selectedCheckPoint;
            set
            {
                if (value != selectedCheckPoint)
                {
                    selectedCheckPoint = value;
                    OnPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private void BackButton_Click()
        {
            SideBar.contentControlW.Content = new ViewMoreTourToday(tour, LoggedInUser);
        }


        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedCheckPoint = (CheckPointViewModel)LiveTourView.CheckPointsListView.SelectedItem;

            if (selectedCheckPoint == null)
            {
                MessageBox.Show("Please select a CheckPoint.");
                return;
            }

            var selectedTourGuests = LiveTourView.TourGuestsListView.SelectedItems.Cast<TourGuestViewModel>().ToList();

            selectedTourGuests.ForEach(tourGuest =>
            {
                UpdateTourGuestCheckpoint(tourGuest, selectedCheckPoint.Id);
                RemoveTourGuestFromList(tourGuest);
            });
        }
        public void FinishTourButton_Click()
        {
            TourRealisation realisation = tourRealisationService.GetTourRealisationById(tourRealisation.Id);
            realisation.IsFinished = true;
            tourRealisationService.Update(realisation);
            var allCheckPoints = checkPointService.GetAll();
            foreach (var cp in allCheckPoints)
            {
                cp.IsChecked = false;
                checkPointService.Update(cp);
            }
            CheckPoints.All(cp => cp.IsChecked = false);
            BackButton_Click();
        }
        private void UpdateTourGuestCheckpoint(TourGuestViewModel tourGuest, int checkPointId)
        {
            tourGuest.CheckPointId = checkPointId;

            TourGuest guest = new TourGuest(tourGuest.Id, tourGuest.FullName, tourGuest.Years, tourGuest.TourReservationId, tourGuest.CheckPointId);
            tourGuestService.Update(guest);
        }

        private void RemoveTourGuestFromList(TourGuestViewModel tourGuest)
        {
            TourGuests.Remove(tourGuest);
        }

        public void Check_Click()
        {
            if (SelectedCheckPoint != null)
            {
                SelectedCheckPoint.IsChecked = true;

                CheckPoint cp = checkPointService.GetById(SelectedCheckPoint.Id);
                cp.IsChecked = true;
                checkPointService.Update(cp);

                if (CheckPoints.All(cp => cp.IsChecked))
                {
                    FinishTourButton_Click();
                }
            }
        }

    }
}
