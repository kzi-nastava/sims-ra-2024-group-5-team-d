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

namespace BookingApp.WPF.Views
{
    /// <summary>
    /// Interaction logic for OwnerMainWindow.xaml
    /// </summary>
    public partial class OwnerMainWindow : Window
    {

        public static ContentControl contentControl;
        public static Popup popUp;
        Notifier notifier = new Notifier(cfg =>
        {
            cfg.PositionProvider = new WindowPositionProvider(
                parentWindow: Application.Current.MainWindow,
                corner: Corner.BottomRight,
                offsetX: 0,
                offsetY: 0);

            cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                notificationLifetime: TimeSpan.FromSeconds(3),
                maximumNotificationCount: MaximumNotificationCount.FromCount(5));

            cfg.Dispatcher = Application.Current.Dispatcher;
        });
        User loggedInUser;

        public OwnerMainWindow(User user)
        {
            InitializeComponent();
            loggedInUser = user;
            DataContext = this;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            contentControl = contentControl1;
            popUp = popup_uc;
            contentControl.Content = new OwnerMainWindowUserControl(user);
            contentMenu.Content = new SmallMenuUserControl();
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
            //contentControl.Content = new RegisterAccommodationNoMenu();
        }

        private void Border_MouseLeftButtonDown_3(object sender, MouseButtonEventArgs e)
        {
          //  contentControl.Content = new RegisterAccommodationNoMenu();
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
               /* if (row != -1)
                {
                    if (row == 0)
                        contentControl.Content = new RequestsNoMenu();
                    if (row == 1)
                        contentControl.Content = new Renovations();
                    if (row == 2)
                        contentControl.Content = new ReviewsNoMenu();
                    if (row == 3)
                        contentControl.Content = new ForumNoMenu();
                }*/

            }
        }

        private void HamburgerClick(object sender, MouseButtonEventArgs e)
        {/*
            var options = new MessageOptions
            {
                FontSize = 30, // set notification font size
                ShowCloseButton = false, // set the option to show or hide notification close button
                Tag = "Any object or value which might matter in callbacks",
                FreezeOnMouseEnter = true, // set the option to prevent notification dissapear automatically if user move cursor on it
                NotificationClickAction = n => // set the callback for notification click event
                {
                    n.Close(); // call Close method to remove notification
                    notifier.ShowSuccess("clicked!");
                },
            };
            notifier.ShowSuccess("Success message",options);*/
           // notifier.ShowSuccess("Message");
            if (contentMenu.Content is SmallMenuUserControl)
            {
                Debug.WriteLine("HamburgerClick");
                Grid.SetColumnSpan(contentMenu, 2);
                Grid.SetColumn(contentControl, 2);
                Grid.SetColumnSpan(contentControl, 1);
                contentMenu.Content = new WideMenuUserControl();
            }
            else
            {
                Grid.SetColumnSpan(contentMenu, 1);
                Grid.SetColumn(contentControl, 1);
                Grid.SetColumnSpan(contentControl, 2);
                Debug.WriteLine("HamburgerClick1");
                contentMenu.Content = new SmallMenuUserControl();
            }
        }

        private void Border_MouseLeftButtonDown_4(object sender, MouseButtonEventArgs e)
        {
            if (popUp.IsOpen == false)
            {
                popUp.PlacementTarget = sender as UIElement;
                popUp.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
                popUp.IsOpen = true;
                Header.ala.Text = "Ala";
            }
            else
            {
                popUp.Visibility = Visibility.Collapsed;
                popUp.IsOpen = false;
            }

        }
    }
}
