using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace BookingApp.WPF.Views
{
    /// <summary>
    /// Interaction logic for TouristHomeWindow.xaml
    /// </summary>
    public partial class TouristHomeWindow : Window
    {
        public static ContentControl contentControl;


       
        public TouristHomeWindow()
        {
            InitializeComponent();
            DataContext = this;
            contentControl = contentControl1;
            contentControl.Content = new TouristHomeUserControl();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            
        }
    }
}
