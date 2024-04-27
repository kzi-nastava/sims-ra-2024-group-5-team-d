using BookingApp.Domain.Models;
using BookingApp.WPF.Commands;
using BookingApp.WPF.Views.TouristGuide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.WPF.ViewModels.TourViewModels.TourGuideViewModels
{
    public class RequestsStatisticsViewModel
    {
        public User LoggedInUser { get; set; }
        public RelayCommand HelpCommand { get; set; }
        public RelayCommand BackCommand { get; set; }
        public RequestsStatisticsViewModel(User user) 
        {
            LoggedInUser = user;
            HelpCommand = new RelayCommand(HelpButton_Click);
            BackCommand = new RelayCommand(Back);
        }
        private void HelpButton_Click()
        {
            if (RequestsStatistics.HelpPopUp.IsOpen)
                RequestsStatistics.HelpPopUp.IsOpen = false;
            else
                RequestsStatistics.HelpPopUp.IsOpen = true;
        }
        private void Back()
        {
            SideBar.contentControlW.Content = new RequestsWindow(LoggedInUser);
        }
    }
}
