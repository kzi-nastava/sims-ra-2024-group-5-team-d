using BookingApp.Domain.Models;
using BookingApp.WPF.ViewModels;
using ceTe.DynamicPDF.Forms;
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

namespace BookingApp.WPF.Views.OwnerView
{
    /// <summary>
    /// Interaction logic for ForumReadMoreUserControl.xaml
    /// </summary>
    public partial class ForumReadMoreUserControl : UserControl
    {
        public ForumReadMoreUserControl(int forumId,User user)
        {
            InitializeComponent();
            DataContext = new ForumReadMoreViewModel(forumId, user);
            Loaded += ForumUserControl_Loaded;
        }

        private void ForumUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            AddComment.Focus();
        }
        private void ListView_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
            Scroller.ScrollToVerticalOffset(Scroller.VerticalOffset - e.Delta);
        }
    }
}
