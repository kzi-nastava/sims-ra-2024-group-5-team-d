using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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
        public ObservableCollection<string> ImagesPaths { get; set; }
        private TourReservationService reservationService { get; set; }
        private TourRatingService tourRatingService { get; set; }
        private TourGuestService guestService { get; set; }
        

        private string comment;

        public string Comment
        {
            get => comment;
            set
            {
                if (value != comment)
                {
                    comment = value;
                    OnPropertyChanged();
                }
            }
        }



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

        private bool isRated;
        public bool IsRated
        {
            get => isRated;
            set
            {
                if (value != isRated)
                {
                    isRated = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool isRateable;
        public bool IsRateable
        {
            get => isRateable;
            set
            {
                if (value != isRateable)
                {
                    isRateable = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool notRated;
        public bool NotRated
        {
            get => notRated;
            set
            {
                if (value != notRated)
                {
                    notRated = value;
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

        private bool language1;
        public bool Language1
        {
            get => language1;
            set
            {
                if (value != language1)
                {
                    language1 = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool language2;
        public bool Language2
        {
            get => language2;
            set
            {
                if (value != language2)
                {
                    language2 = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool language3;
        public bool Language3
        {
            get => language3;
            set
            {
                if (value != language3)
                {
                    language3 = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool language4;
        public bool Language4
        {
            get => language4;
            set
            {
                if (value != language4)
                {
                    language4 = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool language5;
        public bool Language5
        {
            get => language5;
            set
            {
                if (value != language5)
                {
                    language5 = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool knowledge1;
        public bool Knowledge1
        {
            get => knowledge1;
            set
            {
                if (value != knowledge1)
                {
                    knowledge1 = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool knowledge2;
        public bool Knowledge2
        {
            get => knowledge2;
            set
            {
                if (value != knowledge2)
                {
                    knowledge2 = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool knowledge3;
        public bool Knowledge3
        {
            get => knowledge3;
            set
            {
                if (value != knowledge3)
                {
                    knowledge3 = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool knowledge4;
        public bool Knowledge4
        {
            get => knowledge4;
            set
            {
                if (value != knowledge4)
                {
                    knowledge4 = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool knowledge5;
        public bool Knowledge5
        {
            get => knowledge5;
            set
            {
                if (value != knowledge5)
                {
                    knowledge5 = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool amusement1;
        public bool Amusement1
        {
            get => amusement1;
            set
            {
                if (value != amusement1)
                {
                    amusement1 = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool amusement2;
        public bool Amusement2
        {
            get => amusement2;
            set
            {
                if (value != amusement2)
                {
                    amusement2 = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool amusement3;
        public bool Amusement3
        {
            get => amusement3;
            set
            {
                if (value != amusement3)
                {
                    amusement3 = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool amusement4;
        public bool Amusement4
        {
            get => amusement4;
            set
            {
                if (value != amusement4)
                {
                    amusement4 = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool amusement5;
        public bool Amusement5
        {
            get => amusement5;
            set
            {
                if (value != amusement5)
                {
                    amusement5 = value;
                    OnPropertyChanged();
                }
            }
        }



        private string star1Path;
        public string Star1Path
        {
            get => star1Path;
            set
            {
                if (value != star1Path)
                {
                    star1Path = value;
                    OnPropertyChanged();
                }
            }
        }

        private string star2Path;
        public string Star2Path
        {
            get => star2Path;
            set
            {
                if (value != star2Path)
                {
                    star2Path = value;
                    OnPropertyChanged();
                }
            }
        }

        private string star3Path;
        public string Star3Path
        {
            get => star3Path;
            set
            {
                if (value != star3Path)
                {
                    star3Path = value;
                    OnPropertyChanged();
                }
            }
        }

        private string star4Path;
        public string Star4Path
        {
            get => star4Path;
            set
            {
                if (value != star4Path)
                {
                    star4Path = value;
                    OnPropertyChanged();
                }
            }
        }

        private string star5Path;
        public string Star5Path
        {
            get => star5Path;
            set
            {
                if (value != star5Path)
                {
                    star5Path = value;
                    OnPropertyChanged();
                }
            }
        }

        public double Rating { get; set; }

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
            reservationService = new TourReservationService();
            tourRatingService = new TourRatingService();
            guestService = new TourGuestService();
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
            NotRated = !tourRatingService.WasTourRated(ReservationId);
            IsRated = !NotRated;
            Rating = tourRatingService.GetRating(reservationId);
            SetStarPaths();
            IsRateable = guestService.WasTouristOnTour(ReservationId);
            Language1 = false;
            Language2 = false;
            Language3 = false;
            Language4 = false;
            Language5 = false;
            Knowledge1 = false;
            Knowledge2 = false;
            Knowledge3 = false;
            Knowledge4 = false;
            Knowledge5 = false;
            Amusement1 = false;
            Amusement2 = false;
            Amusement3 = false;
            Amusement4 = false;
            Amusement5 = false;
        }

        public void SetStarPaths()
        {
            int fullStars = (int)Rating; // Number of full stars
            int halfStars = (int)((Rating - fullStars) * 2); // Number of half stars

            for (int i = 1; i <= 5; i++)
            {
                if (i <= fullStars)
                {
                    SetStarPath(i, "full_star.png");
                }
                else if (i == fullStars + 1 && halfStars > 0)
                {
                    SetStarPath(i, "half_star.png");
                    halfStars--;
                }
                else
                {
                    SetStarPath(i, "empty_star.png");
                }
            }
        }

        private void SetStarPath(int starNumber, string fileName)
        {
            string basePath = @"C:\Users\Korisnik\Desktop\ikone\";
            string fullPath = Path.Combine(basePath, fileName);

            switch (starNumber)
            {
                case 1:
                    Star1Path = fullPath;
                    break;
                case 2:
                    Star2Path = fullPath;
                    break;
                case 3:
                    Star3Path = fullPath;
                    break;
                case 4:
                    Star4Path = fullPath;
                    break;
                case 5:
                    Star5Path = fullPath;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(starNumber), "Invalid star number.");
            }
        }

    }
}
