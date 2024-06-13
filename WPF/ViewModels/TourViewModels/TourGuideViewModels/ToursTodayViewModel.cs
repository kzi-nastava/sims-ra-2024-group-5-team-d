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
    public class ToursTodayViewModel
    {
        public ICommand ViewMoreCommand { get; set; }
        public ICommand AllToursTabCommand { get; set; }
        public ICommand RequestTabCommand { get; set; }
        public ICommand FinishedToursTabCommand { get; set; }
        public ICommand ComplexRequestTabCommand { get; set; }
        public bool IsGridVisible { get; set; }
        public TourViewModel SelectedTour { get; set; }
        public ObservableCollection<TourViewModel> ToursToday { get; set; }
        private TourService tourService;
        private User LoggedInUser { get; set; }
        public ToursTodayViewModel(User user)
        {
            IsGridVisible = false;
            ViewMoreCommand = new RelayParameterCommand(ViewMore);
            AllToursTabCommand = new RelayCommand(AllToursTab);
            FinishedToursTabCommand = new RelayCommand(FinishedToursTab);
            RequestTabCommand = new RelayCommand(RequestsTab);
            ComplexRequestTabCommand = new RelayCommand(ComplexRequestsTab);
            tourService = new TourService();
            ToursToday = new ObservableCollection<TourViewModel>();
            tourService.GetToursForToday(user).ForEach(tour => ToursToday.Add(new TourViewModel(tour.Id, tour.Name, tour.Description, tour.Location, tour.Duration, tour.ImagesPath, tour.MaxCapacity, tour.Language, tour.User)));
            LoggedInUser = user;
        }
        private void AllToursTab()
        {
            SideBar.contentControlW.Content = new AllToursWindow(LoggedInUser);
        }
        private void ViewMore(object selectedTour)
        {
            TourViewModel tourViewModel = selectedTour as TourViewModel;
            SideBar.contentControlW.Content = new ViewMoreTourToday(tourViewModel, LoggedInUser);
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
