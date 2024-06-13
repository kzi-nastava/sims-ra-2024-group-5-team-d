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

namespace BookingApp.WPF.Views.TouristView
{
    /// <summary>
    /// Interaction logic for PastToursUserControl.xaml
    /// </summary>
    public partial class PastToursUserControl : UserControl
    {
        public User User { get; set; }
        public PastToursUserControl(User user)
        {
            InitializeComponent();
            User = user;
            DataContext = new PastToursViewModel(user);
        }
    }
}
