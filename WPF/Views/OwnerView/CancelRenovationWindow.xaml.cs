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
    /// Interaction logic for CancelRenovationWindow.xaml
    /// </summary>
    public partial class CancelRenovationWindow : Window
    {
        public CancelRenovationWindow(RenovationViewModel renovation)
        {

            InitializeComponent();
            DataContext = new CancelRenovationViewModel(renovation);
            Loaded += CancelRenovationWindow_Loaded;
        }

        private void CancelRenovationWindow_Loaded(object sender, RoutedEventArgs e)
        {
            No.Focus();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
