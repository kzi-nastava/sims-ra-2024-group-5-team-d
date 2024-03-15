using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
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
    /// Interaction logic for TourRealisationsForTourToday.xaml
    /// </summary>
    public partial class TourRealisationsForTourToday : Window
    {
        public int tourId { get; set; }
        public User LoggedInUser { get; set; }
        public static ObservableCollection<TourRealisation> Realisations { get; set; }
        public TourRepository _repository { get; set; }
        public TourRealisation SelectedRealisation { get; set; }

        public TourRealisationsForTourToday(int selectedTourId,User loggedInUser)
        {
            InitializeComponent();
            DataContext = this;
            this.tourId = selectedTourId;
            this.LoggedInUser = loggedInUser;
            _repository = new TourRepository();
            Realisations = new ObservableCollection<TourRealisation>(_repository.GetTourRealisationsForToday(tourId));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string startTime;

        public string StartTime
        {
            get { return startTime; }
            set
            {
                if (startTime != value)
                {
                    startTime = value;
                    OnPropertyChanged(nameof(StartTime));
                }
            }
        }
        private CheckPointWindow currentCheckPointWindow;
        private void StartTour_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedRealisation != null)
            {
                    currentCheckPointWindow = new CheckPointWindow(tourId,SelectedRealisation.Id, LoggedInUser);
                    currentCheckPointWindow.Show();
            }
            else
            {
                // If open, just activate the window
                currentCheckPointWindow.Activate();
            }
        }
    }
}
