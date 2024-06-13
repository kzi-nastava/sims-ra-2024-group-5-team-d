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
    /// Interaction logic for CreateComplexRequestUserControl.xaml
    /// </summary>
    public partial class CreateComplexRequestUserControl : UserControl
    {
        public CreateComplexRequestUserControl(User user, ObservableCollection<SimpleRequestViewModel> requests)
        {
            InitializeComponent();
            DataContext = new CreateComlexRequestViewModel(user, requests);
        }
    }
}
