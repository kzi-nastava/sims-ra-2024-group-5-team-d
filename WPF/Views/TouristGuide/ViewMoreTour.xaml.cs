using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using BookingApp.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace BookingApp.WPF.Views.TouristGuide
{
    /// <summary>
    /// Interaction logic for ViewMoreTour.xaml
    /// </summary>
    public partial class ViewMoreTour : UserControl
    {
        public TourViewModel SelectedTour { get; set; }
        public ObservableCollection<TourRealisationViewModel> TourRealisations { get; set; }
        private readonly ITourRealisationRepository tourRealisationRepository;
        private TourRealisationService tourRealisationService;
        public ViewMoreTour(TourViewModel selectedTour)
        {
            InitializeComponent();
            DataContext = this;
            tourRealisationService = new TourRealisationService();
            TourRealisations = new ObservableCollection<TourRealisationViewModel>();
            SelectedTour = selectedTour;
            tourRealisationRepository = new TourRealisationRepository();
            tourRealisationRepository.GetTourRealisationsByTourId(SelectedTour.Id).ForEach(t => { TourRealisations.Add(new TourRealisationViewModel(t.Id, t.StartTime, t.TourId, t.AvailableSeats, t.User)); });

        }
        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            if (HelpPopup.IsOpen)
                HelpPopup.IsOpen = false;
            else
                HelpPopup.IsOpen = true;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            SideBar.contentControlW.Content = new AllToursWindow();
        }
        private void AddButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
