using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using BookingApp.WPF.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.WPF.Views.OwnerView
{
    /// <summary>
    /// Interaction logic for OwnerMainWindowUserControl.xaml
    /// </summary>
    public partial class OwnerMainWindowUserControl : UserControl
    {
        public static ObservableCollection<AccommodationViewModel> Accommodations { get; set; }
        private readonly IAccommodationRepository accommodationRepository;
        public OwnerMainWindowUserControl(User user)
        {
            accommodationRepository=Injector.CreateInstance<IAccommodationRepository>();
            InitializeComponent();
            Accommodations = new ObservableCollection<AccommodationViewModel>();
            accommodationRepository.GetByUser(user).ForEach(accommodation => Accommodations.Add(new AccommodationViewModel(accommodation.Id, accommodation.Name, accommodation.Location, accommodation.Type,accommodation.ImagesPath)));
            DataContext = this;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RenovateClick(object sender, RoutedEventArgs e)
        {
            /*RenovateDialogue renovate = new RenovateDialogue();
            Window parentWindow = Window.GetWindow(this);
            renovate.Owner = parentWindow;
            renovate.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            renovate.ShowDialog();*/

        }


        private void ShowStats(object sender, RoutedEventArgs e)
        {
           // MainWindow.contentControl.Content = new StatsWindow();
        }
    }
}
