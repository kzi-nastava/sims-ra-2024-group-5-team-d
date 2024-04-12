using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.WPF.Commands;
using BookingApp.WPF.Views.TouristGuide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels
{
    public class FinishedTourViewModel
    {
        public TourViewModel SelectedTour { get; set; }
        public AgeStatisticsViewModel AgeStatistics { get; set; }
        private User LoggedInUser { get; set; }
        public ICommand BackCommand { get; set; }
        
        public TourGuestService tourGuestService { get; set; }

        public FinishedTourViewModel(TourViewModel tour, User user) 
        {
            LoggedInUser = user;
            SelectedTour = tour;
            BackCommand = new RelayCommand(BackButton);
            AgeStatistics = new AgeStatisticsViewModel();
            tourGuestService = new TourGuestService();
            CalculateAgeStatistics();
        }

        private void BackButton()
        {
            SideBar.contentControlW.Content = new FinishedToursWindow(LoggedInUser);
        }

        private void CalculateAgeStatistics()
        {
            int under18Count = 0;
            int age18To50Count = 0;
            int over50Count = 0;

            var tourGuests = tourGuestService.GetTourGuestsOnTourRealisation(SelectedTour.Id);

            foreach (var guest in tourGuests)
            {
                if (guest.Years < 18)
                {
                    under18Count++;
                }
                else if (guest.Years >= 18 && guest.Years <= 50)
                {
                    age18To50Count++;
                }
                else
                {
                    over50Count++;
                }
            }

            AgeStatistics.Under18Count = under18Count;
            AgeStatistics.Age18To50Count = age18To50Count;
            AgeStatistics.Over50Count = over50Count;
        }




    }

}
