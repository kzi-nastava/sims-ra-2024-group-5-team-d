using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.Commands;
using BookingApp.WPF.Views.OwnerView;
using ceTe.DynamicPDF.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    public class TopMenuViewModel :INotifyPropertyChanged
    {
        public ICommand HomeCommand { get; set; }
        public ICommand RegisterCommand { get; set; }
        public ICommand ProfileCommand { get; set; }
        public ICommand NotificationCommand { get; set; }
        public ICommand RequestsCommand { get; set; }
        public ICommand RenovationsCommand { get; set; }
        public ICommand ReviewsCommand { get; set; }
        public ICommand ForumsCommand { get; set; }
        public ICommand ShortcutsCommand { get; set; }
        private User loggedInUser;
        private UnratedGuestService unratedGuestService;
        private NotificationsService notificationService;

        public event PropertyChangedEventHandler? PropertyChanged;
        public int NumberOfNotifications { get; set; }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public TopMenuViewModel()
        {
        }
        public TopMenuViewModel(User user)
        {

            InitializeServices();
            loggedInUser = user;
            notificationService.CreateNotificationForUnratedGuests(unratedGuestService.GetUnratedGuests(loggedInUser), user);
            NumberOfNotifications = notificationService.GetNumberOfUnreadNotificationsForUser(user);
            HomeCommand = new RelayCommand(Home);
            RegisterCommand = new RelayCommand(Register);
            ProfileCommand = new RelayCommand(Profile);
            NotificationCommand = new RelayCommand(Notifications);
            ShortcutsCommand = new RelayCommand(Shortcuts);
            RequestsCommand = new RelayCommand(Requests);
            RenovationsCommand = new RelayCommand(Renovations);
            ReviewsCommand = new RelayCommand(Reviews);
            ForumsCommand = new RelayCommand(Forums);

        }   

        private void InitializeServices()
        {
            UserService userService = new UserService(Injector.CreateInstance<IUserRepository>());
            AccommodationService accommodationService = new AccommodationService(Injector.CreateInstance<IAccommodationRepository>(), userService);
            unratedGuestService = new UnratedGuestService(accommodationService);
            notificationService = new NotificationsService(Injector.CreateInstance<INotificationRepository>());
        }
        private void Home()
        {
            OwnerMainWindow.contentControl.Content = new OwnerMainWindowUserControl(loggedInUser);
        }

        private void Profile()
        {
            OwnerMainWindow.contentControl.Content = new OwnerProfileUserControl(loggedInUser);
        }

        private void Register()
        {
            OwnerMainWindow.contentControl.Content = new RegisterAccommodationUserControl(loggedInUser);
        }

        private void Shortcuts()
        {
            ShortcutsWindow shortcutsWindow = new ShortcutsWindow(loggedInUser);
            shortcutsWindow.Show();
        }
        private void Notifications()
        {
            OwnerMainWindow.contentControl.Content = new OwnerNotificationsUserControl(loggedInUser);

        }
        public void Requests()
        {
            OwnerMainWindow.contentControl.Content = new RequestsUserControl(loggedInUser);
        }
        public void Renovations()
        {
            OwnerMainWindow.contentControl.Content = new RenovationsUserControl(loggedInUser);
        }
        public void Reviews()
        {
            OwnerMainWindow.contentControl.Content = new OwnerReviewUserControl(loggedInUser);
        }
        public void Forums()
        {
            OwnerMainWindow.contentControl.Content = new ForumUserControl(loggedInUser);
        }
    }
}
