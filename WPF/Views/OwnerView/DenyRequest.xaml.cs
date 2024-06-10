using BookingApp.Domain.Models;
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
    /// Interaction logic for DenyRequest.xaml
    /// </summary>
    public partial class DenyRequest : Window
    {
        public DenyRequest(User user, RequestViewModel request)
        {
            InitializeComponent();
            DataContext= new ProcessRequestViewModel(user, request);
            Loaded += DenyRequest_Loaded;
        }

        private void DenyRequest_Loaded(object sender, RoutedEventArgs e)
        {
            Reason.Focus();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
