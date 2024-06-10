using System;
using System.Collections.Generic;
using System.IO;
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

namespace BookingApp.WPF.Views.OwnerView
{
    /// <summary>
    /// Interaction logic for SavePDFWindow.xaml
    /// </summary>
    public partial class SavePDFWindow : Window
    {
        string pdfPath;
        public SavePDFWindow(string pdfPath)
        {
            InitializeComponent();
            this.pdfPath = pdfPath;
            Loaded += SavePDFWindow_Loaded;
        }

        private void SavePDFWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Confirm.Focus();
        }

        private void SavePDF(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DeletePDF(object sender, RoutedEventArgs e)
        {
            File.Delete(pdfPath);
            Close();
        }
    }
}
