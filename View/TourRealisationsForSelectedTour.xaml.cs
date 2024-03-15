using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
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
    /// Interaction logic for TourRealisationsForSelectedTour.xaml
    /// </summary>
    public partial class TourRealisationsForSelectedTour : Window
    {
        public TourRealisation SelectedTourRealisation { get; set; }
        public ObservableCollection<TourRealisation> TourRealisations { get; set; }

        public int NumberOfPeople { get; set; }

        public string TourName { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TourRealisationsForSelectedTour(Tour pickedTour)
        {
            TourRealisations = new ObservableCollection<TourRealisation>();

            InitializeComponent();

            NumberOfPeople = 1;

            TourName = pickedTour.Name;

            PeopleCounter.Text = NumberOfPeople.ToString();

            GetTourRealisationsForSelectedTour(pickedTour);

            //TourRealisationsHeader.Content = "Future Tour Realisations (" + pickedTour.Name + ")";
            DataContext = this;
        }

        private void GetTourRealisationsForSelectedTour(Tour pickedTour)
        {
            TourRepository _tours = new TourRepository();

            TourRealisations.Clear();
            foreach (TourRealisation tR in _tours.GetAllTourRealisations())
            {

                if (tR.TourId == pickedTour.Id)
                {
                    TourRealisations.Add(tR);
                }
            }
        }

        private void tourList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void IncreaseCount_Click(object sender, RoutedEventArgs e)
        {
            NumberOfPeople++;
            PeopleCounter.Text = NumberOfPeople.ToString();
        }

        private void DecreaseCount_Click(object sender, RoutedEventArgs e)
        {
            if (NumberOfPeople > 1)
            {
                NumberOfPeople--;
            }
            PeopleCounter.Text = NumberOfPeople.ToString();

        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void BookNow_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTourRealisation != null)
            {
                OpenBookTourWindow();
            }
            else
            {
                MessageBox.Show("No tour has been selected.", "Warning", MessageBoxButton.OK);
            }
        }

        private void OpenBookTourWindow()
        {
            if(SelectedTourRealisation.AvailableSeats >= NumberOfPeople)
            {
                TourPersonRegistrationForm personFormular = new TourPersonRegistrationForm(NumberOfPeople, SelectedTourRealisation.Id);
                personFormular.Owner = this;
                personFormular.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                personFormular.Show();
            }
            else if(SelectedTourRealisation.AvailableSeats == 0){
                MessageBox.Show("Unfortunatelly, tour for selected term is full. \n We will show you other avaliable terms.", ":(", MessageBoxButton.OK);
                TourRealisations.Remove(SelectedTourRealisation);
            }
            else
            {
                MessageBox.Show($"There is not enough space for the entered number of people on this tour \n(Avaliable seats: {SelectedTourRealisation.AvailableSeats})", ":(", MessageBoxButton.OK);
            }
        }
    }
}
