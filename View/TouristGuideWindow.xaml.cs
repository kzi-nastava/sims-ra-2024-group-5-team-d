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

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for TouristGuideWindow.xaml
    /// </summary>
    public partial class TouristGuideWindow : Window
    {
        public static ObservableCollection<Tour> AllTours { get; set; }
        public static ObservableCollection<Tour> ToursToday { get; set; }
        public User LoggedInUser { get; set; }

        private readonly TourRepository _repository;
        public TouristGuideWindow(User user)
        {
            InitializeComponent();
            LoggedInUser = user;
            DataContext = this;
            _repository = new TourRepository();
            AllTours = new ObservableCollection<Tour>(_repository.GetByUserTours(LoggedInUser));
            ToursToday = new ObservableCollection<Tour>();
            GetToursForToday();
        }

        private void GetToursForToday()
        {
            List<TourRealisation> tourRealisations = new List<TourRealisation>();
            foreach(Tour t in AllTours)
            {
                foreach(TourRealisation tR in _repository.GetTourRealisationsByTourId(t.Id))
                {
                    if(tR.StartTime.Day == DateTime.Now.Day)
                    {
                        ToursToday.Add(t);
                    }
                }
                
            }
        }

        private void CreateNewTourWindow(object sender, RoutedEventArgs e)
        {
            NewTourForm createNewTourForm = new NewTourForm(LoggedInUser);
            createNewTourForm.ShowDialog();
        }
    }
}
