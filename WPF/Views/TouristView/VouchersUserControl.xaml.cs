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
    /// Interaction logic for VouchersUserControl.xaml
    /// </summary>
    public partial class VouchersUserControl : UserControl
    {
        public VouchersUserControl(User user)
        {
            InitializeComponent();
            DataContext = new VouchersViewModel(user);
        }
    }
}
