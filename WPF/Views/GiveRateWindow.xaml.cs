using BookingApp.Domain.Models;
using BookingApp.Repositories;
using BookingApp.WPF.ViewModels;
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
    /// Interaction logic for GiveRateWindow.xaml
    /// </summary>
    public partial class GiveRateWindow : Window
    {
        private int cleanlinessRating=1;
            public int CleanlinessRating
        {
            get
            {   
                return cleanlinessRating;
            }
            set { 
                if (value != cleanlinessRating && value>0 && value<6)
                {
                    cleanlinessRating = value;
                    OnPropertyChanged("CleanlinessRating");
                }
            }
        }
        private int ruleComplianceRating=1;
        public int RuleComplianceRating
        {
            get
            {
                return ruleComplianceRating;
            }
            set
            {
                if (value != ruleComplianceRating && value>0 && value<6)
                {
                    ruleComplianceRating = value;
                    OnPropertyChanged("RuleComplianceRating");
                }
            }
        }
        private string comment;
        public string Comment
        {
            get
            {
                return comment;
            }
            set
            {
                if (value != comment)
                {
                    comment = value;
                    OnPropertyChanged("Comment");
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private GuestRatingRepository GuestRatingRepository;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private GuestRatingDTO GuestRatingDTO;

        public GiveRateWindow(GuestRatingDTO guestRatingDTO)
        {
            GuestRatingRepository = new GuestRatingRepository();
            GuestRatingDTO = guestRatingDTO;
            InitializeComponent();
            DataContext = this;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void RateGuestButton_Click(object sender, RoutedEventArgs e)
        {
            GuestRatingRepository.Save(new GuestRating(GuestRatingDTO.AccommodatioId,GuestRatingDTO.GuestId,GuestRatingDTO.ReservationId,cleanlinessRating,ruleComplianceRating,comment));
            RateGuestsWindow.GuestRatingsObservable.Remove(GuestRatingDTO);
            Close();
        }
    }
}
