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

namespace BookingApp.WPF.Views
{
    /// <summary>
    /// Interaction logic for CreateAnAccountUserControl.xaml
    /// </summary>
    public partial class CreateAnAccountUserControl : UserControl
    {
        public CreateAnAccountUserControl(string avatarPath)
        {
            InitializeComponent();
            DataContext= new UserRegistrationViewModel(avatarPath);
        }

        private void LogIn(object sender, MouseButtonEventArgs e)
        {
            LoginScreen.contentControl.Content = new LoginUserControl();
        }
    }
}
