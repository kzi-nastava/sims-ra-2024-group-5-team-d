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
    /// Interaction logic for SmallMenuUserControl.xaml
    /// </summary>
    public partial class SmallMenuUserControl : UserControl
    {
        public SmallMenuUserControl()
        {
            InitializeComponent();
        }
        private void LeftMenu(object sender, MouseButtonEventArgs e)
        {
            if (sender is Grid clickedGrid)
            {
                // Pronalaženje pozicije na kojoj je kliknut
                Point clickPoint = e.GetPosition(clickedGrid);

                // Pronalaženje reda na kojem je kliknut
                int row = -1;
                double accumulatedHeight = 0.0;
                foreach (var rowDefinition in clickedGrid.RowDefinitions)
                {
                    accumulatedHeight += rowDefinition.ActualHeight;
                    if (accumulatedHeight >= clickPoint.Y)
                    {
                        row = clickedGrid.RowDefinitions.IndexOf(rowDefinition);
                        break;
                    }
                }
                if (row != -1)
                {/*
                    if (row == 0)
                       OwnerMainWindow.contentControl.Content = new RequestsNoMenu();
                    if (row == 1)
                        OwnerMainWindow.contentControl.Content = new Renovations();
                    if (row == 2)
                        OwnerMainWindow.contentControl.Content = new ReviewsNoMenu();
                    if (row == 3)
                        OwnerMainWindow.contentControl.Content = new ForumNoMenu();*/
                }
                // Ako je pronađen red, prikazujemo njegov indeks


            }
        }
    }
}
