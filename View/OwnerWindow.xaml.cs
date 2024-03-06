using BookingApp.Model;
using BookingApp.Repository;
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
using System.Xml.Linq;

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for OwnerWindow.xaml
    /// </summary>
    public partial class OwnerWindow : Window
    {
        public static ObservableCollection<Accommodation> Accommodations { get; set; }
        public User LoggedInUser { get; set; }
        private readonly AccommodationRepository _repository;


        public OwnerWindow(User user)
        {
            InitializeComponent();
            LoggedInUser = user;
            DataContext = this;
            _repository =new AccommodationRepository();
            Accommodations = new ObservableCollection<Accommodation>(_repository.GetByUser(user));

        }

        private void RegisterPropertyButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterAccommodationWindow registerAccommodationWindow = new RegisterAccommodationWindow(LoggedInUser);
            registerAccommodationWindow.ShowDialog();
        }
    }
}
