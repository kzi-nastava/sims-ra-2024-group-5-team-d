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
    /// Interaction logic for ToursTodayWindow.xaml
    /// </summary>
    public partial class ToursTodayWindow : UserControl
    {
        public TourViewModel SelectedTour { get; set; }
        public ObservableCollection<TourViewModel> ToursToday { get; set; }
        private readonly ITourRepository tourRepository;
        private TourService tourService;
        private User LoggedInUser { get; set; }
        public ToursTodayWindow(User user)
        {
            InitializeComponent();
            collapseGrid.Visibility = Visibility.Collapsed;
            DataContext = this;
            tourService = new TourService();
            ToursToday = new ObservableCollection<TourViewModel>();
            tourService.GetToursForToday().ForEach(tour => ToursToday.Add(new TourViewModel(tour.Id, tour.Name, tour.Description, tour.Location, tour.Duration, tour.ImagesPath, tour.MaxCapacity, tour.Language, tour.User)));
            LoggedInUser = user;
        }
        private void AllToursTab_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SideBar.contentControlW.Content = new AllToursWindow(LoggedInUser);
        }
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void ViewMore_Click(object sender, RoutedEventArgs e)
        {
            SideBar.contentControlW.Content = new ViewMoreTourToday(SelectedTour, LoggedInUser);

        }
        private void ToursTodayTab_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SideBar.contentControlW.Content = new ToursTodayWindow(LoggedInUser);
        }
        private void RequestsTab_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //SideBar.contentControlW.Content = new RequestsWindow();
        }

        private void FinishedToursTab_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SideBar.contentControlW.Content = new FinishedToursWindow(LoggedInUser);
        }
    }
}
