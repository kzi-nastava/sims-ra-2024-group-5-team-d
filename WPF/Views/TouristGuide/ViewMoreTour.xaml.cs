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
using Xceed.Wpf.Toolkit;

namespace BookingApp.WPF.Views.TouristGuide
{
    /// <summary>
    /// Interaction logic for ViewMoreTour.xaml
    /// </summary>
    public partial class ViewMoreTour : UserControl, INotifyPropertyChanged
    {
        public TourViewModel SelectedTour { get; set; }
        public ObservableCollection<TourRealisationViewModel> TourRealisations { get; set; }
        private readonly ITourRealisationRepository tourRealisationRepository;
        private TourRealisationService tourRealisationService;
        private User LoggedInUser { get; set; }

        private DateTime _newTourRealizationDateTime;
        public DateTime NewTourRealizationDateTime
        {
            get { return _newTourRealizationDateTime; }
            set
            {
                if (_newTourRealizationDateTime != value)
                {
                    _newTourRealizationDateTime = value;
                    OnPropertyChanged(nameof(NewTourRealizationDateTime));
                }
            }
        }

        public ViewMoreTour(TourViewModel selectedTour,User user)
        {
            InitializeComponent();
            DataContext = this;
            tourRealisationService = new TourRealisationService();
            TourRealisations = new ObservableCollection<TourRealisationViewModel>();
            SelectedTour = selectedTour;
            tourRealisationRepository = new TourRealisationRepository();
            tourRealisationRepository.GetTourRealisationsByTourId(SelectedTour.Id)
                .ForEach(t => { TourRealisations.Add(new TourRealisationViewModel(t.Id, t.StartTime, t.TourId, t.AvailableSeats, t.User)); });

            // Initialize NewTourRealizationDateTime with current date and time
            LoggedInUser = user;
            NewTourRealizationDateTime = DateTime.Now;
        }

        // Implement INotifyPropertyChanged interface
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Event handler for the "+" button click
        private void AddTourRealizationButton_Click(object sender, RoutedEventArgs e)
        {
            NewTourRealizationPopup.IsOpen = true;
        }

        // Event handler for the "Save" button click
        private void SaveNewTourRealization_Click(object sender, RoutedEventArgs e)
        {
            // Saving logic here using NewTourRealizationDateTime property
            DateTime selectedDateTime = NewTourRealizationDateTime;
            TourRealisation tourRealisation = new TourRealisation(selectedDateTime,SelectedTour.Id,SelectedTour.Capacity, LoggedInUser);
            tourRealisationRepository.SaveTourRealisation(tourRealisation);
            TourRealisationViewModel tourRealisationViewModel = new TourRealisationViewModel(tourRealisation.Id,selectedDateTime, SelectedTour.Id, SelectedTour.Capacity, LoggedInUser);
            TourRealisations.Add(tourRealisationViewModel);
            // For example, close the popup after saving
            NewTourRealizationPopup.IsOpen = false;
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            SideBar.contentControlW.Content = new AllToursWindow(LoggedInUser);
        }
        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            if (HelpPopup.IsOpen)
                HelpPopup.IsOpen = false;
            else
                HelpPopup.IsOpen = true;
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
