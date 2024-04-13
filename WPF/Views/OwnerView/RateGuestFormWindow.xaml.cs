using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using BookingApp.WPF.ViewModels.OwnerViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for RateGuestFormWindow.xaml
    /// </summary>
    public partial class RateGuestFormWindow : Window
    {

        public RateGuestFormWindow(UnratedGuestViewModel selectedGuest)
        {
            DataContext = new RateGuestViewModel(selectedGuest);
            InitializeComponent();
        }
        private void Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void RateGuest_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
 
    }
}
