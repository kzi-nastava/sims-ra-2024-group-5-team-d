using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BookingApp.WPF.Views.TouristView;
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
using BookingApp.Domain.Models;
using BookingApp.Appl.UseCases;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Windows.Media.Media3D;
using BookingApp.WPF.Views.OwnerView;

namespace BookingApp.WPF.Views.TouristView
{
    /// <summary>
    /// Interaction logic for TouristHomeWindow.xaml
    /// </summary>
    public partial class TouristHomeWindow : Window , INotifyPropertyChanged
    {
        public static ContentControl contentControl;

        User User { get; set; }

        private bool isLanguageChecked;
        public bool IsLanguageChecked
        {
            get => isLanguageChecked;
            set
            {
                if (value != isLanguageChecked)
                {
                    isLanguageChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool isThemeSwitchChecked;

        public bool IsThemeSwitchChecked
        {
            get => isThemeSwitchChecked;
            set
            {
                if (value != isThemeSwitchChecked)
                {
                    isThemeSwitchChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool isNotificationsClicked;

        public bool IsNotificationsClicked
        {
            get => isNotificationsClicked;
            set
            {
                if (value != isNotificationsClicked)
                {
                    isNotificationsClicked = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool isHomePageClicked;
        public bool IsHomePageClicked
        {
            get => isHomePageClicked;
            set
            {
                if (value != isHomePageClicked)
                {
                    isHomePageClicked = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool isRequestsClicked;

        public bool IsRequestsClicked
        {
            get => isRequestsClicked;
            set
            {
                if (value != isRequestsClicked)
                {
                    isRequestsClicked = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool isToursClicked;

        public bool IsToursClicked
        {
            get => isToursClicked;
            set
            {
                if (value != isToursClicked)
                {
                    isToursClicked = value;
                    OnPropertyChanged("IsToursClicked");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //private NotifierService notifier;

        public TouristHomeWindow(User user)
        {
            InitializeComponent();
            IsThemeSwitchChecked = false;
            IsLanguageChecked = true;
            IsNotificationsClicked = false;
            IsHomePageClicked = true;
            IsRequestsClicked = false;
            IsToursClicked = false; 
            DataContext = this;
            User = user;
            contentControl = contentControl1;
            contentControl.Content = new TouristHomeUserControl(user);
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //notifier = new NotifierService();
            //notifier.ShowInformation("You have been added to a tour!");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            contentControl.Content = new TouristHomeUserControl(User);
        }

        private void YourTours_Click(object sender, RoutedEventArgs e)
        {
            IsToursClicked = true;
            IsHomePageClicked = true;
            IsNotificationsClicked = true;
            IsRequestsClicked = true;
            IsHomePageClicked = false;
            IsNotificationsClicked = false;
            IsRequestsClicked = false;
            contentControl.Content = new YourToursUserControl(User);
        }

        private void Requests_Click(object sender, RoutedEventArgs e)
        {
            IsRequestsClicked = true; 
            IsNotificationsClicked = true;
            IsHomePageClicked = true;
            IsToursClicked = true;
            IsNotificationsClicked = false;
            IsHomePageClicked = false;
            IsToursClicked = false;
            contentControl.Content = new YourRequestsUserControl(User);
        }

        private void Notifications_Click(object sender, RoutedEventArgs e)
        {
            IsNotificationsClicked = true;
            IsRequestsClicked = true;
            IsHomePageClicked = true;
            IsToursClicked = true;
            IsRequestsClicked = false;
            IsHomePageClicked = false;
            IsToursClicked = false;
            contentControl.Content = new NotificationsUserControl(User);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            IsHomePageClicked = true;
            IsNotificationsClicked = true;
            IsRequestsClicked = true;
            IsToursClicked = true;
            IsNotificationsClicked = false;
            IsRequestsClicked = false;
            IsToursClicked=false;
            contentControl.Content = new TouristHomeUserControl(User);
        }

        private void Button_Click_1()
        {
            IsHomePageClicked = true;
            IsNotificationsClicked = true;
            IsRequestsClicked = true;
            IsToursClicked = true;
            IsNotificationsClicked = false;
            IsRequestsClicked = false;
            IsToursClicked = false;
            contentControl.Content = new TouristHomeUserControl(User);
        }
        private void ThemeSwitch_Click(object sender, RoutedEventArgs e)
        {

            Debug.WriteLine(IsThemeSwitchChecked);
            if(!IsThemeSwitchChecked)
            {
                AppTheme.ChangeTheme(new Uri("Themes/LightTheme.xaml", UriKind.Relative), new Uri("Themes/DarkTheme.xaml", UriKind.Relative));
            }
            else
            {
                AppTheme.ChangeTheme(new Uri("Themes/DarkTheme.xaml", UriKind.Relative), new Uri("Themes/LightTheme.xaml", UriKind.Relative));
            }

            OnPropertyChanged("IsToursClicked");
            OnPropertyChanged("IsNotificationsClicked");
            OnPropertyChanged("IsHomePageClicked");
            OnPropertyChanged("IsRequestsClicked");


        }

        private void SwitchLanguageClick(object sender, RoutedEventArgs e)
        {
            if (IsLanguageChecked)
            {
                AppLanguage.ChangeLanguage(new Uri("Languages/English.xaml", UriKind.Relative), new Uri("Languages/Serbian.xaml", UriKind.Relative));
            }
            else
            {
                AppLanguage.ChangeLanguage(new Uri("Languages/Serbian.xaml", UriKind.Relative), new Uri("Languages/English.xaml", UriKind.Relative));
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            TouristHomeWindow homeWindow = (TouristHomeWindow)Window.GetWindow(this);
            homeWindow.Close();
        }
    }
}
