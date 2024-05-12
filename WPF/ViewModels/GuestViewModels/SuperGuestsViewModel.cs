using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.WPF.Commands;
using BookingApp.WPF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ToastNotifications.Position;

namespace BookingApp.WPF.ViewModels.GuestViewModels
{
    public class SuperGuestsViewModel
    {
        public ICommand LogOutCommand { get; set; }
        //public string GuestName { get; set; }
        //public string AvatarPath { get; set; }
        public SuperGuestViewModel SuperGuest { get; set; }
        public bool IsSuperGuest { get; set; }
        public string TimeLeft { get; set; }
        //public int NumberOfReservationThisYear { get; set; }
        private SuperUserService superUserService { get; set; }
        private AccommodationReservationService accommodationReservationService { get; set; }
        public SuperGuestsViewModel(User user) 
        {
            LogOutCommand = new RelayCommand(LogOut);
            superUserService = new SuperUserService();
            accommodationReservationService = new AccommodationReservationService();

            SuperGuest = new SuperGuestViewModel(user.Id, superUserService.GetById(user.Id).ValidFrom, superUserService.GetById(user.Id).BonusPoints, user.AvatarPath, accommodationReservationService.GetNumberOfReservationsLastYear(user), user.FullName);

            //GuestName = user.FullName;
            //AvatarPath = user.AvatarPath;
            IsSuperGuest = false;
            //NumberOfReservationThisYear = accommodationReservationService.GetNumberOfReservationsLastYear(user); //proveri sa lukom ???

            TimeSpan remainingTime = DateTime.UtcNow - superUserService.GetById(user.Id).ValidFrom.AddYears(-1) ;

            int years = remainingTime.Days / 365;
            int months = (remainingTime.Days % 365) / 30;
            int days = (remainingTime.Days % 365) % 30;

            string remainingTimeFormatted = "";
            if (years > 0)
            {
                remainingTimeFormatted += $"{years} y/";
            }
            if (months > 0)
            {
                remainingTimeFormatted += $"{months} m/";
            }
            if (days > 0)
            {
                remainingTimeFormatted += $"{days} d";
            }

            TimeLeft = remainingTimeFormatted.Trim();


            if (user.IsSuperUser.HasValue)
            {
                IsSuperGuest = user.IsSuperUser.Value;
            }
        }

        private void LogOut()
        {
            LoginScreen newLoginWindow = new LoginScreen();

            foreach (Window window in Application.Current.Windows)
            {
                if (window != newLoginWindow)
                {
                    window.Close();
                    newLoginWindow.Show();
                }
            }
        }
    }
}
