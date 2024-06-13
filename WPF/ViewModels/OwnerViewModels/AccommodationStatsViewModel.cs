using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.Commands;
using BookingApp.WPF.Views.OwnerView;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    public class AccommodationStatsViewModel :INotifyPropertyChanged
    {
        public ICommand ShowReccommendationCommand { get; set; }
        public ICommand ChangeStatsCommand { get; set; }


        private AccommodationStatsService accommodationStatsService;
        private AccommodationService accommodationService;
        private AccommodationReservationService accommodationReservationService;


        private User loggedInUser;
        public SeriesCollection YearlyBussinessStats { get; set; }
        public SeriesCollection YearlyGeneralStats { get; set; }
        public ObservableCollection<string>Years { get; set; }
        public string SelectedYear { get; set; }
        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title)));
            }
        }
        public ObservableCollection<string> YearLabels { get; set; }
        public SeriesCollection YearlyReccommendedrenovations { get; set; }

        private Accommodation accommodation;

        public event PropertyChangedEventHandler? PropertyChanged;

        public AccommodationStatsViewModel(int accommodationId, User user)
        {
            InitializeServices();
            loggedInUser = user;

            YearLabels = new ObservableCollection<string>();
            YearlyBussinessStats = new SeriesCollection();
            YearlyGeneralStats = new SeriesCollection();
            Years = new ObservableCollection<string>();
            YearlyReccommendedrenovations = new SeriesCollection();

            ShowReccommendationCommand = new RelayCommand(ShowReccommendation);
            ChangeStatsCommand = new RelayCommand(ChangeStats);

            accommodation = accommodationService.GetById(accommodationId);
            SelectedYear = "All years";
            if (accommodationReservationService.GetByAccommodation(accommodation).Count == 0)
                Title = "No stats available";
            else
            {
                InitializeComboBox(accommodation);
                ChangeStats();

            }
        }
        private void InitializeServices()
        {
            UserService userService = new UserService(Injector.CreateInstance<IUserRepository>());
            accommodationService = new AccommodationService(Injector.CreateInstance<IAccommodationRepository>(),userService);
            accommodationReservationService = new AccommodationReservationService(Injector.CreateInstance<IAccommodationReservationRepository>());
            accommodationStatsService = new AccommodationStatsService(accommodationReservationService);
        }

        public void ChangeStats()
        {
            if (accommodationReservationService.GetByAccommodation(accommodation).Count == 0)
                return;
            YearlyBussinessStats.Clear();
            YearlyGeneralStats.Clear();
            YearlyReccommendedrenovations.Clear();
            YearLabels.Clear();
            if (SelectedYear == "All years")
            {
                Title = "All time stats";
                ShowYearlyStats(SelectedYear);
            }
            else
            {
                Title = $"Stats for {SelectedYear}";
                ShowYearlyStats(SelectedYear);
            }
        }
        private void InitializeComboBox(Accommodation accommodation)
        {
            List<AccommodationReservation> reservations = new List<AccommodationReservation>(accommodationReservationService.GetByAccommodation(accommodation));
            Years.Add("All years");
            accommodationStatsService.SortReservations(reservations);
            int LastBusyYear = reservations[reservations.Count - 1].ReservedTo.Year;
            int FirstBusyYear = reservations[0].ReservedFrom.Year;
            for (int i = LastBusyYear; i >= FirstBusyYear; i--)
                Years.Add(i.ToString());
        }
        private void ShowYearlyStats(string selectedYear)
        {
                List<AccommodationStat> sortedStats = GetSortedStats(selectedYear);
                 sortedStats.ForEach(stat => {
                YearlyBussinessStats.Add(new PieSeries
                {
                    Title = stat.Year,
                    Values = new ChartValues<ObservableValue> { new ObservableValue(stat.Busyness) },
                    DataLabels = true
                });


                var movedReservations = new ChartValues<double> { stat.NumberOfRescheduledReservations };
                var finishedReservations = new ChartValues<double> { stat.NumberOfReservations };
                var canceledReservations = new ChartValues<double> { stat.NumberOfCancelledReservations };

                var columnSeriesMoved = new ColumnSeries
                {
                    Title ="Moved",
                    Values = movedReservations,
                    Fill = Brushes.Green
                };

                var columnSeriesFinished = new ColumnSeries
                {
                    Title = "Finished",
                    Values = finishedReservations,
                    Fill = Brushes.Blue
                };

                var columnSeriesCanceled = new ColumnSeries
                {
                    Title = "Canceled",
                    Values = canceledReservations,
                    Fill = Brushes.Red
                };
                if (YearlyGeneralStats.Count == 0)
                {
                    YearlyGeneralStats.Add(columnSeriesMoved);
                    YearlyGeneralStats.Add(columnSeriesFinished);
                    YearlyGeneralStats.Add(columnSeriesCanceled);
                }
                else
                {
                    YearlyGeneralStats[0].Values.Add((double)stat.NumberOfRescheduledReservations);
                    YearlyGeneralStats[1].Values.Add((double)stat.NumberOfReservations);
                    YearlyGeneralStats[2].Values.Add((double)stat.NumberOfCancelledReservations);
                }


                YearLabels.Add(stat.Year);

                     if (YearlyReccommendedrenovations.Count == 0)
                     {
                         var values = new ChartValues<double> { 0, stat.NumberOfRecommendedRenovations };
                         var lineSeries = new LineSeries
                         {
                             Title = $"Renovations in {stat.Year}",
                             Values = values,
                         };
                         YearlyReccommendedrenovations.Add(lineSeries);
                     }
                     else {
                         YearlyReccommendedrenovations[0].Values.Add((double)stat.NumberOfRecommendedRenovations);
                     }
               
            });
        }
        private List<AccommodationStat> GetSortedStats(string selectedYear)
        {
            List<AccommodationStat> accommodationStats = accommodationStatsService.GetAccommodationStats(selectedYear, accommodation);
            if (selectedYear == "All years")
            {
                Comparison<AccommodationStat> comparison = (stat1, stat2) => stat1.Year.CompareTo(stat2.Year);
                accommodationStats.Sort(comparison);
            }
           
            return accommodationStats;
        }
        public void ShowReccommendation()
        {
            ReccommendationsWindow reccommendationsWindow = new ReccommendationsWindow();
            reccommendationsWindow.Show();
            reccommendationsWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }
    }
}
