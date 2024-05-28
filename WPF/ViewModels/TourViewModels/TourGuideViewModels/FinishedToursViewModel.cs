using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.Commands;
using BookingApp.WPF.Views.TouristGuide;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels.TourViewModels.TourGuideViewModels
{
    public class FinishedToursViewModel
    {
        public ICommand ViewMoreCommand { get; set; }
        public ICommand ToursTodayTabCommand { get; set; }
        public ICommand RequestTabCommand { get; set; }
        public ICommand ComplexRequestTabCommand { get; set; }
        public ICommand AllToursTabCommand { get; set; }
        public bool IsGridVisible { get; set; }
        private User LoggedInUser { get; set; }
        public TourViewModel SelectedTour { get; set; }
        public ObservableCollection<TourViewModel> FinishedTours { get; set; }
        private TourService tourService;
        public FinishedToursViewModel(User user)
        {
            ViewMoreCommand = new RelayParameterCommand(ViewMore);
            ToursTodayTabCommand = new RelayCommand(ToursTodayTab);
            AllToursTabCommand = new RelayCommand(AllToursTab);
            RequestTabCommand = new RelayCommand(RequestsTab);
            ComplexRequestTabCommand = new RelayCommand(ComplexRequestsTab);
            IsGridVisible = false;
            LoggedInUser = user;
            tourService = new TourService();
            FinishedTours = new ObservableCollection<TourViewModel>();
            tourService.GetFinishedTours(user).ForEach(tour => FinishedTours.Add(new TourViewModel(tour.Id, tour.Name, tour.Description, tour.Location, tour.Duration, tour.ImagesPath, tour.MaxCapacity, tour.Language, tour.User)));
        }
        private void ViewMore(object selectedTour)
        {
            TourViewModel tourViewModel = selectedTour as TourViewModel;
            SideBar.contentControlW.Content = new ViewMoreFinishedTour(tourViewModel, LoggedInUser);
        }
        private void AllToursTab()
        {
            SideBar.contentControlW.Content = new AllToursWindow(LoggedInUser);
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
        private void ComplexRequestsTab()
        {
            SideBar.contentControlW.Content = new ComplexRequestsWindow(LoggedInUser);
        }
    }
}
