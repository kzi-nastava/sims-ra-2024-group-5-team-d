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
    /// Interaction logic for ViewMoreTourToday.xaml
    /// </summary>
    public partial class ViewMoreTourToday : UserControl
    {
        public TourViewModel SelectedTour { get; set; }
        public TourRealisationViewModel SelectedTourRealisation { get; set; }
        public ObservableCollection<TourRealisationViewModel> TourRealisationsToday { get; set; }
        private TourRealisationService tourRealisationService;
        private User LoggedInUser { get; set; }
        public ViewMoreTourToday(TourViewModel selectedTour,User user)
        {
            InitializeComponent();
            DataContext = this;
            tourRealisationService = new TourRealisationService();
            TourRealisationsToday = new ObservableCollection<TourRealisationViewModel>();
            SelectedTour = selectedTour;
            tourRealisationService.GetTourRealisationsForToday(SelectedTour.Id).ForEach(t =>  TourRealisationsToday.Add(new TourRealisationViewModel(t.Id, t.StartTime, t.TourId, t.AvailableSeats, t.IsCancellable(),t.User, t.IsFinished)) );
            LoggedInUser = user;
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
            SideBar.contentControlW.Content = new ToursTodayWindow(LoggedInUser);
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            TourRealisation realisation = tourRealisationService.GetTourRealisationById(SelectedTourRealisation.Id);
            realisation.IsLive = true;
            tourRealisationService.Update(realisation);
            SideBar.contentControlW.Content = new LiveTourView(LoggedInUser,SelectedTourRealisation, SelectedTour);
        }

    }
}
