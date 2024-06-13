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
    /// Interaction logic for OwnerNotificationsUserControl.xaml
    /// </summary>
    public partial class OwnerNotificationsUserControl : UserControl
    {
        public OwnerNotificationsUserControl(User user)
        {
            InitializeComponent();
            DataContext = new NotificationsViewModel(user);
            Loaded += OwnerNotificationsUserControl_Loaded;
        }

        private void OwnerNotificationsUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Notifications.Focus();
        }
    }
}
