using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BookingApp.WPF.Views.TouristView;
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
using BookingApp.Domain.Models;

namespace BookingApp.WPF.Views.TouristView
{
    /// <summary>
    /// Interaction logic for TouristHomeWindow.xaml
    /// </summary>
    public partial class TouristHomeWindow : Window
    {
        public static ContentControl contentControl;

        User User { get; set; }
       
        public TouristHomeWindow(User user)
        {
            InitializeComponent();
            DataContext = this;
            User = user;
            contentControl = contentControl1;
            contentControl.Content = new TouristHomeUserControl(user);
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            contentControl.Content = new TouristHomeUserControl(User);
        }

        private void YourTours_Click(object sender, RoutedEventArgs e)
        {
            contentControl.Content = new YourToursUserControl(User);
        }
    }
}
