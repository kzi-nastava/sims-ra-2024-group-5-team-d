using BookingApp.Model;
using BookingApp.Repository;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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


        private List<string> imagesPath;

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
            NumberOfCheckpoints = 0;
            imagesPath = new List<string>();

        }
       
    private void CreateNewTour_Click(object sender, RoutedEventArgs e)
        {
            string folderPath = "../../../TourImages/Tour";
            folderPath = folderPath + _repository.NextIdForTour();
            Directory.CreateDirectory(folderPath);
            foreach (string imagePath in imagesPath)
            {
                string targetImagePath = System.IO.Path.Combine(folderPath, System.IO.Path.GetFileName(imagePath));
                File.Copy(imagePath, targetImagePath);
            }
            int tourId = 0;
            Tour newTour = new Tour(tourName, _repository.getLocationByLocationId(locationId), description, (LANGUAGE)Language, Capacity, Duration, folderPath, LoggedInUser);
            bool indicator = false;
            foreach (Tour tour in _repository.GetAllTours())
            {
                if(tour.Name == newTour.Name && tour.MaxCapacity == newTour.MaxCapacity && tour.Location.Id == newTour.Location.Id)
                {
                    indicator = true;
                    tourId = tour.Id;
                }
            }
            if (!indicator)
            {
                Tour savedTour = _repository.SaveTour(newTour);
                TourRealisation newTourRealisation = new TourRealisation(DateTime, savedTour.Id, Capacity, LoggedInUser);
                TourRealisation savedTourRealisation = _repository.SaveTourRealisation(newTourRealisation);
                TouristGuideWindow.AllTours.Add(newTour);
                if (savedTourRealisation.StartTime.Day == DateTime.Now.Day)
                {
                    TouristGuideWindow.ToursToday.Add(newTour);
                }
                Close();
            }
            else
            {
                TourRealisation newTourRealisation = new TourRealisation(DateTime, tourId, Capacity, LoggedInUser);
                TourRealisation savedTourRealisation = _repository.SaveTourRealisation(newTourRealisation);
                Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        
        private int _numberofCheckPoints;
        public int NumberOfCheckpoints
        {
            get => _numberofCheckPoints;
            set
            {
                if (value != _numberofCheckPoints)
                {
                    _numberofCheckPoints = value;
                    OnPropertyChanged(nameof(NumberOfCheckpoints));
                }
            }
        }
        private void IncreaseCount_Click(object sender, RoutedEventArgs e)
        {
            NumberOfCheckpoints++;
            CheckPointCounter.Text = NumberOfCheckpoints.ToString();
        }
        private void DecreaseCount_Click(object sender, RoutedEventArgs e)
        {
            if (NumberOfCheckpoints > 1)
            {
                NumberOfCheckpoints--;
            }
            CheckPointCounter.Text = NumberOfCheckpoints.ToString();

        }
        private void AddCheckpointButton_Click(object sender, RoutedEventArgs e)
        {
            // Create a new instance of the AddCheckpointsWindow

            // Get the number of checkpoints specified by the user
            int numberOfCheckpoints = int.Parse(CheckPointCounter.Text);
            AddCheckpointsWindow addCheckpointsWindow = new AddCheckpointsWindow(numberOfCheckpoints);

            // Show the window
            addCheckpointsWindow.ShowDialog();
        }

        private void UploadPictureButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                imagesPath.Add(filePath);
                Debug.WriteLine(filePath);
            }
        }
    }
}
