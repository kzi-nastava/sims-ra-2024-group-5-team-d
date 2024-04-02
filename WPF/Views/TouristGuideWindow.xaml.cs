using BookingApp.Domain.Models;
using BookingApp.Repositories;
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

namespace BookingApp.WPF.Views
{
    /// <summary>
    /// Interaction logic for TouristGuideWindow.xaml
    /// </summary>
    public partial class TouristGuideWindow : Window
    {
        public static ObservableCollection<Tour> AllTours { get; set; }
        public static ObservableCollection<Tour> ToursToday { get; set; }
        public Tour SelectedTour { get; set; }
        public User LoggedInUser { get; set; }

        private readonly TourRepository _repository;

        public TouristGuideWindow(User user)
        {
            InitializeComponent();
            LoggedInUser = user;
            DataContext = this;
            _repository = new TourRepository();
            AllTours = new ObservableCollection<Tour>(_repository.GetByUserTours(LoggedInUser));
            ToursToday = new ObservableCollection<Tour>(_repository.GetToursForToday());
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void CreateNewTourWindow(object sender, RoutedEventArgs e)
        {
            NewTourForm createNewTourForm = new NewTourForm(LoggedInUser);
            createNewTourForm.ShowDialog();
        }
        private void CreateNewTourRealisationWindow(object sender, RoutedEventArgs e)
        {
            if (SelectedTour != null)
            {
                NewTourRealisationForm createNewTourRealisationForm = new NewTourRealisationForm(LoggedInUser, SelectedTour);
                createNewTourRealisationForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select a tour","Error",MessageBoxButton.OK);
            }
        }

        private void Tour_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SelectedTour != null)
            {
                string tourId = GetSelectedTourId();
                TourRealisationsForTourToday tourRealisationWindow = new TourRealisationsForTourToday(SelectedTour.Id,LoggedInUser);
                tourRealisationWindow.ShowDialog();
            }

        }

        private string GetSelectedTourId()
        {
            // Assuming you're using a DataGrid named "toursTodayDataGrid" for the Tours Today tab
            if (toursTodayDataGrid.SelectedItem != null)
            {
                // Assuming your tour object has a property named "Id"
                var selectedTour = toursTodayDataGrid.SelectedItem as Tour;
                if (selectedTour != null)
                {
                    return selectedTour.Id.ToString();
                }
            }

            // Return null if no tour is selected
            return null;
        }
    }
}
