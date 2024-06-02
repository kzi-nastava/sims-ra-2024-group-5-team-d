using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.WPF.Commands;
using BookingApp.WPF.Views.TouristGuide;
using HarfBuzzSharp;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace BookingApp.WPF.ViewModels.TourViewModels.TourGuideViewModels
{
    public class RequestsStatisticsViewModel : INotifyPropertyChanged
    {
        public User LoggedInUser { get; set; }
        public ICommand ComboBoxSelectionChangedCommand { get; set; }
        public ICommand LanguageSelectedCommand { get; set; }
        public ICommand LocationSelectedCommand { get; set; }
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

        public SeriesCollection YearlyStats { get; set; }
        public SeriesCollection MonthlyStats { get; set; }

        private int _pickedLocationId = 10;
        private int _pickedLanguage = 3;
        public bool isMonthly { get; set; }
        public bool isYearly { get; set; }

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

        public bool IsMonthly
        {
            get => isMonthly;
            set
            {
                isMonthly = value;
                OnPropertyChanged(nameof(IsMonthly));
            }
        }
        public bool IsYearly
        {
            get => isYearly;
            set
            {
                isYearly = value;
                OnPropertyChanged(nameof(IsYearly));
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
            LanguageSelectedCommand = new RelayCommand(LanguageSelection);
            LocationSelectedCommand = new RelayCommand(LocationSelection);
            LocationComboBoxGotFocusCommand = new RelayCommand(LocationComboBoxGotFocus);
            LanguageComboBoxGotFocusCommand = new RelayCommand(LanguageComboBoxGotFocus);
            tourRequestService = new TourRequestService();
            tourRealisationService = new TourRealisationService();
            Years = new ObservableCollection<string>();
            MonthlyStats = new SeriesCollection();
            YearlyStats = new SeriesCollection();
            YearLabels = new ObservableCollection<string>();
            MonthLabels = new ObservableCollection<string>();
            ComboBoxSelectionChangedCommand = new RelayParameterCommand(OnComboBoxSelectionChanged);
            InitializeComboBox();
            OnComboBoxSelectionChanged("All Time");
        }
        private void OnComboBoxSelectionChanged(object parameter)
        {
            string selectedValue = parameter as string;
            if (selectedValue != null)
            {
                if (selectedValue == "All Time")
                {
                    IsYearly = true;
                    IsMonthly = false;
                    InitializeYearlyStats();
                }
                else
                {
                    if (int.TryParse(selectedValue, out int selectedYear))
                    {
                        IsYearly = false;
                        IsMonthly = true;
                        InitializeMonthlyStats(selectedYear);
                    }
                }
            }
        }
        private void LanguageSelection()
        {
            var language = tourRequestService.GetMostWantedLanguage();
            SideBar.contentControlW.Content = new CreateNewTourForm(LoggedInUser,-1,Convert.ToInt32(language));
        }
        private void LocationSelection()
        {
            var location = tourRequestService.GetMostWantedLocation();

            Debug.WriteLine("MIKIKIKIKLIKILILI" + location.Id + "HASAHA");
            SideBar.contentControlW.Content = new CreateNewTourForm(LoggedInUser,location.Id,-1);
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
            Years.Add("All Time");

            var uniqueYears = tourRequestService.GetAll()
                .SelectMany(request => new[] { request.RangeFrom.Year, request.RangeTo.Year })
                .Distinct();

            foreach (var year in uniqueYears)
            {
                Years.Add(year.ToString());
                YearLabels.Add(year.ToString());
            }
        }
        private void InitializeYearlyStats()
        {
            YearlyStats.Clear();
            YearlyStats.Add(new ColumnSeries
            {
                Title = "Years",
                Values = new ChartValues<int>(),
                Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6A9A"))
            });
            for(int i=0; i < Years.Count -1 ; i++) 
            {
                int counter = tourRequestService.GetRequestsInAYear(Convert.ToInt32(Years[i+1]),PickedLanguage,PickedLocationId, LoggedInUser);
                YearlyStats[0].Values.Add(counter);
            }
        }
        private void InitializeMonthlyStats(int year)
        {
            MonthlyStats.Clear();
            MonthlyStats.Add(new ColumnSeries
            {
                Title = "Months",
                Values = new ChartValues<int> (),
                Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A375E1"))
            });
            Dictionary<int,int> dictionary = tourRequestService.GetRequestsInAYearByMonths(year, PickedLanguage, PickedLocationId,LoggedInUser);
            for (int i = 1; i <= dictionary.Count; i++)
            {
                MonthlyStats[0].Values.Add(dictionary[i]);
                MonthLabels.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(i));
            }
        }
    }
}
