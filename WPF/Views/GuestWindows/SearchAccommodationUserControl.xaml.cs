using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Repositories;
using BookingApp.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
    public partial class SearchAccommodationUserControl : UserControl,INotifyPropertyChanged
    {
        private DateTime fromDate = DateTime.UtcNow;
        public DateTime FromDate
        {
            get { return fromDate; }
            set
            {
                fromDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime toDate = DateTime.UtcNow;
        public DateTime ToDate
        {
            get { return toDate; }
            set
            {
                toDate = value;
                OnPropertyChanged();
            }
        }
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
        private int locationId = 10;
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
        private int accommodationType = 3;
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
        private readonly AccommodationService accommodationService;
        private readonly SuperGuestService superGuestService;
        private readonly NotifierService notifierService;
        private readonly UserService userService;
        private readonly SearchAccommodationService SearchService;
        private AccommodationRatingService accommodationRatingService;
        public AccommodationViewModel SelectedAccommodation { get; set; }
        List<Accommodation> accommodations;
        public SearchAccommodationUserControl(User user)
        {
            InitializeComponent();
            LoggedInUser = user;
            DataContext = this;
            SearchService = new SearchAccommodationService();
            accommodationService = new AccommodationService();
            accommodationRatingService = new AccommodationRatingService();
            Accommodations = new ObservableCollection<AccommodationViewModel>();
            accommodations = accommodationService.GetAll();
            SortAccommodation();
            accommodations.ForEach(a =>Accommodations.Add(new AccommodationViewModel(a.Id,a.Name,a.Location,a.Type,a.ImagesPath,a.MinStay,a.Capacity, a.Owner.IsSuperUser, a.AverageRating, accommodationRatingService.GetNumberOfRatingsForAccommodation(a))));

            superGuestService = new SuperGuestService();
            notifierService = new NotifierService();

            bool IsUpdated = superGuestService.UpdateUserStatus(LoggedInUser);
            if(IsUpdated)
            {
                if(LoggedInUser.IsSuper())
                {
                    notifierService.ShowSuccess("Congratulations you have become super guest!");
                }
                else
                    notifierService.ShowWarning("You have been demoted from super guest!");
            }

        }
        private void SearchAccommodation(object sender, RoutedEventArgs e)
        {
            Accommodations.Clear();
            accommodations = SearchService.GetSearchedAccommodation(accommodationName, accommodationType, locationId, numberOfPeople, numberOfDays);
            SortAccommodation();
            accommodations.ForEach(accommodation => Accommodations.Add(new AccommodationViewModel(accommodation.Id, accommodation.Name, accommodation.Location, accommodation.Type, accommodation.ImagesPath, accommodation.MinStay, accommodation.Capacity, accommodation.Owner.IsSuperUser, accommodation.AverageRating, accommodationRatingService.GetNumberOfRatingsForAccommodation(accommodation))));

        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SelectedAccommodation!=null)
            { 

                GuestWindow.contentControl.Content = new AccommodationUserControl(LoggedInUser, SelectedAccommodation);
            }
        }
        private void ListViewAnytimeAnywhere_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SelectedAccommodation != null)
            {
                GuestWindow.contentControl.Content = new AccommodationAnytimeAnywhereUserControl(LoggedInUser, SelectedAccommodation, fromDate, toDate, NumberOfPeople, NumberOfDays);
            }
        }

        private void AnytimeAnywhereSearch_Click(object sender, RoutedEventArgs e)
        {
            Accommodations.Clear();
            accommodations = SearchService.AnytimeAnywhere(numberOfPeople, numberOfDays, FromDate, ToDate);
            SortAccommodation();
            accommodations.ForEach(accommodation => Accommodations.Add(new AccommodationViewModel(accommodation.Id, accommodation.Name, accommodation.Location, accommodation.Type, accommodation.ImagesPath, accommodation.MinStay, accommodation.Capacity, accommodation.Owner.IsSuperUser, accommodation.AverageRating, accommodationRatingService.GetNumberOfRatingsForAccommodation(accommodation))));
        }
        private void SortAccommodation()
        {
            accommodations.Sort((x, y) =>
            {
                if (x.Owner.IsSuperUser == true && y.Owner.IsSuperUser == false)
                {
                    return -1; 
                }
                else if (x.Owner.IsSuperUser == false && y.Owner.IsSuperUser == true)
                {
                    return 1; 
                }
                else
                {
                    return 0; 
                }
            });
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is TabItem)
            {
                ToDate = DateTime.UtcNow;
                FromDate = DateTime.UtcNow;
                NumberOfPeople = 1;
                NumberOfDays = 1;   
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            TextBox textBox = sender as TextBox;
            string newText = textBox.Text + e.Text;

            if (textBox.Text.Length == 0 && e.Text == "0")
            {
                e.Handled = true; 
                return;
            }

            if (!int.TryParse(newText, out _))
            {
                textBox.BorderBrush = Brushes.Red; 
                e.Handled = true; 
                return;
            }

            textBox.ClearValue(TextBox.BorderBrushProperty);
        }
    }
}
