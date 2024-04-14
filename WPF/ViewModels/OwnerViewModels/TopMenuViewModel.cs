using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
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
            notificationService = new NotificationsService();
            unratedGuestService = new UnratedGuestService();
            loggedInUser = user;
            notificationService.CreateNotificationForUnratedGuests(unratedGuestService.GetUnratedGuests(loggedInUser), user);
            NumberOfNotifications = notificationService.GetNumberOfUnreadNotificationsForUser(user).ToString();
        }
    }
}
