using BookingApp.Domain.Models;
using BookingApp.WPF.ViewModels;
using BookingApp.WPF.ViewModels.GuestViewModels;
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

namespace BookingApp.WPF.Views.GuestWindows
{
    /// <summary>
    /// Interaction logic for OwnerRateGuestWindow.xaml
    /// </summary>
    public partial class OwnerRateGuestWindow : Window
    {
        public OwnerRateGuestWindow(OwnerRatingGuestViewModel ownerRatingGuest)
        {
            InitializeComponent();
            DataContext = new OwnerRateGuestViewModel(ownerRatingGuest, this);
        }
    }
}
