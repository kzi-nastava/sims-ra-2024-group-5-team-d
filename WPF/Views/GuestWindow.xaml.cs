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

namespace BookingApp.WPF.Views
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
        private int numberOfPeople = 1;
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

        private int numberOfDays = 1;
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

        private readonly SearchAccommodationService SearchService;

        public Accommodation SelectedAccommodation { get; set; }
        public GuestWindow(User user)
        {
            InitializeComponent();
            LoggedInUser = user;
            DataContext = this;
            SearchService = new SearchAccommodationService(); 
            _repository = new AccommodationRepository();
            Accommodations = new ObservableCollection<Accommodation>(_repository.GetAll());
        }

        private void SearchAccommodation(object sender, RoutedEventArgs e)
        {

            Accommodations.Clear();
            SearchService.GetSearchedAccommodation(accommodationName, accommodationType, locationId, numberOfPeople, numberOfDays)
                .ForEach(foundAccommodation=> Accommodations.Add(foundAccommodation));

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
