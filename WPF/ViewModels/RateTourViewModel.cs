using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels
{
    public class RateTourViewModel : INotifyPropertyChanged
    {
        public int ReservationId { get; set; }
        public string TourImagesPath { get; set; }
        public string TourName { get; set; }
        public double TourDuration { get; set; }
        public Location Location { get; set; }
        public DateOnly RealisationDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public double AverageTourRating { get; set; }
        public string TouristImages { get; set; }
        public string Comment { get; set; }
        public ObservableCollection<string> ImagesPaths { get; set; }

        private string buttonContent;

        public string ButtonContent
        {
            get => buttonContent;
            set
            {
                if (value != buttonContent)
                {
                    buttonContent = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool isRatingEntered;
        public bool IsRatingEntered
        {
            get => isRatingEntered;
            set
            {
                if (value != isRatingEntered)
                {
                    isRatingEntered = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public RateTourViewModel(int reservationId, string tourImagesPath, string tourName, double tourDuration, Location location, DateOnly realisationDate, TimeOnly startTime, TimeOnly endTime, string touristImages, string comment)
        {
            ReservationId = reservationId;
            TourImagesPath = tourImagesPath;
            TourName = tourName;
            TourDuration = tourDuration;
            Location = location;
            RealisationDate = realisationDate;
            StartTime = startTime;
            EndTime = endTime;
            TouristImages = touristImages;
            Comment = comment;
        }
        public RateTourViewModel(int reservationId,string imagesPath,string tourName,Location location,double tourDuration,DateOnly realisationDate,TimeOnly startTime,TimeOnly endTIme)
        {
            ReservationId = reservationId;
            TourImagesPath = imagesPath;
            TourName = tourName;
            TourDuration = tourDuration;
            Location = location;
              RealisationDate = realisationDate;
            StartTime = startTime;
            EndTime = endTIme;
              TouristImages = imagesPath;
            IsRatingEntered = false;
           ButtonContent = "Rate";
            ImagesPaths = new ObservableCollection<string>();
        }

    }
}
