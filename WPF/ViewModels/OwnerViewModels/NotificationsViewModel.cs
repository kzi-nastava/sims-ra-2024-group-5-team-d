using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.WPF.Commands;
using BookingApp.WPF.Views.OwnerView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xceed.Wpf.Toolkit.Primitives;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    public class NotificationsViewModel
    {
        public ICommand SelectionChangedCommand { get; set; }
        public ICommand OpenNotification { get; set; }
        public ObservableCollection<NotificationViewModel> Notifications { get; set; }
        private User loggedInUser;
        private NotificationsService notificationsService;
        private string message;
        private NotificationViewModel selectedNotification;
        int selectedNotificationIndex=-1;
        public NotificationsViewModel(User user)
        {
            notificationsService = new NotificationsService();
            loggedInUser = user;
            Notifications = new ObservableCollection<NotificationViewModel>();           
            notificationsService.GetSortedNotificationsForUser(user).ForEach(notification =>
            {
                message=notificationsService.GenerateMessage(notification);
                User sender= notificationsService.GetSender(notification);
                Notifications.Add(new NotificationViewModel(message,notification,sender));
            });
            SelectionChangedCommand = new RelayParameterCommand(ListViewSelectionChanged);
            OpenNotification = new RelayCommand(OpenNotificationWindow);
        }
        public void ListViewSelectionChanged(object parameter)
        {
            if (parameter != null)
            {
                selectedNotification = parameter as NotificationViewModel;
            }
        }
        public void OpenNotificationWindow()
        {
            if (selectedNotification != null)
            {
                Notification notification = notificationsService.GetById(selectedNotification.NotificationId);
                switch(notification.Type)
                {
                    case Domain.Models.Type.RATE:
                        OwnerMainWindow.contentControl.Content = new UnratedGuestsUserControl(loggedInUser);
                        break;
                    case Domain.Models.Type.REQUEST:
                        OwnerMainWindow.contentControl.Content = new RequestsUserControl(loggedInUser);
                        break;
                }
                notificationsService.ReadNotification(notification);
            }
        }
    }
}
