using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.Commands;
using BookingApp.WPF.Views.OwnerView;
using BookingApp.WPF.Views.TouristView;
using BookingApp.WPF.Views.Utils;
using HarfBuzzSharp;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BookingApp.WPF.ViewModels.TourViewModels.TouristViewModels
{
    
    public class RequestStatisticsViewModel : INotifyPropertyChanged
    {
        public ICommand ComplexRequestsTabCommand { get; private set; }
        public ICommand ComboBoxSelectionChangedCommand { get; set; }   
        public ICommand PrintPDFCommand { get; set; }   
        public ICommand GoBackCommand { get; set; }   
        public User Tourist { get; set; }
        public SeriesCollection RequestsStatistics { get; set; }
        public SeriesCollection LanguageStats { get; set; }
        public List<int> RequestCounterForLanguage {  get; set; }
        public List<int> RequestCounterForLocation { get; set; }
        public SeriesCollection LocationStats { get; set; }
        public int AcceptedRequests { get; set; }

        private double avgNumberOfPeople;
        public double AvgNumberOfPeople
        {
            get => avgNumberOfPeople;
            set
            {
                if (value != avgNumberOfPeople)
                {
                    avgNumberOfPeople = value;
                    OnPropertyChanged();
                }
            }
        }

        public int ItemIndex = 0;
        public int NotAcceptedRequests { get; set; }
        public TourRequestService tourRequestService { get; set; }
        public ObservableCollection<string> LanguageLabels { get; set; }
        public ObservableCollection<string> Years { get; set; }
        public ObservableCollection<string> LocationLabels { get; set; } 
        public string SelectedYear { get; set; }

        private LocationService locationService { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public RequestStatisticsViewModel(User tourist) 
        {
            locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            RequestCounterForLanguage = new List<int>();
            RequestCounterForLocation = new List<int>();
            Tourist = tourist;
            LanguageStats = new SeriesCollection();
            LocationStats = new SeriesCollection();
            tourRequestService = new TourRequestService();
            LanguageLabels = new ObservableCollection<string>();

            ComplexRequestsTabCommand = new RelayCommand(ShowComplexRequests);
            GoBackCommand = new RelayCommand(GoBack);
            PrintPDFCommand = new RelayCommand(Print);

            LocationLabels = new ObservableCollection<string>();
            for (int i = 0; i < Enum.GetNames(typeof(LANGUAGE)).Length; i++)
            {
                LanguageLabels.Add(Enum.GetNames(typeof(LANGUAGE))[i]);
            }

            locationService.GetAll().ForEach(loc => LocationLabels.Add(loc.City.ToString()));
            RequestsStatistics = new SeriesCollection();

            ComboBoxSelectionChangedCommand = new RelayParameterCommand(OnComboBoxSelectionChanged);

            InitializeYears();

            OnComboBoxSelectionChanged("All Time");
        }

        public void Print()
        {
            string pdfPath;
            PDFGenerator pdfGenerator = new PDFGenerator();
            pdfPath = "";
            if (AppLanguage.CurrentLanguage.Equals("English"))
                pdfPath = pdfGenerator.CreateTouristRequestStatisticsPDFEnglish(this);
            else if (AppLanguage.CurrentLanguage.Equals("Serbian"))
                pdfPath = pdfGenerator.CreateTouristRequestStatisticsPDFSerbian(this);

            if (pdfPath != "")
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = pdfPath,
                    UseShellExecute = true
                });
            }
            SavePDFWindow savePDFWindow = new SavePDFWindow(pdfPath);
            savePDFWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            savePDFWindow.ShowDialog();
        }

        public void GoBack()
        {

            TouristHomeWindow.contentControl.Content = new YourRequestsUserControl(Tourist);
        }

        public void ShowComplexRequests()
        {
            TouristHomeWindow.contentControl.Content = new ComplexRequestsUserControl(Tourist);
        }

        public void InitializeLocationStatistics()
        {
            SelectedYear = "All Time";
            LocationStats.Clear();
            RequestCounterForLocation.Clear();
            LocationStats.Add(new ColumnSeries
            {
                Title = "Locations",
                Values = new ChartValues<int>(),
                Fill = Brushes.Green
            });
            for (int i = 0; i < LocationLabels.Count; i++)
            {
                int counter = tourRequestService.GetRequestsForTourist(Tourist).Where(x => locationService.GetById(x.Location.Id).City.ToString() == LocationLabels[i]).Count();                LocationStats[0].Values.Add(counter);
                RequestCounterForLocation.Add(counter);
            }

        }

        public void InitializeLocationStatistics(int year)
        {
            LocationStats.Clear();
            RequestCounterForLocation.Clear();
            LocationStats.Add(new ColumnSeries
            {
                Title = "Locations",
                Values = new ChartValues<int>(),
                Fill = Brushes.Green
            });
            for (int i = 0; i < LocationLabels.Count; i++)
            {
                int counter = tourRequestService.GetRequestsForTourist(Tourist).Where(x => locationService.GetById(x.Location.Id).City.ToString() == LocationLabels[i] && 
                x.RangeFrom.Year == year).Count();
                LocationStats[0].Values.Add(counter);
                RequestCounterForLocation.Add(counter);
            }
        }

        public void OnComboBoxSelectionChanged(object parameter)
        {
            
            string selectedYear = parameter as string;

            if (selectedYear != null)
            {

                SelectedYear = selectedYear;
                if (selectedYear != "All Time")
                {
                    if(int.TryParse(selectedYear, out int SelectedYear))
                    {
                        InitializeRequestsStatistics(SelectedYear);
                        InitializeLanguageStatistics(SelectedYear);
                        InitializeLocationStatistics(SelectedYear);
                        AvgNumberOfPeople = tourRequestService.AverageNumberOfGuestsOnAcceptedRequests(SelectedYear,Tourist);
                    }
                }
                else
                {
                    InitializeRequestsStatistics();
                    InitializeLanguageStatistics();                  
                    InitializeLocationStatistics();
                    AvgNumberOfPeople = tourRequestService.AllTimeAverageNumberOfPeopleOnAcceptedRequests();
                }
            }
        }

        

        public void InitializeRequestsStatistics()
        {
            AcceptedRequests = tourRequestService.GetRequestsForTourist(Tourist).Count(request => request.Status == STATE.ACCEPTED);
            NotAcceptedRequests = tourRequestService.GetRequestsForTourist(Tourist).Count(request => request.Status != STATE.ACCEPTED);

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
            RequestCounterForLanguage.Clear();
            LanguageStats.Add(new ColumnSeries
            {
                Title = "Languages",
                Values = new ChartValues<int>(),
                Fill = Brushes.Green
            });
            for (int i = 0; i < LanguageLabels.Count; i++)
            {
                int counter = tourRequestService.GetRequestsForTourist(Tourist).Where(x => x.Language == (LANGUAGE)Enum.Parse(typeof(LANGUAGE), LanguageLabels[i])).Count();
                LanguageStats[0].Values.Add(counter);
                RequestCounterForLanguage.Add(counter);
            }
        }

        public void InitializeRequestsStatistics(int year)
        {
            AcceptedRequests = tourRequestService.GetRequestsForTourist(Tourist).Count(request => request.Status == STATE.ACCEPTED && request.RangeFrom.Year == year);
            NotAcceptedRequests = tourRequestService.GetRequestsForTourist(Tourist).Count(request => request.Status != STATE.ACCEPTED && request.RangeFrom.Year == year);

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
            RequestCounterForLanguage.Clear();
            LanguageStats.Add(new ColumnSeries
            {
                Title = "Languages",
                Values = new ChartValues<int>(),
                Fill = Brushes.Green
            });
            for (int i = 0; i < LanguageLabels.Count; i++)
            {
                int counter = tourRequestService.GetRequestsForTourist(Tourist).Where(x => x.Language == (LANGUAGE)Enum.Parse(typeof(LANGUAGE), LanguageLabels[i]) && x.RangeFrom.Year == year).Count();
                LanguageStats[0].Values.Add(counter);
                RequestCounterForLanguage.Add(counter);
            }
        }

        private void InitializeYears()
        {
            Years = new ObservableCollection<string>();
            Years.Add("All Time");

            var uniqueYears = tourRequestService.GetRequestsForTourist(Tourist)
                .SelectMany(request => new[] { request.RangeFrom.Year, request.RangeTo.Year })
                .Distinct();

            foreach (var year in uniqueYears)
            {
                Years.Add(year.ToString());
            }
        }

    }
}
