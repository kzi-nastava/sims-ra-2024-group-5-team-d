using BookingApp.Appl.UseCases;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.ViewModels;
using BookingApp.WPF.ViewModels.OwnerViewModels;
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

        public ShowDetailedReviewWindow(int ratingId)
        {
            InitializeComponent();
            DataContext = new DetailedOwnerRatingViewModel(ratingId);
            Loaded += ShowDetailedReviewWindow_Loaded;
        }

        private void ShowDetailedReviewWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Ok.Focus();
        }
        private void Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
