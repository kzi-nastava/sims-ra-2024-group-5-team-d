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
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.WPF.Views.TouristView
{
    /// <summary>
    /// Interaction logic for BookingSection.xaml
    /// </summary>
    public partial class BookingSection : UserControl
    {
        public User User;
        public BookingSection(TourViewModel selectedTour, TourRealisationViewModel selectedRealisation, User user, int numberOfSeats)
        {
            InitializeComponent();
            User = user;
            DataContext = new BookingSectionViewModel(selectedRealisation, selectedTour, user, numberOfSeats);
        }

        private void ListView_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
            Scroller.ScrollToVerticalOffset(Scroller.VerticalOffset - e.Delta);
        }
    }
}
