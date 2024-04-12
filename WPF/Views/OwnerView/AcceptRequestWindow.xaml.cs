using BookingApp.Domain.Models;
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
using BookingApp.WPF.ViewModels;

namespace BookingApp.WPF.Views.OwnerView
{
    /// <summary>
    /// Interaction logic for AcceptRequestWindow.xaml
    /// </summary>
    public partial class AcceptRequestWindow : Window
    {
        public AcceptRequestWindow(User user, RequestViewModel request)
        {
            DataContext=new ProcessRequestViewModel(user, request);
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
