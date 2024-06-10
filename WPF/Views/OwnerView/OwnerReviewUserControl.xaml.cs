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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.WPF.Views.OwnerView
{
    /// <summary>
    /// Interaction logic for OwnerReviewUserControl.xaml
    /// </summary>
    public partial class OwnerReviewUserControl : UserControl
    {
        public OwnerReviewUserControl(User user)
        {
            DataContext=new OwnerReviewViewModel(user);
            InitializeComponent();
            Loaded += OwnerReviewUserControl_Loaded;
        }

        private void OwnerReviewUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Rate.Focus();
        }
    }
}
