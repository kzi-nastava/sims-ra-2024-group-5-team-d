using BookingApp.Domain.Models;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace BookingApp.WPF.Views
{
    /// <summary>
    /// Interaction logic for NewTourRealisationForm.xaml
    /// </summary>
    public partial class NewTourRealisationForm : Window
    {
        private int TourId{ get; set; }
        private string TourName { get; set; }
        private Tour SelectedTour { get; set; }
        public User LoggedInUser { get; }
        private int Capacity { get; set; }
        private TourRealisationRepository _repository { get; set; }

        private DateTime dateTime;
        public DateTime DateTime
        {
            get => dateTime;
            set
            {
                if (value != dateTime)
                {
                    dateTime = value;
                    OnPropertyChanged();
                }
            }
        }
        public NewTourRealisationForm(User loggedInUser, Tour selectedTour)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = loggedInUser;
            SelectedTour = selectedTour;
            TourId = selectedTour.Id;
            TourName = selectedTour.Name;
            Capacity = selectedTour.MaxCapacity;
            _repository = new TourRealisationRepository();
        }
        public void CancelTourRealisation_Button(object sender, RoutedEventArgs e)
        {
            Close();
        }
        public void ConfirmTourRealisation_Button(object sender, RoutedEventArgs e)
        {
            TourRealisation newTourRealisation = new TourRealisation(DateTime, TourId, Capacity, LoggedInUser);
            TourRealisation savedTourRealisation = _repository.SaveTourRealisation(newTourRealisation);
            Close();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
