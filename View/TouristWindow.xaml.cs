using BookingApp.Model;
using BookingApp.Repository;
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

namespace BookingApp.View
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

        private void LoadToursFromCSV()
        {
            /*
            Tour tours = new Tour();
            foreach (Tour tour in tours.GetAllTours())
            {
                Tours.Add(tour);
            }*/ //BRISANJEE
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
            _tourRepository.GetAllTours().ForEach(tour => {
                if (pickedLanguage != 3 && pickedDuration != 5 && pickedLocationId != 10)
                {
                    if (tour.Location.Id == pickedLocationId
                        && tour.Language == (LANGUAGE)pickedLanguage
                        && tour.Duration <= pickedDuration + 1 && tour.Duration >= pickedDuration
                        && tour.MaxCapacity >= Convert.ToDouble(pickedMaxCapacity))
                    {
                        Tours.Add(tour);
                    }
                }
                else if (pickedLanguage == 3 && pickedDuration != 5 && pickedLocationId != 10)
                {
                    // Handle case where pickedLanguage is 4
                    // You may add specific logic for this case if needed

                    if (tour.Location.Id == pickedLocationId
                        && tour.Duration <= pickedDuration + 1 && tour.Duration >= pickedDuration
                        && tour.MaxCapacity >= Convert.ToDouble(pickedMaxCapacity))
                    {
                        Tours.Add(tour);
                    }

                }
                else if (pickedLanguage != 3 && pickedDuration == 5 && pickedLocationId != 10)
                {
                    // Handle case where pickedDuration is 5
                    // You may add specific logic for this case if needed

                    if (tour.Location.Id == pickedLocationId
                        && tour.Language == (LANGUAGE)pickedLanguage
                        && tour.MaxCapacity >= Convert.ToDouble(pickedMaxCapacity))
                    {
                        Tours.Add(tour);
                    }
                }
                else if (pickedLanguage != 3 && pickedDuration != 5 && pickedLocationId == 10)
                {
                    // Handle case where pickedLocationId is 10
                    // You may add specific logic for this case if needed

                    if (tour.Language == (LANGUAGE)pickedLanguage
                        && tour.Duration <= pickedDuration + 1 && tour.Duration >= pickedDuration
                        && tour.MaxCapacity >= Convert.ToDouble(pickedMaxCapacity))
                    {
                        Tours.Add(tour);
                    }
                }
                else if (pickedLanguage == 3 && pickedDuration == 5 && pickedLocationId != 10)
                {
                    // Handle case where pickedLanguage is 4 and pickedDuration is 5
                    // You may add specific logic for this case if needed

                    if (tour.Location.Id == pickedLocationId
                        && tour.MaxCapacity >= Convert.ToDouble(pickedMaxCapacity))
                    {
                        Tours.Add(tour);
                    }
                }
                else if (pickedLanguage == 3 && pickedDuration != 5 && pickedLocationId == 10)
                {
                    // Handle case where pickedLanguage is 4 and pickedLocationId is 10
                    // You may add specific logic for this case if needed

                    if (tour.Duration <= pickedDuration + 1 && tour.Duration >= pickedDuration
                        && tour.MaxCapacity >= Convert.ToDouble(pickedMaxCapacity))
                    {
                        Tours.Add(tour);
                    }
                }
                else if (pickedLanguage != 3 && pickedDuration == 5 && pickedLocationId == 10)
                {
                    // Handle case where pickedDuration is 5 and pickedLocationId is 10
                    // You may add specific logic for this case if needed

                    if (tour.Language == (LANGUAGE)pickedLanguage
                        && tour.MaxCapacity >= Convert.ToDouble(pickedMaxCapacity))
                    {
                        Tours.Add(tour);
                    }
                }
                else if (pickedLanguage == 3 && pickedDuration == 5 && pickedLocationId == 10)
                {
                    // Handle case where all three values are 4, 5, and 10 respectively
                    // You may add specific logic for this case if needed

                    if (tour.MaxCapacity >= Convert.ToDouble(pickedMaxCapacity))
                    {
                        Tours.Add(tour);
                    }
                }
            });


        }

    }

}
