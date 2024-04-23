using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.WPF.Commands;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    public class AccommodationStatsViewModel
    {
        public ICommand ChangeStatsCommand { get; set; }
        private User loggedInUser;
        public SeriesCollection YearlyBussinessStats { get; set; }
        public SeriesCollection YearlyGeneralStats { get; set; }
        public ObservableCollection<string>Years { get; set; }
        public string SelectedYear { get; set; }
        public ObservableCollection<string> YearLabels { get; set; }
        public SeriesCollection YearlyReccommendedrenovations { get; set; }
        private AccommodationStatsService accommodationStatsService;
        private AccommodationService accommodationService;
        private Accommodation accommodation;
        private AccommodationReservationService accommodationReservationService;
        public AccommodationStatsViewModel(int accommodationId, User user)
        {
            loggedInUser = user;
            accommodationService = new AccommodationService();
            accommodationStatsService = new AccommodationStatsService();
            accommodationReservationService = new AccommodationReservationService();

            YearLabels = new ObservableCollection<string>();
            YearlyBussinessStats = new SeriesCollection();
            YearlyGeneralStats = new SeriesCollection();
            Years = new ObservableCollection<string>();
            YearlyReccommendedrenovations = new SeriesCollection();

            ChangeStatsCommand = new RelayCommand(ChangeStats);

            accommodation = accommodationService.GetById(accommodationId);
            SelectedYear = "All years";
            InitializeComboBox(accommodation);
            ChangeStats();

        }
        public void ChangeStats()
        {
            YearlyBussinessStats.Clear();
            YearlyGeneralStats.Clear();
            YearlyReccommendedrenovations.Clear();
            YearLabels.Clear();
            if (SelectedYear == "All years")
                ShowYearlyStats(SelectedYear);
            else
                ShowYearlyStats(SelectedYear);
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
            accommodationStatsService.GetAccommodationStats(selectedYear, accommodation).ForEach(stat => {
                Debug.WriteLine(stat.NumberOfReservations);
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

                YearlyGeneralStats.Add(columnSeriesMoved);
                YearlyGeneralStats.Add(columnSeriesFinished);
                YearlyGeneralStats.Add(columnSeriesCanceled);
                YearLabels.Add(stat.Year);
                var values = new ChartValues<double> { 0, stat.NumberOfRecommendedRenovations };
                var lineSeries = new LineSeries
                {
                    Title = $"Renovations in {stat.Year}",
                    Values = values,
                };
                YearlyReccommendedrenovations.Add(lineSeries);
            });
        }
    }
}
