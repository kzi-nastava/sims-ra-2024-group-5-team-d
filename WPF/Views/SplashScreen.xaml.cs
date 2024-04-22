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
using System.Windows.Threading;

namespace BookingApp.WPF.Views
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        private DispatcherTimer timer;

        public SplashScreen()
        {
            InitializeComponent();

            this.WindowStyle = WindowStyle.None;
            this.AllowsTransparency = true;
            this.Background = null;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;


            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(4);//TREBA 5 ili 4 sekunde
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();

            LoginScreen loginScreen = new LoginScreen();
            loginScreen.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            loginScreen.Show();

            this.Close();
        }
    }
}

