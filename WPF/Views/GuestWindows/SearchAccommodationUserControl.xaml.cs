using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Repositories;
using BookingApp.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for SearchAccommodationUserControl.xaml
    /// </summary>
    public partial class SearchAccommodationUserControl : UserControl
    {
        private string accommodationName = "";
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
        public ObservableCollection<AccommodationViewModel> Accommodations { get; set; }
        public User LoggedInUser { get; set; }
        private readonly AccommodationRepository _repository;

        private readonly SearchAccommodationService SearchService;
        private readonly ContentControl contentControl;
        public AccommodationViewModel SelectedAccommodation { get; set; }
        List<Accommodation> accommodations;
        public SearchAccommodationUserControl(User user, ContentControl contentControl)
        {
            InitializeComponent();
            LoggedInUser = user;
            DataContext = this;
            SearchService = new SearchAccommodationService();
            _repository = new AccommodationRepository();
            Accommodations = new ObservableCollection<AccommodationViewModel>();
            accommodations = _repository.GetAll();
            accommodations.Sort((x, y) => y.IsSuperOwner.CompareTo(x.IsSuperOwner));
            accommodations.ForEach(a =>Accommodations.Add(new AccommodationViewModel(a.Id,a.Name,a.Location,a.Type,a.ImagesPath,a.MinStay,a.Capacity)));
            this.contentControl = contentControl;
        }
        private void SearchAccommodation(object sender, RoutedEventArgs e)
        {
            Accommodations.Clear();
            accommodations = SearchService.GetSearchedAccommodation(accommodationName, accommodationType, locationId, numberOfPeople, numberOfDays);
            accommodations.Sort((x, y) => y.IsSuperOwner.CompareTo(x.IsSuperOwner));
            accommodations.ForEach(fA => Accommodations.Add(new AccommodationViewModel(fA.Id,fA.Name,fA.Location,fA.Type,fA.ImagesPath,fA.MinStay, fA.Capacity)));

        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SelectedAccommodation!=null)
            { 

                GuestWindow.contentControl.Content = new AccommodationUserControl(LoggedInUser, SelectedAccommodation);
            }
        }
    }
}
