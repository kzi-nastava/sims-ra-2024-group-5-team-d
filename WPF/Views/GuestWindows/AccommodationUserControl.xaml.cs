using BookingApp.Domain.Models;
using BookingApp.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.WPF.Views.GuestWindows
{
    /// <summary>
    /// Interaction logic for AccommodationUserControl.xaml
    /// </summary>
    public partial class AccommodationUserControl : UserControl
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private AccommodationViewModel accommodation;
        public AccommodationViewModel Accommodation
        {
            get => accommodation;
            set
            {
                if (value != accommodation)
                {
                    accommodation = value;
                    OnPropertyChanged();
                }
            }
        }

        public User LoggedInUser;

        public AccommodationUserControl(User user, AccommodationViewModel accommodationViewModel)
        {
            InitializeComponent();
            LoggedInUser = user;
            accommodation = accommodationViewModel;
            DataContext = this;
        }

        private void AddReservation_Button(object sender, RoutedEventArgs e)
        {
            GuestWindow.contentControl.Content = new AddReservationAccommodationUserControl(LoggedInUser,Accommodation);
        }
    }
}
