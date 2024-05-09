using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.WPF.Commands;
using BookingApp.WPF.Views.OwnerView;
using BookingApp.WPF.Views.TouristView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels.TourViewModels.TouristViewModels
{
    public class NotificationsViewModel
    {
        public ICommand DeleteNotification { get; private set; }
        public ICommand OpenNotification { get; set; }
        public ICommand MarkAllAsReadCommand { get; set; }
        public ObservableCollection<NotificationViewModel> Notifications { get; set; }
        private User loggedInUser;
        private NotificationsService notificationsService;
        private string message;
        private TourService tourService;
        private NotificationViewModel selectedNotification { get; set; }

        public NotificationsViewModel(User user)
        {
            tourService = new TourService();
            notificationsService = new NotificationsService();
            loggedInUser = user;
            Notifications = new ObservableCollection<NotificationViewModel>();
            notificationsService.GetSortedNotificationsForUser(user).ForEach(notification =>
            {
                message = notificationsService.GenerateMessage(notification);
                User sender = notificationsService.GetSender(notification);
                Notifications.Add(new NotificationViewModel(message, notification, sender));
            });

            OpenNotification = new RelayParameterCommand(OpenNotificationWindow);
            DeleteNotification = new RelayParameterCommand(DeleteExecute);
            MarkAllAsReadCommand = new RelayCommand(MarkAllAsRead);
        }

        public void MarkAllAsRead()
        {
            notificationsService.MarkAllAsRead(loggedInUser.Id);
            foreach(NotificationViewModel notification in Notifications)
            {
                notification.IsUnread = false;
            }
        }

        private void DeleteExecute(object parameter)
        {
            if (parameter is NotificationViewModel notification)
            {
                Notification toRemove = new Notification();
                toRemove.Id = notification.NotificationId;
                notificationsService.Delete(toRemove);
                Notifications.Remove(notification);
            }
        }

        public void OpenNotificationWindow(object parameter)
        {
            selectedNotification = parameter as NotificationViewModel;

            if (selectedNotification != null)
            {
                Notification notification = notificationsService.GetById(selectedNotification.NotificationId);
                switch (notification.Type)
                {
                    case Domain.Models.Type.TOURREQUEST:
                        TouristHomeWindow.contentControl.Content = new YourRequestsUserControl(loggedInUser);
                        break;
                    case Domain.Models.Type.NEWTOUR:
                        Tour tour = tourService.GetById(notification.LinkId);
                        TourViewModel tourView = new TourViewModel(tour.Id, tour.Name, tour.Description, tour.Location, tour.Duration, tour.ImagesPath, tour.MaxCapacity, tour.Language, tour.User);
                        TouristHomeWindow.contentControl.Content = new TourDetailsUserControl(tourView, 1, loggedInUser);
                        break;
                    case Domain.Models.Type.LIVETOUR:
                        TouristHomeWindow.contentControl.Content = new YourToursUserControl(loggedInUser);
                        break;
                    case Domain.Models.Type.VOUCHER:
                        TouristHomeWindow.contentControl.Content = new VouchersUserControl(loggedInUser);
                        break;
                }
                notificationsService.ReadNotification(notification);
            }
        }



    }
}
