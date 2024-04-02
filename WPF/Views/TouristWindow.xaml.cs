using BookingApp.Domain.Models;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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
    /// Interaction logic for TouristWindow.xaml
    /// </summary>
    public partial class TouristWindow : Window
    {
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

        private string pickedMaxCapacity = "1";

        public string PickedMaxCapacity
        {
            get => pickedMaxCapacity;
            set
            {
                if (value != pickedMaxCapacity)
                {
                    pickedMaxCapacity = value;
                    OnPropertyChanged();
                }
            }
        }
        public Tour SelectedTour { get; set; }
        public ObservableCollection<Tour> Tours { get; set; }

        private readonly TourRepository _tourRepository;

        public TouristWindow()
        {
            InitializeComponent();
            _tourRepository = new TourRepository();
            Tours = new ObservableCollection<Tour>(_tourRepository.GetAllTours());
            //LoadToursFromCSV();
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

       
        private void tourList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(SelectedTour != null)
            {
                TourRealisationsForSelectedTour realisationsWindow = new TourRealisationsForSelectedTour(SelectedTour);
                realisationsWindow.Owner = this;
                realisationsWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                realisationsWindow.ShowDialog();
            }
            
        }

        private void SearchTours_Click(object sender, RoutedEventArgs e)
        {
            Tours.Clear();

            foreach (Tour tour in _tourRepository.GetAllTours())
            {
                bool languageMatch = pickedLanguage == 3 || tour.Language == (LANGUAGE)pickedLanguage;
                bool durationMatch = pickedDuration == 5 || (tour.Duration >= pickedDuration && tour.Duration <= pickedDuration + 1);
                bool locationMatch = pickedLocationId == 10 || tour.Location.Id == pickedLocationId;
                bool capacityMatch = tour.MaxCapacity >= Convert.ToDouble(pickedMaxCapacity);

                if (languageMatch && durationMatch && locationMatch && capacityMatch)
                {
                    Tours.Add(tour);
                }
            }

        }

    }

}
