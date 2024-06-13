using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Repositories;
using BookingApp.WPF.ViewModels;
using BookingApp.WPF.ViewModels.GuestViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace BookingApp.WPF.Views.GuestWindows
{
    /// <summary>
    /// Interaction logic for AccommodationAnytimeAnywhereUserControl.xaml
    /// </summary>
    public partial class AccommodationAnytimeAnywhereUserControl : UserControl
    {
        public AccommodationAnytimeAnywhereUserControl(User user, AccommodationViewModel selectedAccommmodation, DateTime fromDate, DateTime toDate, int numberOfPeople, int numberOfDays)
        {
            InitializeComponent();
            DataContext = new AnytimeAnywhereViewModel(user, selectedAccommmodation, fromDate, toDate, numberOfPeople, numberOfDays); 

        }

    }
}
