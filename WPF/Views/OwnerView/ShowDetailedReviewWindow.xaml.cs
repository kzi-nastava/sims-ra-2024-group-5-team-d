using BookingApp.Appl.UseCases;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.ViewModels;
using System;
using System.Collections.Generic;
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

namespace BookingApp.WPF.Views.OwnerView
{
    /// <summary>
    /// Interaction logic for ShowDetailedReviewWindow.xaml
    /// </summary>
    public partial class ShowDetailedReviewWindow : Window
    {
        private IAccommodationRatingRepository accommodationRatingRepository;

        public ShowDetailedReviewWindow(int ratingId)
        {
            InitializeComponent();
            accommodationRatingRepository = Injector.CreateInstance<IAccommodationRatingRepository>();
            DataContext = new OwnerRatingViewModel(accommodationRatingRepository.GetById(ratingId));
        }
        private void Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
