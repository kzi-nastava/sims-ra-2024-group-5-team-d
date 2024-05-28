using BookingApp.Domain.Models;
using BookingApp.WPF.Commands;
using BookingApp.WPF.Views.TouristGuide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels.TourViewModels.TourGuideViewModels
{
    public class ComplexRequestsViewModel
    {
        public User LoggedInUser { get; set; }
        public ICommand ToursTodayTabCommand { get; set; }
        public ICommand RequestTabCommand { get; set; }
        public ICommand FinishedToursTabCommand { get; set; }
        public ICommand AllToursTabCommand { get; set; }
        public ComplexRequestsViewModel(User user) {
            LoggedInUser = user;
            ToursTodayTabCommand = new RelayCommand(ToursTodayTab);
            FinishedToursTabCommand = new RelayCommand(FinishedToursTab);
            RequestTabCommand = new RelayCommand(RequestsTab);
            AllToursTabCommand = new RelayCommand(AllToursTab);
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
    }
}
