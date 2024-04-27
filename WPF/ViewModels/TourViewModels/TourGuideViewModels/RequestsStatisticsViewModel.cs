using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.WPF.Commands;
using BookingApp.WPF.Views.TouristGuide;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels.TourViewModels.TourGuideViewModels
{
    public class RequestsStatisticsViewModel : INotifyPropertyChanged
    {
        public User LoggedInUser { get; set; }
        public ICommand ComboBoxSelectionChangedCommand { get; set; }
        public RelayCommand HelpCommand { get; set; }
        public RelayCommand BackCommand { get; set; }
        public RelayCommand LocationComboBoxGotFocusCommand { get; }
        public RelayCommand LanguageComboBoxGotFocusCommand { get; }
        public TourRequestService tourRequestService { get; set; }
        public TourRealisationService tourRealisationService { get; set; }
        public TourReservationService tourReservationService { get; set; }


        public ObservableCollection<string> Years { get; set; }
        public ObservableCollection<string> YearLabels { get; set; }
        public ObservableCollection<string> MonthLabels { get; set; }

        private int _pickedLocationId = 10;
        private int _pickedLanguage = 3;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public int PickedLocationId
        {
            get => _pickedLocationId;
            set
            {
                _pickedLocationId = value;
                OnPropertyChanged(nameof(PickedLocationId));
            }
        }

        public int PickedLanguage
        {
            get => _pickedLanguage;
            set
            {
                _pickedLanguage = value;
                OnPropertyChanged(nameof(PickedLanguage));
            }
        }
        public RequestsStatisticsViewModel(User user) 
        {
            LoggedInUser = user;
            tourReservationService = new TourReservationService();
            HelpCommand = new RelayCommand(HelpButton_Click);
            BackCommand = new RelayCommand(Back);
            LocationComboBoxGotFocusCommand = new RelayCommand(LocationComboBoxGotFocus);
            LanguageComboBoxGotFocusCommand = new RelayCommand(LanguageComboBoxGotFocus);
            tourRequestService = new TourRequestService();
            tourRealisationService = new TourRealisationService();
            Years = new ObservableCollection<string>();
            ComboBoxSelectionChangedCommand = new RelayParameterCommand(OnComboBoxSelectionChanged);
            InitializeComboBox();
            OnComboBoxSelectionChanged("All Time");
        }
        private void OnComboBoxSelectionChanged(object parameter)
        {/*
            Tour.Clear();
            string selectedValue = parameter as string;
            if (selectedValue != null)
            {
                if (selectedValue == "All Time")
                {

                    Tour tour = tourService.GetBestTourOfAllTime();
                    Tour.Add(new TourViewModel(tour.Id, tour.Name, tour.Description, locationService.GetById(tour.Location.Id), tour.Duration, tour.ImagesPath, tour.MaxCapacity, tour.Language, tour.User));
                }
                else
                {
                    if (int.TryParse(selectedValue, out int selectedYear))
                    {
                        Tour tour = tourService.GetBestTourInAYear(selectedYear);
                        if (tour == null) { return; }
                        Tour.Add(new TourViewModel(tour.Id, tour.Name, tour.Description, locationService.GetById(tour.Location.Id), tour.Duration, tour.ImagesPath, tour.MaxCapacity, tour.Language, tour.User));
                    }
                }
            }*/
        }
        private void HelpButton_Click()
        {
            if (RequestsStatistics.HelpPopUp.IsOpen)
                RequestsStatistics.HelpPopUp.IsOpen = false;
            else
                RequestsStatistics.HelpPopUp.IsOpen = true;
        }
        private void Back()
        {
            SideBar.contentControlW.Content = new RequestsWindow(LoggedInUser);
        }
        private void LocationComboBoxGotFocus()
        {
            PickedLanguage = 3;
        }

        private void LanguageComboBoxGotFocus()
        {
            PickedLocationId = 10;
        }
        
        private void InitializeComboBox()
        {
            TourRequest first = tourRequestService.GetFirstTourRequest();
            TourRequest last = tourRequestService.GetLastTourRequest();
            Years.Add("All Time");
            for (int i = tourRealisationService.GetById(tourReservationService.GetById(last.TourReservationId).TourRealisationId).StartTime.Year; i >= tourRealisationService.GetById(tourReservationService.GetById(first.TourReservationId).TourRealisationId).StartTime.Year; i--)
                Years.Add(i.ToString());
        }
    }
}
