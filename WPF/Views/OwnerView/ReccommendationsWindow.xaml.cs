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
    /// Interaction logic for ReccommendationsWindow.xaml
    /// </summary>
    public partial class ReccommendationsWindow : Window
    {
        public ReccommendationsWindow()
        {
            InitializeComponent();
            DataContext = new ReccommendationsViewModel();
        }
    }
}
