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
    /// Interaction logic for AllToursWindow.xaml
    /// </summary>
    public partial class AllToursWindow : UserControl
    {
        public TourViewModel SelectedTour { get; set; }
        public ObservableCollection<TourViewModel> Tours { get; set; }
        private readonly ITourRepository tourRepository;
        private User LoggedInUser { get; set; }
        public AllToursWindow(User user)
        {
            InitializeComponent();
            collapseGrid.Visibility = Visibility.Collapsed;
            DataContext = this;
            tourRepository = Injector.CreateInstance<ITourRepository>();
            Tours = new ObservableCollection<TourViewModel>();
            tourRepository.GetAllTours().ForEach(tour => Tours.Add(new TourViewModel(tour.Id, tour.Name, tour.Description, tour.Location, tour.Duration, tour.ImagesPath,tour.MaxCapacity,tour.Language)));
            LoggedInUser = user;
        }
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ViewMore_Click(object sender, RoutedEventArgs e)
        {
            SideBar.contentControlW.Content = new ViewMoreTour(SelectedTour, LoggedInUser);

        }
        private void ToursTodayTab_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SideBar.contentControlW.Content = new ToursTodayWindow(LoggedInUser);
        }
        private void RequestsTab_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //SideBar.contentControlW.Content = new RequestsWindow(LoggedInUser);
        }

        private void FinishedToursTab_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SideBar.contentControlW.Content = new FinishedToursWindow(LoggedInUser);
        }
    }
}
