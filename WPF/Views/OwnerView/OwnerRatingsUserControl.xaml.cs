using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.ViewModels.OwnerViewModels;
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

namespace BookingApp.WPF.Views.OwnerView
{
    /// <summary>
    /// Interaction logic for OwnerRatingsUserControl.xaml
    /// </summary>
    public partial class OwnerRatingsUserControl : UserControl
    {
        public ObservableCollection<OwnerRatingViewModel> OwnerRatings { get; set; }

        private AccommodationRatingService accommodationRatingService;
        public OwnerRatingViewModel SelectedOwnerRating { get; set; }
        public OwnerRatingsUserControl(User user)
        {
            InitializeComponent();
            accommodationRatingService = new AccommodationRatingService();
            OwnerRatings = new ObservableCollection<OwnerRatingViewModel>();
            accommodationRatingService.GetAllRatingsForOwner(user).ForEach(rating=>OwnerRatings.Add(new OwnerRatingViewModel(rating)));//NEKI KOORDINATORSKI SERVIS
            DataContext = this;
        }
        private void ShowDetails(object sender, RoutedEventArgs e)
        {
            if (SelectedOwnerRating != null)
            {
                ShowDetailedReviewWindow details = new ShowDetailedReviewWindow(SelectedOwnerRating.Id);
                Window window = Window.GetWindow(this);
                details.Owner = window;
                details.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                details.ShowDialog();
            }
        }
    }
}
