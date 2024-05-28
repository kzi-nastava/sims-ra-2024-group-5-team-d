using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.Commands;
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

namespace BookingApp.WPF.ViewModels
{
    public class ProfileViewModel
    {
        public ICommand ComboBoxSelectionChangedCommand {  get; set; }
        public ObservableCollection<TourViewModel> Tour { get; set; }
        public ObservableCollection<string> Date { get; set; }

        public User LoggedInUser { get; set; }
        public TourService tourService { get; set; }
        public TourRealisationService tourRealisationService { get; set; }
        public TourRatingService tourRatingService { get; set; }
        public LocationService locationService { get; set; }
        public SuperGuideService superGuideService { get; set; }
        public bool IsSuperGuide { get; set; }
        public double AverageRating { get; set; }
        public int NumberOfRatings { get; set; }
        public ProfileViewModel(User user) 
        { 
            LoggedInUser = user;
            superGuideService = new SuperGuideService(user);
            IsSuperGuide = superGuideService.ShouldHaveTheSuperStatus(LoggedInUser);
            tourService = new TourService();
            tourRealisationService = new TourRealisationService();
            tourRatingService = new TourRatingService();
            locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            Tour = new ObservableCollection<TourViewModel>();
            Date = new ObservableCollection<string>();
            ComboBoxSelectionChangedCommand = new RelayParameterCommand(OnComboBoxSelectionChanged);
            AverageRating = tourRatingService.GetAverageRatingForGuide(LoggedInUser);
            NumberOfRatings = tourRatingService.RatingsOfGuide(LoggedInUser).Count();
            InitializeComboBox();
            OnComboBoxSelectionChanged("All Time");
        }

        private void OnComboBoxSelectionChanged(object parameter)
        {
            Tour.Clear();
            string selectedValue = parameter as string;
            if (selectedValue != null)
            {
                if (selectedValue == "All Time")
                {
                    
                    Tour tour = tourService.GetBestTourOfAllTime();
                    Tour.Add(new TourViewModel(tour.Id, tour.Name, tour.Description, locationService.GetById(tour.Location.Id), tour.Duration, tour.ImagesPath, tour.MaxCapacity,tour.Language, tour.User));
                }
                else
                {
                    if (int.TryParse(selectedValue, out int selectedYear))
                    {
                        Tour tour = tourService.GetBestTourInAYear(selectedYear);
                        if(tour == null) {  return; }
                        Tour.Add(new TourViewModel(tour.Id, tour.Name, tour.Description, locationService.GetById(tour.Location.Id), tour.Duration, tour.ImagesPath, tour.MaxCapacity, tour.Language, tour.User));
                    }
                }
            }
        }
        private void InitializeComboBox()
        {
            Date.Add("All Time");

            var uniqueYears = tourRealisationService.GetAllTourRealisations()
                .SelectMany(realisation => new[] { realisation.StartTime.Year })
                .Distinct();
            foreach (var year in uniqueYears)
            {
                Date.Add(year.ToString());
            }
        }

    }
}
