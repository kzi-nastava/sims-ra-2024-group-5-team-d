using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.WPF.Commands;
using BookingApp.WPF.Views.TouristGuide;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels.TourViewModels.TourGuideViewModels
{
    public class AllToursViewModel
    {
        public ICommand ViewMoreCommand { get; set; }
        public ICommand ToursTodayTabCommand { get; set; }
        public ICommand RequestTabCommand { get; set; }
        public ICommand FinishedToursTabCommand { get; set; }
        public bool IsGridVisible { get; set; }

        public TourViewModel SelectedTour { get; set; }
        public ObservableCollection<TourViewModel> Tours { get; set; }
        public TourService tourService;
        private User LoggedInUser { get; set; }
        public AllToursViewModel(User user)
        {
            IsGridVisible = false;
            LoggedInUser = user;
            tourService = new TourService();
            Tours = new ObservableCollection<TourViewModel>();
            tourService.GetAllTours(user).ForEach(tour => Tours.Add(new TourViewModel(tour.Id, tour.Name, tour.Description, tour.Location, tour.Duration, tour.ImagesPath, tour.MaxCapacity, tour.Language, tour.User)));
            ViewMoreCommand = new RelayCommand(ViewMore);
            ToursTodayTabCommand = new RelayCommand(ToursTodayTab);
            FinishedToursTabCommand = new RelayCommand(FinishedToursTab);
            RequestTabCommand = new RelayCommand(RequestsTab);
        }
        private void ViewMore()
        {
            SideBar.contentControlW.Content = new ViewMoreTour(SelectedTour, LoggedInUser);

        }
        private void ToursTodayTab()
        {
            SideBar.contentControlW.Content = new ToursTodayWindow(LoggedInUser);
        }
        private void FinishedToursTab()
        {
            SideBar.contentControlW.Content = new FinishedToursWindow(LoggedInUser);
        }
        private void RequestsTab()
        {
            SideBar.contentControlW.Content = new RequestsWindow(LoggedInUser);
        }
    }
}
