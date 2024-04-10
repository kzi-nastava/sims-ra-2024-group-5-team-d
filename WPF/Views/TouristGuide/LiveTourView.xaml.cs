using BookingApp.Domain.Models;
using BookingApp.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for LiveTourView.xaml
    /// </summary>
    public partial class LiveTourView : UserControl
    {
        public User LoggedInUser { get; set; }
        public TourRealisationViewModel tourRealisation{ get; set; }
        public TourViewModel tour { get; set; }

        public LiveTourView(User user, TourRealisationViewModel tourRealisationViewModel,TourViewModel tourViewModel)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;
            tourRealisation= tourRealisationViewModel;
            tour = tourViewModel;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            SideBar.contentControlW.Content = new ViewMoreTourToday(tour,LoggedInUser);
        }
    }
}
