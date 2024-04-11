using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
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
    /// Interaction logic for FinishedToursWindow.xaml
    /// </summary>
    public partial class FinishedToursWindow : UserControl
    {
        private User LoggedInUser { get; set; }
        public TourViewModel SelectedTour { get; set; }
        public ObservableCollection<TourViewModel> FinishedTours { get; set; }
        private readonly ITourRepository tourRepository;
        private TourService tourService;
        public FinishedToursWindow(User user)
        {
            InitializeComponent();
            collapseGrid.Visibility = Visibility.Collapsed;
            DataContext = this;
            LoggedInUser = user;
            tourService = new TourService();
            FinishedTours = new ObservableCollection<TourViewModel>();
            tourService.GetFinishedTours().ForEach(tour => FinishedTours.Add(new TourViewModel(tour.Id, tour.Name, tour.Description, tour.Location, tour.Duration, tour.ImagesPath, tour.MaxCapacity, tour.Language, tour.User)));
        }
        private void ToursTodayTab_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SideBar.contentControlW.Content = new ToursTodayWindow(LoggedInUser);
        }
        private void RequestsTab_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //SideBar.contentControlW.Content = new RequestsWindow();
        }
        private void AllToursTab_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SideBar.contentControlW.Content = new AllToursWindow(LoggedInUser);
        }
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void FinishedToursTab_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SideBar.contentControlW.Content = new FinishedToursWindow(LoggedInUser);
        }
        private void ViewMore_Click(object sender, RoutedEventArgs e)
        {
            SideBar.contentControlW.Content = new ViewMoreFinishedTour(SelectedTour, LoggedInUser);
        }
    }
}
