using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    public class TopMenuViewModel
    {
        private User loggedInUser;
        private UnratedGuestService unratedGuestService;
        private NotificationsService notificationService;
        public string NumberOfNotifications { get; set; }
        public TopMenuViewModel(User user)
        {
            InitializeServices();
            loggedInUser = user;
            notificationService.CreateNotificationForUnratedGuests(unratedGuestService.GetUnratedGuests(loggedInUser), user);
            NumberOfNotifications = notificationService.GetNumberOfUnreadNotificationsForUser(user).ToString();
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
