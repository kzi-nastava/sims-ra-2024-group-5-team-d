using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace BookingApp.Appl.UseCases
{
    public class GuestNotificationsService
    {
        private NotificationsService notificationsService;
        private GuestInboxService guestInboxService;
        public GuestNotificationsService() 
        {
            notificationsService = new NotificationsService();
            guestInboxService = new GuestInboxService();
        }
        public int GetNumerOfApprovedRequest(User user)
        {
            List<Notification> unreadNotifications = notificationsService.GetUnreadNotificationsCountForUser(user);
            List<GuestRequest> approvedRequest = guestInboxService.GetApprovedRequests(user);
            return unreadNotifications.Where(notification => approvedRequest.Any(request => notification.LinkId == request.Id && notification.isRequest())).Count();
        }
        public int GetNumerOfRejectedRequest(User user)
        {
            List<Notification> unreadNotifications = notificationsService.GetUnreadNotificationsCountForUser(user);
            List<GuestRequest> rejectedRequest = guestInboxService.GetRejectedRequests(user);
            return unreadNotifications.Where(notification => rejectedRequest.Any(request => notification.LinkId == request.Id && notification.isRequest())).Count();
        }
    }
}
