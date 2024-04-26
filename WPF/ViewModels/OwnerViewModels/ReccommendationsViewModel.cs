using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    public class ReccommendationsViewModel
    {

        public SeriesCollection BestLocations { get; set; }
        public ObservableCollection<string> Locations { get; set; }
        private LocationService locationService;
        public ReccommendationsViewModel()
        {
            locationService = new LocationService();
            List<KeyValuePair<Location, double>> locations = locationService.GetMostPopularLocations();
            BestLocations = new SeriesCollection();
            Locations = new ObservableCollection<string>();
            BestLocations.Add(new ColumnSeries
            {
                Title = "Popularity",
                Values = new ChartValues<double>(),
                Fill = System.Windows.Media.Brushes.ForestGreen
            });
            foreach (KeyValuePair<Location, double> location in locations)
            {
                /*BestLocations.Add(new ColumnSeries
                {
                    Title = l.Name,
                    Values = new ChartValues<double> { l.Popularity }
                });*/
                BestLocations[0].Values.Add(location.Value);
                Locations.Add(location.Key.City);
            }
            Debug.WriteLine("Best locations: " + Locations.Count);
        }
    }
}
