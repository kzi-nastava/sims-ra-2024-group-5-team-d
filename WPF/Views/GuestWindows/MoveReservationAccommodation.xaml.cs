using BookingApp.Domain.Models;
using BookingApp.WPF.ViewModels;
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
    /// Interaction logic for MoveReservationAccommodation.xaml
    /// </summary>
    public partial class MoveReservationAccommodation : Window
    {
        public MoveReservationAccommodation(User user ,int reservationId)
        {
            InitializeComponent();
            DataContext = new CreateRequestViewModel(user, reservationId, this);
        }

    }
}
