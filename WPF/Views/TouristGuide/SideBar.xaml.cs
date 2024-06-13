using BookingApp.Domain.Models;
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

namespace BookingApp.WPF.Views.TouristGuide
{
    /// <summary>
    /// Interaction logic for SideBar.xaml
    /// </summary>
    public partial class SideBar : Window
    {
        public User LoggedInUser { get; set; }
        public static ContentControl contentControlW;
        public SideBar(User user)
        {
            InitializeComponent();
            DataContext = this;
            contentControlW = contentControl;
            LoggedInUser = user;
            contentControlW.Content = new AllToursWindow(LoggedInUser);
        }
        public void Home_Click(object sender, RoutedEventArgs e)
        {
            contentControlW.Content = new AllToursWindow(LoggedInUser);
        }

        private void NewTour_Click(object sender, MouseButtonEventArgs e)
        {
            contentControlW.Content = new CreateNewTourForm(LoggedInUser);
        }

        private void Profile_Click(object sender, MouseButtonEventArgs e)
        {
            contentControlW.Content = new Profile(LoggedInUser,this);
        }
        private void LogOut_Click(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
    }
}
