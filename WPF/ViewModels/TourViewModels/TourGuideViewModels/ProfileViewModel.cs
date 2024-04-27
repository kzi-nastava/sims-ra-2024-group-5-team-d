using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private User LoggedInUser { get; set; }
        public TourService tourService { get; set; }
        public TourRealisationService tourRealisationService { get; set; }
        public LocationService locationService { get; set; }
        public ProfileViewModel(User user) 
        { 
            LoggedInUser = user;
            tourService = new TourService();
            tourRealisationService = new TourRealisationService();
            locationService = new LocationService();
            Tour = new ObservableCollection<TourViewModel>();
            Date = new ObservableCollection<string>();
            ComboBoxSelectionChangedCommand = new RelayParameterCommand(OnComboBoxSelectionChanged);
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
            TourRealisation first = tourRealisationService.GetFirstTourRealisationMadeByUser(LoggedInUser);
            TourRealisation last = tourRealisationService.GetLastTourRealisationMadeByUser(LoggedInUser);
            Date.Add("All Time");
            for (int i = last.StartTime.Year; i >= first.StartTime.Year; i--)
                Date.Add(i.ToString());
        }

    }
}
