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
using ToastNotifications.Core;
using ToastNotifications.Lifetime;

namespace BookingApp.WPF.Views.OwnerView
{
    /// <summary>
    /// Interaction logic for OwnerProfileUserControl.xaml
    /// </summary>
    public partial class OwnerProfileUserControl : UserControl
    {
        public OwnerProfileUserControl(User user)
        {
            InitializeComponent();
            DataContext=new OwnerProfileViewModel(user);
            Loaded += OwnerProfileUserControl_Loaded;
        }

        private void OwnerProfileUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Credentials.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OwnerMainWindow ownerMainWindow = (OwnerMainWindow)Window.GetWindow(this);
            ownerMainWindow.Close();
        }
    }
}
