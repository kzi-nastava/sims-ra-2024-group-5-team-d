using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
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

namespace BookingApp.WPF.Views.TouristView
{
    /// <summary>
    /// Interaction logic for TouristHomeUserControl.xaml
    /// </summary>
    public partial class TouristHomeUserControl : UserControl, INotifyPropertyChanged
    {
        public TourViewModel SelectedTour { get; set; }
        public ObservableCollection<TourViewModel> Tours { get; set; } 
        private TourService tourService { get; set; }

        public User User { get; set; }

        public TouristHomeUserControl(User user)
        {
            InitializeComponent();
            tourService = new TourService();
            DataContext = this;
            User = user;
            Tours = new ObservableCollection<TourViewModel>();
            tourService.GetAllTours().ForEach(tour => Tours.Add(new TourViewModel(tour.Id, tour.Name, tour.Description, tour.Location, tour.Duration, tour.ImagesPath, tour.MaxCapacity, tour.Language, tour.User)));
            var sortedTours = Tours.OrderByDescending(tour => tour.IsSuperGuide).ToList();
            Tours.Clear();
            foreach (var tour in sortedTours)
            {
                Tours.Add(tour);
            }
        }

        private int pickedLocationId = 10;

        public int PickedLocationId
        {
            get => pickedLocationId;
            set
            {
                if (value != pickedLocationId)
                {
                    pickedLocationId = value;
                    OnPropertyChanged();
                }
            }
        }

        private double pickedDuration = 5;

        public double PickedDuration
        {
            get => pickedDuration;
            set
            {
                if (value != pickedDuration)
                {
                    pickedDuration = value;
                    OnPropertyChanged();
                }
            }
        }

        private int pickedLanguage = 3;

        public int PickedLanguage
        {
            get => pickedLanguage;
            set
            {
                if (value != pickedLanguage)
                {
                    pickedLanguage = value;
                    OnPropertyChanged();
                }
            }
        }

        private int pickedMaxCapacity = 1;

        public int PickedMaxCapacity
        {
            get => pickedMaxCapacity;
            set
            {
                if (value != pickedMaxCapacity)
                {
                    pickedMaxCapacity = value;
                    OnPropertyChanged(nameof(PickedMaxCapacity));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        
        private void IncreaseCount_Click(object sender, RoutedEventArgs e)
        {
            PickedMaxCapacity = PickedMaxCapacity + 1;
            PeopleCounter.Text = PickedMaxCapacity.ToString();
        }

        private void DecreaseCount_Click(object sender, RoutedEventArgs e)
        {
            if(PickedMaxCapacity > 1)
                PickedMaxCapacity = PickedMaxCapacity - 1;
            PeopleCounter.Text = PickedMaxCapacity.ToString();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            Tours.Clear();

            foreach (Tour tour in tourService.GetAllTours())
            {
                bool languageMatch = pickedLanguage == 3 || tour.Language == (LANGUAGE)pickedLanguage;
                bool durationMatch = pickedDuration == 5 || (tour.Duration >= pickedDuration && tour.Duration <= pickedDuration + 1);
                bool locationMatch = pickedLocationId == 10 || tour.Location.Id == pickedLocationId;
                bool capacityMatch = tour.MaxCapacity >= Convert.ToDouble(pickedMaxCapacity);

                if (languageMatch && durationMatch && locationMatch && capacityMatch)
                {
                    Tours.Add(new TourViewModel(tour.Id, tour.Name, tour.Description, tour.Duration, tour.ImagesPath, tour.Location, tour.User));
                }
            }
        }

        private void TourDetails_Click(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedTour != null)
            {
                TouristHomeWindow.contentControl.Content = new TourDetailsUserControl(SelectedTour, PickedMaxCapacity, User);
            }
        }
    }
}
