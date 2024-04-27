using BookingApp.Domain.Models;
using BookingApp.WPF.ViewModels.TourViewModels.TourGuideViewModels;
using System;
using System.Collections.Generic;
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

namespace BookingApp.WPF.Views.TouristGuide
{
    /// <summary>
    /// Interaction logic for RequestsStatistics.xaml
    /// </summary>
    public partial class RequestsStatistics : UserControl
    {
        public static Popup HelpPopUp { get; set; }
        public RequestsStatistics(User user)
        {
            InitializeComponent();
            HelpPopUp = HelpPopup;
            DataContext = new RequestsStatisticsViewModel(user);
        }
    }
}
