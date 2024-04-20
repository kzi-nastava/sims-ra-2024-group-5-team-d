using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using BookingApp.WPF.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.WPF.Views.OwnerView
{
    /// <summary>
    /// Interaction logic for OwnerMainWindowUserControl.xaml
    /// </summary>
    public partial class OwnerMainWindowUserControl : UserControl
    {
        private User loggedInUser;
        public OwnerMainWindowUserControl(User user)
        {
            InitializeComponent();
            loggedInUser = user;
            DataContext =new AccommodationsViewModel(user);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }



        private void ShowStats(object sender, RoutedEventArgs e)
        {
           // MainWindow.contentControl.Content = new StatsWindow();
        }
    }
}
