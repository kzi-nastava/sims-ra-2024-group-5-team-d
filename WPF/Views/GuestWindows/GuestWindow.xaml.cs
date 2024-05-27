using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookingApp.WPF.Views.GuestWindows
{
    /// <summary>
    /// Interaction logic for GuestWindow.xaml
    /// </summary>
    public partial class GuestWindow : Window
    {
        public User LoggedInUser { get; set; }
        public static ContentControl contentControl;
        public Accommodation Accommodation { get; set; }
        public NotificationsService notificationsService { get; set; }
        public int NumberOfNotifications { get; set; }
        public GuestWindow(User user)
        {
            InitializeComponent();
            LoggedInUser = user;
            DataContext = this;
            contentControl = ContentControl;
            notificationsService = new NotificationsService();
            NumberOfNotifications = notificationsService.GetNumberOfUnreadNotificationsForUser(LoggedInUser);
            contentControl.Content = new SearchAccommodationUserControl(user);

        }
        private void TravelBagIcon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            contentControl.Content = new ReservationsAccommodationUserControl(LoggedInUser);
        }

        private void InboxOpen_Button(object sender, MouseButtonEventArgs e)
        {
            contentControl.Content = new InboxAccommodationUserControl(LoggedInUser);
        }

        private void Search_MouseLeftBottonDown(object sender, MouseButtonEventArgs e)
        {
            contentControl.Content = new SearchAccommodationUserControl(LoggedInUser);
        }

        private void ProfileIcon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            contentControl.Content = new ProfileUserControl(LoggedInUser);
        }

        private void ForumOpen_Button(object sender, MouseButtonEventArgs e)
        {
            contentControl.Content = new ForumUserControl(LoggedInUser);
        }

        private void Help_MouseLeftBottonDown(object sender, MouseButtonEventArgs e)
        {
            //WizzardWindow helpWindow = new WizzardWindow();

            //helpWindow.Show();
            contentControl.Content = new HelpUserControl();

        }
    }
}
