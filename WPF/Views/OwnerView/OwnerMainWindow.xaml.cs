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
using BookingApp.WPF.ViewModels.OwnerViewModels;

namespace BookingApp.WPF.Views.OwnerView
{
    /// <summary>
    /// Interaction logic for OwnerMainWindow.xaml
    /// </summary>
    public partial class OwnerMainWindow : Window
    {
        public static ContentControl contentControl;
        User loggedInUser;
        public OwnerMainWindow(User user)
        {

            InitializeComponent();
            loggedInUser = user;
            DataContext = new TopMenuViewModel(user);
            contentControl = contentControl1;
            contentControl.Content = new OwnerMainWindowUserControl(user);
            contentMenu.Content = new SmallMenuUserControl(user);    
        }


        private void HamburgerClick(object sender, MouseButtonEventArgs e)
        {
            if (contentMenu.Content is SmallMenuUserControl)
            {
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
                contentMenu.Content = new SmallMenuUserControl(loggedInUser);
            }
        }
    }
}
