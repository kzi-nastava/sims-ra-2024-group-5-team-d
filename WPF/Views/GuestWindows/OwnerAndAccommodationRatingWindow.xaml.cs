using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using BookingApp.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using System.Xml.Linq;

namespace BookingApp.WPF.Views.GuestWindows
{
    /// <summary>
    /// Interaction logic for OwnerAndAccommodationRatingWindow.xaml
    /// </summary>
    public partial class OwnerAndAccommodationRatingWindow : Window
    {
        private int _cleanliness;
        public int Cleanliness
        {
            get { return _cleanliness; }
            set
            {
                if (_cleanliness != value)
                {
                    _cleanliness = value;
                    OnPropertyChanged("Cleanliness");
                }
            }
        }

        private int _correctness;
        public int Correctness
        {
            get { return _correctness; }
            set
            {
                if (_correctness != value)
                {
                    _correctness = value;
                    OnPropertyChanged("Correctness");
                }
            }
        }

        private string _comment;
        public string Comment
        {
            get { return _comment; }
            set
            {
                if (_comment != value)
                {
                    _comment = value;
                    OnPropertyChanged("Comment");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public User LoggedInUser;
        public AccommodationReservation AccommodationReservation;
        public int accommodationId;
        public DateTime dateTime;
        private IAccommodationRatingRepository accommodationRatingRepository;
        private RateOwnerService rateOwnerService;
        public OwnerAndAccommodationRatingWindow(User user, AccommodationReservation accommodationReservation, int accommodationId)
        {
            rateOwnerService = new RateOwnerService();
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;
            AccommodationReservation = accommodationReservation;
            this.accommodationId = accommodationId;
            dateTime = DateTime.Now;
            accommodationRatingRepository = Injector.CreateInstance<IAccommodationRatingRepository>();

        }

        private void RateOwnerAndAccommodation(object sender, RoutedEventArgs e)
        {
           rateOwnerService.RateOwner(new AccommodationRating(accommodationId, LoggedInUser.Id, AccommodationReservation.Id, Cleanliness, Correctness, Comment, DateOnly.FromDateTime(dateTime)));
       
            Close();
        }
    }
}
