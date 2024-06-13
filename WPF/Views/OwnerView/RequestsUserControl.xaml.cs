using BookingApp.Domain.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.WPF.Views.OwnerView
{
    /// <summary>
    /// Interaction logic for RequestsUserControl.xaml
    /// </summary>
    public partial class RequestsUserControl : UserControl
    {
        public RequestsUserControl(User user)
        {
            DataContext = new RequestsViewModel(user);
            InitializeComponent();

            Loaded += RequestsUserControl_Loaded;
        }

        private void RequestsUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Requests.Focus();
        }
    }
}
