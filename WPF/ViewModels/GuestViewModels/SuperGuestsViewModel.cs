using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.Commands;
using BookingApp.WPF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
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
        public SuperGuestViewModel SuperGuest { get; set; }
        public bool IsSuperGuest { get; set; }
        public string TimeLeft { get; set; }

        private SuperUserService superUserService;
        private AccommodationReservationService accommodationReservationService;

        public SuperGuestsViewModel(User user) 
        {
            InitializeServices();

            LogOutCommand = new RelayCommand(LogOut);

            SuperGuest = new SuperGuestViewModel(user.Id, superUserService.GetById(user.Id).ValidFrom, superUserService.GetById(user.Id).BonusPoints, user.AvatarPath, accommodationReservationService.GetNumberOfReservationsLastYear(user), user.FullName);
            IsSuperGuest = false;

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

        private void InitializeServices()
        {
            superUserService = new SuperUserService(Injector.CreateInstance<ISuperUserRepository>());
            accommodationReservationService = new AccommodationReservationService(Injector.CreateInstance<IAccommodationReservationRepository>());
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
