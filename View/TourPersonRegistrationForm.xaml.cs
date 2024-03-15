using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for TourPersonRegistrationForm.xaml
    /// </summary>
    public partial class TourPersonRegistrationForm : Window
    {
        private List<TourGuest> people;
        private int currentPageIndex { get; set; } = 0;
        private int idTourRealisation { get; set; }

        public TourPersonRegistrationForm(int numberOfPeople, int selectedRealisationId)
        {
            InitializeComponent();

            // Initialize the list of people
            people = new List<TourGuest>();
            for (int i = 0; i < numberOfPeople; i++)
            {
                people.Add(new TourGuest());
            }
            idTourRealisation = selectedRealisationId;
            currentPageIndex = 0;
            DisplayCurrentPerson();
            UpdateNavigationButtons();
            DataContext = people[currentPageIndex];
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void DisplayCurrentPerson()
        {
            PersonNumberLabel.Content = "Person " + (currentPageIndex + 1).ToString();
            
            if (currentPageIndex >= 0 && currentPageIndex < people.Count)
            {
                var currentPerson = people[currentPageIndex];
                NameTextBox.Text = currentPerson.FullName;
                AgeTextBox.Text = currentPerson.Years.ToString();
            }
        }

        private void UpdateNavigationButtons()
        {
            PreviousButton.IsEnabled = currentPageIndex > 0;
            NextButton.Content = currentPageIndex == people.Count - 1 ? "Confirm" : "Next";
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            people[currentPageIndex].FullName = NameTextBox.Text;
            people[currentPageIndex].Years = Convert.ToInt32(AgeTextBox.Text);
            currentPageIndex--;

            NameTextBox.Text = people[currentPageIndex].FullName;
            AgeTextBox.Text = people[currentPageIndex].Years.ToString();

            DisplayCurrentPerson();
            UpdateNavigationButtons();
            DataContext = people[currentPageIndex];
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (NameTextBox.Text == "" || AgeTextBox.Text == "")
            {
                MessageBox.Show("Error", "You didn't fill all the info", MessageBoxButton.OK);
            }
            if (currentPageIndex == people.Count - 1)
            {
                people[currentPageIndex].FullName = NameTextBox.Text;

                people[currentPageIndex].Years = Convert.ToInt32(AgeTextBox.Text);

                TourReservation reservation = new TourReservation();
                reservation.TourRealisationId = idTourRealisation;
                TourGuestRepository tourGuestRepository = new TourGuestRepository();
                tourGuestRepository.SaveReservation(reservation);
                TourRepository tourRepository = new TourRepository();
                TourRealisation tourRealisation = new TourRealisation();
                tourRealisation = tourRepository.GetTourRealisationById(reservation.TourRealisationId);
                // Confirm registration
                MessageBox.Show("Registration confirmed!");
                foreach(TourGuest person in people)
                {
                    person.TourReservationId = reservation.Id;
                    person.CheckPointId = -1;
                    tourGuestRepository.SaveGuest(person);
                    tourRealisation.AvailableSeats--;
                }
                tourRepository.UpdateTourRealisation(tourRealisation);
                this.Close();
            }
            else if (NameTextBox.Text != "" && AgeTextBox.Text != "")
            {
                people[currentPageIndex].FullName = NameTextBox.Text;

                people[currentPageIndex].Years = Convert.ToInt32(AgeTextBox.Text);
                
                currentPageIndex++;

                
                NameTextBox.Text = people[currentPageIndex].FullName;
                AgeTextBox.Text = people[currentPageIndex].Years.ToString();
                DisplayCurrentPerson();
                UpdateNavigationButtons();
                DataContext = people[currentPageIndex];
            }

        }
    }
}
