using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
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
using ToastNotifications.Core;
using Xceed.Wpf.Toolkit.Primitives;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    public class NotificationsViewModel
    {
        public ICommand OpenNotification { get; set; }

        private NotificationsService notificationsService;

        public ObservableCollection<NotificationViewModel> Notifications { get; set; }
        private User loggedInUser;
        private string message;
        public NotificationsViewModel(User user)
        {
            InitializeServices();
            loggedInUser = user;
            Notifications = new ObservableCollection<NotificationViewModel>();           
            notificationsService.GetSortedNotificationsForUser(user).ForEach(notification =>
            {
                message=notificationsService.GenerateMessage(notification);
                User sender= notificationsService.GetSender(notification);
                Notifications.Add(new NotificationViewModel(message,notification,sender));
            });
            OpenNotification = new RelayParameterCommand(OpenNotificationWindow);
        }

        private void InitializeServices()
        {
            UserService userService = new UserService(Injector.CreateInstance<IUserRepository>());
            AccommodationService accommodationService = new AccommodationService(Injector.CreateInstance<IAccommodationRepository>(), userService);
            AccommodationReservationService accommodationReservationService = new AccommodationReservationService(Injector.CreateInstance<IAccommodationReservationRepository>());
            GuestRequestService guestRequestService = new GuestRequestService(Injector.CreateInstance<IGuestRequestRepository>());
            LocationService locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            ForumService forumService = new ForumService(Injector.CreateInstance<IForumRepository>(), locationService);
            notificationsService = new NotificationsService(Injector.CreateInstance<INotificationRepository>(),userService,accommodationService,accommodationReservationService,guestRequestService,forumService);
        }
        public void OpenNotificationWindow(object parameter)
        {
            if (parameter != null)
            {
                NotificationViewModel selectedNotification = (NotificationViewModel)parameter;
                Notification notification = notificationsService.GetById(selectedNotification.NotificationId);
                switch(notification.Type)
                {
                    case Domain.Models.Type.RATE:
                        OwnerMainWindow.contentControl.Content = new UnratedGuestsUserControl(loggedInUser);
                        break;
                    case Domain.Models.Type.REQUEST:
                        OwnerMainWindow.contentControl.Content = new RequestsUserControl(loggedInUser);
                        break;
                    case Domain.Models.Type.FORUM:
                        OwnerMainWindow.contentControl.Content = new ForumReadMoreUserControl(notification.LinkId, loggedInUser);
                        break;
                }
                if (notification.IsRead == false)
                    notificationsService.ReadNotification(notification);
                
            }
        }
    }
}
