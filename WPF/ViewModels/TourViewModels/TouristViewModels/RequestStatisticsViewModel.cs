using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.WPF.Commands;
using HarfBuzzSharp;
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

namespace BookingApp.WPF.ViewModels.TourViewModels.TouristViewModels
{
    
    public class RequestStatisticsViewModel
    {
        public ICommand ComboBoxSelectionChangedCommand { get; set; }   
        public User Tourist { get; set; }
        public SeriesCollection RequestsStatistics { get; set; }
        public SeriesCollection LanguageStats { get; set; }
        public int AcceptedRequests { get; set; }
        public int NotAcceptedRequests { get; set; }
        public TourRequestService tourRequestService { get; set; }
        public ObservableCollection<string> LanguageLabels { get; set; }
        public ObservableCollection<string> Years { get; set; }
        public int SelectedYear { get; set; }
        public RequestStatisticsViewModel(User tourist) 
        {
            Tourist = tourist;
            LanguageStats = new SeriesCollection();
            tourRequestService = new TourRequestService();
            LanguageLabels = new ObservableCollection<string>();
            for (int i = 0; i < Enum.GetNames(typeof(LANGUAGE)).Length; i++)
            {
                LanguageLabels.Add(Enum.GetNames(typeof(LANGUAGE))[i]);
            }

            RequestsStatistics = new SeriesCollection();

            ComboBoxSelectionChangedCommand = new RelayParameterCommand(OnComboBoxSelectionChanged);

            InitializeYears();


            OnComboBoxSelectionChanged("All Time");

            
        }


        public void OnComboBoxSelectionChanged(object parameter)
        {
            
            string selectedYear = parameter as string;

            if (selectedYear != null)
            {
                if(selectedYear != "All Time")
                {
                    if(int.TryParse(selectedYear, out int SelectedYear))
                    {
                        InitializeRequestsStatistics(SelectedYear);
                        InitializeLanguageStatistics(SelectedYear);
                    }
                }
                else
                {
                    InitializeRequestsStatistics();
                    InitializeLanguageStatistics();
                }
            }
        }

        public void InitializeRequestsStatistics()
        {
            AcceptedRequests = tourRequestService.GetAll().Count(request => request.Status == STATE.ACCEPTED);
            NotAcceptedRequests = tourRequestService.GetAll().Count(request => request.Status != STATE.ACCEPTED);

            RequestsStatistics.Clear();
            RequestsStatistics.Add(new PieSeries
            {
                Title = "Accepted",
                Values = new ChartValues<ObservableValue> { new ObservableValue(AcceptedRequests) },
                DataLabels = true
            });
            RequestsStatistics.Add(new PieSeries
            {
                Title = "Not Accepted",
                Values = new ChartValues<ObservableValue> { new ObservableValue(NotAcceptedRequests) },
                DataLabels = true
            });
        }


        public void InitializeLanguageStatistics()
        {
            LanguageStats.Clear();
            LanguageStats.Add(new ColumnSeries
            {
                Title = "Languages",
                Values = new ChartValues<int>(),
                Fill = Brushes.Green
            });
            for (int i = 0; i < LanguageLabels.Count; i++)
            {
                int counter = tourRequestService.GetAll().Where(x => x.Language == (LANGUAGE)Enum.Parse(typeof(LANGUAGE), LanguageLabels[i])).Count();
                LanguageStats[0].Values.Add(counter);
            }
        }

        public void InitializeRequestsStatistics(int year)
        {
            AcceptedRequests = tourRequestService.GetAll().Count(request => request.Status == STATE.ACCEPTED && request.RangeFrom.Year == year);
            NotAcceptedRequests = tourRequestService.GetAll().Count(request => request.Status != STATE.ACCEPTED && request.RangeFrom.Year == year);

            RequestsStatistics.Clear();
            RequestsStatistics.Add(new PieSeries
            {
                Title = "Accepted",
                Values = new ChartValues<ObservableValue> { new ObservableValue(AcceptedRequests) },
                DataLabels = true
            });
            RequestsStatistics.Add(new PieSeries
            {
                Title = "Not Accepted",
                Values = new ChartValues<ObservableValue> { new ObservableValue(NotAcceptedRequests) },
                DataLabels = true
            });
        }

        public void InitializeLanguageStatistics(int year)
        {
            LanguageStats.Clear();
            LanguageStats.Add(new ColumnSeries
            {
                Title = "Languages",
                Values = new ChartValues<int>(),
                Fill = Brushes.Green
            });
            for (int i = 0; i < LanguageLabels.Count; i++)
            {
                int counter = tourRequestService.GetAll().Where(x => x.Language == (LANGUAGE)Enum.Parse(typeof(LANGUAGE), LanguageLabels[i]) && x.RangeFrom.Year == year).Count();
                LanguageStats[0].Values.Add(counter);
            }
        }

        private void InitializeYears()
        {
            Years = new ObservableCollection<string>();
            Years.Add("All Time");

            var uniqueYears = tourRequestService.GetAll()
                .SelectMany(request => new[] { request.RangeFrom.Year, request.RangeTo.Year })
                .Distinct();

            foreach (var year in uniqueYears)
            {
                Years.Add(year.ToString());
            }
        }

    }
}
