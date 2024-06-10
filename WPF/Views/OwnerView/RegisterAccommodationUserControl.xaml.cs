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
    /// Interaction logic for RegisterAccommodationUserControl.xaml
    /// </summary>
    public partial class RegisterAccommodationUserControl : UserControl
    {
        public RegisterAccommodationUserControl(User user)
        {
            InitializeComponent();
            DataContext=new RegisterAccommodationViewModel(user);
            Loaded += RegisterAccommodationUserControl_Loaded;
        }

        private void RegisterAccommodationUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Name.Focus();
        }
    }
}
