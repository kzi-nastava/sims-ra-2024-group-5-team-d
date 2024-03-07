using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for NewTourForm.xaml
    /// </summary>
    public partial class NewTourForm : Window
    {
        private string tourName;
        public string TourName
        {
            get => tourName;
            set
            {
                if (value != tourName)
                {
                    tourName = value;
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
        private double duration;
        public double Duration
        {
            get => duration;
            set
            {
                if (value != duration)
                {
                    duration = value;
                    OnPropertyChanged();
                }
            }
        }
        private int capacity;
        public int Capacity
        {
            get => capacity;
            set
            {
                if (value != capacity)
                {
                    capacity = value;
                    OnPropertyChanged();
                }
            }
        }
        private int language;
        public int Language
        {
            get => language;
            set
            {
                if (value != language)
                {
                    language = value;
                    OnPropertyChanged();
                }
            }
        }
        private string description;
        public string Description
        {
            get => description;
            set
            {
                if (value != description)
                {
                    description = value;
                    OnPropertyChanged();
                }
            }
        }
        private DateTime dateTime;
        public DateTime DateTime
        {
            get=> dateTime;
            set
            {
                if(value != dateTime)
                {
                    dateTime = value;
                    OnPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public User LoggedInUser { get; set; }
        private readonly TourRepository _repository;
        public NewTourForm(User user)
        {
            InitializeComponent();
            _repository = new TourRepository();
            LoggedInUser = user;
            DataContext = this;

        }

        private void CreateNewTour_Click(object sender, RoutedEventArgs e)
        {
            string imagesPath = "putanja";
            Debug.WriteLine(tourName+locationId+language+"aa"+capacity);
            Tour newTour = new Tour(tourName, _repository.getLocationByLocationId(locationId), description, (LANGUAGE)Language, Capacity, Duration, imagesPath, LoggedInUser);
            Tour savedTour = _repository.SaveTour(newTour);
            TourRealisation newTourRealisation = new TourRealisation(DateTime,savedTour.Id, Capacity, LoggedInUser);
            TourRealisation savedTourRealisation = _repository.SaveTourRealisation(newTourRealisation);
            TouristGuideWindow.AllTours.Add(savedTour);
            if(savedTourRealisation.StartTime.Day == DateTime.Now.Day) 
            {
                TouristGuideWindow.ToursToday.Add(savedTour);
            }
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
