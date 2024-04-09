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

            // Postavljanje prozora da bude proziran
            this.WindowStyle = WindowStyle.None;
            this.AllowsTransparency = true;
            this.Background = null;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            // Postavljanje logotipa i imena aplikacije
            // Ovde dodajte logotip i ime aplikacije

            // Postavljanje tajmera za kašnjenje
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Zaustavljanje tajmera
            timer.Stop();

            // Otvaranje glavnog prozora ili početka aplikacije
            LoginScreen loginScreen = new LoginScreen();
            loginScreen.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            loginScreen.Show();

            // Zatvaranje SplashScreen prozora
            this.Close();
        }
    }
}

