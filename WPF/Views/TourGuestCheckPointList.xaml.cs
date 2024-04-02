using BookingApp.Domain.Models;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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

namespace BookingApp.WPF.Views
{
    /// <summary>
    /// Interaction logic for TourGuestCheckPointList.xaml
    /// </summary>
    public partial class TourGuestCheckPointList : Window
    {
        public static ObservableCollection<TourGuest> Guests { get; set; }
        private readonly TourGuestRepository _repository;
        public CheckPoint currentCheckPoint { get; set; }
        public TourGuestCheckPointList(object selectedItem, int tourId, int tourRealisationId)
        {
            InitializeComponent();
            DataContext = this;
            _repository = new TourGuestRepository();
            Guests = new ObservableCollection<TourGuest>(_repository.GetTourGuestsOnTourRealisation(tourRealisationId));
            currentCheckPoint = selectedItem as CheckPoint;
        }
        private string fullName;
        public string FullName
        {
            get { return fullName; }
            set 
            {
                if(value != fullName)
                {
                    fullName = value;
                    OnPropertyChanged(nameof(FullName));
                }
            }
        }

        private int years;
        public int Years
        {
            get { return years; }
            set
            {
                if (value != years)
                {
                    years = value;
                    OnPropertyChanged(nameof(Years));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private void SignUpGuests_Click(object sender, RoutedEventArgs e)
        {
            foreach (TourGuest guest in guestsDataGrid.SelectedItems)
            {
                guest.CheckPointId = currentCheckPoint.Id;
                _repository.UpdateTourGuest(guest);
                Close();
            }
        }

    }
}
