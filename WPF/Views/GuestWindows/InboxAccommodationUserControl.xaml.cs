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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.WPF.Views.GuestWindows
{
    /// <summary>
    /// Interaction logic for InboxAccommodationUserControl.xaml
    /// </summary>
    public partial class InboxAccommodationUserControl : UserControl
    {
        public InboxAccommodationUserControl(User user)
        {
            InitializeComponent();
            DataContext = new CreateInboxViewModel(user);
        }
    }
}
