using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    public class TopMenuViewModel :INotifyPropertyChanged
    {
        private User loggedInUser;
        private UnratedGuestService unratedGuestService;
        private NotificationsService notificationService;

        public event PropertyChangedEventHandler? PropertyChanged;
        public int NumberOfNotifications { get; set; }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public TopMenuViewModel(User user)
        {

            InitializeServices();
            loggedInUser = user;
            notificationService.CreateNotificationForUnratedGuests(unratedGuestService.GetUnratedGuests(loggedInUser), user);
            NumberOfNotifications = notificationService.GetNumberOfUnreadNotificationsForUser(user);
        }   

        private void InitializeServices()
        {
            UserService userService = new UserService(Injector.CreateInstance<IUserRepository>());
            AccommodationService accommodationService = new AccommodationService(Injector.CreateInstance<IAccommodationRepository>(), userService);
            unratedGuestService = new UnratedGuestService(accommodationService);
            notificationService = new NotificationsService(Injector.CreateInstance<INotificationRepository>());
        }
    }
}
