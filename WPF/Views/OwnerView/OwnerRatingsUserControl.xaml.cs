using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.ViewModels.OwnerViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
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
    /// Interaction logic for OwnerRatingsUserControl.xaml
    /// </summary>
    public partial class OwnerRatingsUserControl : UserControl
    {

        public OwnerRatingsUserControl(User user)
        {
            InitializeComponent();
            DataContext =new OwnerRatingsViewModel(user);
            Loaded += OwnerRatingsUserControl_Loaded;
        }

        private void OwnerRatingsUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Ratings.Focus();
        }

    }
}
