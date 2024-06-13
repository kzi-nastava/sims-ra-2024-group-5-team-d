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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.WPF.Views.OwnerView
{
    /// <summary>
    /// Interaction logic for SmallMenuUserControl.xaml
    /// </summary>
    public partial class SmallMenuUserControl : UserControl
    {
        public SmallMenuUserControl(User user)
        {
            InitializeComponent();
            DataContext =new MenuViewModel(user);
            Loaded += SmallMenuUserControl_Loaded;
        }

        private void SmallMenuUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Requests.Focus();
        }
    }
}
