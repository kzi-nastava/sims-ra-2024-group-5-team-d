using BookingApp.Model;
using BookingApp.Repository;
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

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for GuestWindow.xaml
    /// </summary>
    public partial class GuestWindow : Window
    {
        private string accommodationName;
        public string AccommodationName
        {
            get => accommodationName;
            set
            {
                if (value != accommodationName)
                {
                    accommodationName = value;
                    OnPropertyChanged();
                }
            }
        }
        private int locationId = 0;
        public int LocationId
        {
            get => locationId;
            set
            {
                if (value != locationId)
                {
                    locationId = value;
                    OnPropertyChanged();
                }
            }
        }
        private int accommodationType = 0;
        public int AccommodationType
        {
            get => accommodationType;
            set
            {
                if (value != accommodationType)
                {
                    accommodationType = value;
                    OnPropertyChanged();
                }
            }
        }
        private int numberOfPeople;
        public int NumberOfPeople
        {
            get => numberOfPeople;
            set
            {
                if (value != numberOfPeople)
                {
                    numberOfPeople = value;
                    OnPropertyChanged();
                }
            }
        }

        private int numberOfDays;
        public int NumberOfDays
        {
            get => numberOfDays;
            set
            {
                if (value != numberOfDays)
                {
                    numberOfDays = value;
                    OnPropertyChanged();
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ObservableCollection<Accommodation> Accommodations { get; set; }
        public User LoggedInUser { get; set; }

        private readonly AccommodationRepository _repository;

        public Accommodation SelectedAccommodation { get; set; }
        public GuestWindow(User user)
        {
            InitializeComponent();
            LoggedInUser = user;
            DataContext = this;
            _repository = new AccommodationRepository();
            Accommodations = new ObservableCollection<Accommodation>(_repository.GetAll());
        }

        private void SearchAccommodation(object sender, RoutedEventArgs e)
        {

            Accommodations.Clear();
            _repository.GetAll().ForEach(accommodation =>
            {
                if(IsWantedAccommodation(accommodation))
                    Accommodations.Add(accommodation); 
            });
        
        }

        private bool IsWantedAccommodation(Accommodation accommodation)
        {
            bool isAccommodationNameContained = accommodation.Name.Contains(accommodationName);
            bool isAccommodationTypeValid = accommodation.Type == (TYPE)accommodationType;
            bool isLocationValid = accommodation.Location.Id == locationId;
            bool isNumberOfPeopleValid = accommodation.Capacity >= numberOfPeople;
            bool isNumberOfDaysValid = accommodation.MinStay <= numberOfDays;

            return isAccommodationNameContained && isAccommodationTypeValid && isLocationValid && isNumberOfPeopleValid && isNumberOfDaysValid;
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem != null)
            {

                ReservationsWindow reservationsWindow = new ReservationsWindow(LoggedInUser, SelectedAccommodation);
                reservationsWindow.Show();
            }
        }
    }
}
