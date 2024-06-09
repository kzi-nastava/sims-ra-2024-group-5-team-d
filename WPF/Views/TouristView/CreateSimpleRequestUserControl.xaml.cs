using BookingApp.Domain.Models;
using BookingApp.WPF.ViewModels.TourViewModels.TouristViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for CreateSimpleRequestUserControl.xaml
    /// </summary>
    public partial class CreateSimpleRequestUserControl : UserControl
    {
        public CreateSimpleRequestUserControl(User tourist, ObservableCollection<SimpleRequestViewModel> requests)
        {
            InitializeComponent();
            DataContext = new CreateSimpleRequestViewModel(tourist, requests);
        }

        public CreateSimpleRequestUserControl(int index, SimpleRequestViewModel request, ObservableCollection<SimpleRequestViewModel> requests)
        {
            InitializeComponent();
            DataContext = new CreateSimpleRequestViewModel(index, request, requests);
        }

        private void ListView_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
            Scroller.ScrollToVerticalOffset(Scroller.VerticalOffset - e.Delta);
        }
    }
}
