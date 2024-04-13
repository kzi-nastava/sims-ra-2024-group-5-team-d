using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using ToastNotifications.Messages;
using BookingApp.Domain.Models;
using BookingApp.Appl.UseCases;
using BookingApp.WPF.Commands;

namespace BookingApp.WPF.Views.OwnerView
{
    /// <summary>
    /// Interaction logic for OwnerMainWindow.xaml
    /// </summary>
    public partial class OwnerMainWindow : Window
    {

        public ICommand ReviewCommand { get; private set; }
        public static ContentControl contentControl;
        User loggedInUser;
        private UnratedGuestService unratedGuestService;
        private NotificationsService notificationService;
        public OwnerMainWindow(User user)
        {

            notificationService = new NotificationsService();
            unratedGuestService = new UnratedGuestService();
            InitializeComponent();
            loggedInUser = user;
            DataContext = this;
            ReviewCommand = new RelayCommand(OpenReview);
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            contentControl = contentControl1;
            contentControl.Content = new OwnerMainWindowUserControl(user);
            contentMenu.Content = new SmallMenuUserControl(loggedInUser);
            notificationService.CreateNotificationForUnratedGuests(unratedGuestService.GetUnratedGuests(loggedInUser), user);
            numberOfNotify.Text = notificationService.GetNumberOfUnreadNotificationsForUser(user).ToString();
        }
        private void OpenReview() {
            contentControl.Content = new OwnerReviewUserControl(loggedInUser);
        }
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
          //  contentControl.Content = new ProfileNoMenu();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("Usao");
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("Grid_MouseLeftButtonDown");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Button_Click");
        }


        private void Border_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            contentControl.Content = new OwnerMainWindowUserControl(loggedInUser);
        }

        private void Border_MouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {
            contentControl.Content = new RegisterAccommodationUserControl(loggedInUser);
        }

        private void Border_MouseLeftButtonDown_3(object sender, MouseButtonEventArgs e)
        {
            contentControl.Content = new RegisterAccommodationUserControl(loggedInUser);
        }

        private void LeftMenu(object sender, MouseButtonEventArgs e)
        {
            if (sender is Grid clickedGrid)
            {
                // Pronalaženje pozicije na kojoj je kliknut
                Point clickPoint = e.GetPosition(clickedGrid);

                // Pronalaženje reda na kojem je kliknut
                int row = -1;
                double accumulatedHeight = 0.0;
                foreach (var rowDefinition in clickedGrid.RowDefinitions)
                {
                    accumulatedHeight += rowDefinition.ActualHeight;
                    if (accumulatedHeight >= clickPoint.Y)
                    {
                        row = clickedGrid.RowDefinitions.IndexOf(rowDefinition);
                        break;
                    }
                }

                // Ako je pronađen red, prikazujemo njegov indeks
                if (row != -1)
                {
                    if (row == 0)
                        contentControl.Content = new RequestsUserControl(loggedInUser);
                    //if (row == 1)
                      //  contentControl.Content = new Renovations();
                    if (row == 2)
                        contentControl.Content = new OwnerReviewUserControl(loggedInUser);
                    //if (row == 3)
                        //contentControl.Content = new ForumNoMenu();
                }

            }
        }

        private void HamburgerClick(object sender, MouseButtonEventArgs e)
        {
            if (contentMenu.Content is SmallMenuUserControl)
            {
                Debug.WriteLine("HamburgerClick");
                Grid.SetColumnSpan(contentMenu, 2);
                Grid.SetColumn(contentControl, 2);
                Grid.SetColumnSpan(contentControl, 1);
                contentMenu.Content = new WideMenuUserControl(loggedInUser);
            }
            else
            {
                Grid.SetColumnSpan(contentMenu, 1);
                Grid.SetColumn(contentControl, 1);
                Grid.SetColumnSpan(contentControl, 2);
                Debug.WriteLine("HamburgerClick1");
                contentMenu.Content = new SmallMenuUserControl(loggedInUser);
            }
        }

        private void Border_MouseLeftButtonDown_4(object sender, MouseButtonEventArgs e)
        {
            contentControl.Content = new OwnerNotificationsUserControl(loggedInUser);

        }
    }
}
