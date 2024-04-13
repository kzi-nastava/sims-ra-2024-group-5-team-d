using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    public class NotificationsViewModel
    {
        public ObservableCollection<NotificationViewModel> Notifications { get; set; }
        private User loggedInUser;
        public NotificationsViewModel(User user)
        {
            loggedInUser = user;
            Notifications = new ObservableCollection<NotificationViewModel>();
        }
    }
}
