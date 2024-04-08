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
        public GuestWindow(User user)
        {
            InitializeComponent();
            LoggedInUser = user;
            DataContext = this;
            contentControl = ContentControl;
            contentControl.Content = new SearchAccommodationUserControl(user, contentControl);

        }
        private void TravelBagIcon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            contentControl.Content = new ReservationsAccommodationUserControl(LoggedInUser, Accommodation);
        }

        private void InboxOpen_Button(object sender, MouseButtonEventArgs e)
        {
            contentControl.Content = new InboxAccommodationUserControl();
        }

        private void Search_MouseLeftBottonDown(object sender, MouseButtonEventArgs e)
        {
            contentControl.Content = new SearchAccommodationUserControl(LoggedInUser, contentControl);
        }
    }
}
