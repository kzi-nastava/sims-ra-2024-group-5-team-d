using BookingApp.Domain.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.WPF.Views.TouristGuide
{
    /// <summary>
    /// Interaction logic for ViewMoreFinishedTour.xaml
    /// </summary>
    public partial class ViewMoreFinishedTour : UserControl
    {
        public ViewMoreFinishedTour(TourViewModel tour, User user)
        {
            InitializeComponent();
            DataContext = new FinishedTourViewModel(tour,user);
        }
    }
}
