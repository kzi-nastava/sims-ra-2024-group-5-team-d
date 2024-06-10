using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.ViewModels.OwnerViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    /// Interaction logic for UnratedGuestsUserControl.xaml
    /// </summary>
    public partial class UnratedGuestsUserControl : UserControl
    {
        public UnratedGuestsUserControl(User user)
        {     
            InitializeComponent();
            DataContext = new UnratedGuestsViewModel(user);
            Loaded += UnratedGuestsUserControl_Loaded;
        }

        private void UnratedGuestsUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Guests.Focus();
        }
    }
}
