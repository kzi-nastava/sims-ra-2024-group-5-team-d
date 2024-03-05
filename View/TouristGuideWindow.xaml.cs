using BookingApp.Model;
using BookingApp.Repository;
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

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for TouristGuideWindow.xaml
    /// </summary>
    public partial class TouristGuideWindow : Window
    {
        public User LoggedInUser { get; set; }

        private readonly TourRepository _repository;
        public TouristGuideWindow(User user)
        {
            InitializeComponent();
            LoggedInUser = user;
            _repository = new TourRepository();

        }

        private void CreateNewTour(object sender, RoutedEventArgs e)
        {
            NewTourForm createNewTourForm = new NewTourForm(LoggedInUser);
            createNewTourForm.Show();
        }
    }
}
