using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using BookingApp.WPF.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    public class OwnerRatingViewModel
    {
       
        public string Star1 { get; set; }
        public string Star2 { get; set; }
        public string Star3 { get; set; }
        public string Star4 { get; set; }
        public string Star5 { get; set; }

        public int Id { get; set; }
        public string GuestName { get; set; }
        public Location Location { get; set; }
        public int ReservationId { get; set; }
        public string AccommodationName { get; set; }
        public int CleanlinessRating { get; set; }
        public int CorrectnessRating { get; set; }
        public DateOnly RatingDate { get; set; }
        public string Comment { get; set; }
        public string AvatarPath { get; set; }
        public string RenovationText { get; set; }
        public bool IsRenovationVisible { get; set; }
       
        public OwnerRatingViewModel()
        {
        }
        public OwnerRatingViewModel(User guest,AccommodationRating accommodationRating,Accommodation accommodation,string renovationText)
        {
            Id = accommodationRating.Id;
            GuestName = guest.FullName;
            Location = accommodation.Location;
            ReservationId = accommodationRating.ReservationId;
            AccommodationName = accommodation.Name;
            CleanlinessRating = accommodationRating.Cleanliness;
            CorrectnessRating = accommodationRating.Correctness;
            RatingDate = accommodationRating.TimeOfRating;
            Comment = accommodationRating.Comment;
            AvatarPath = guest.AvatarPath;
            RenovationText = renovationText;
            IsRenovationVisible = !string.IsNullOrEmpty(renovationText);
            List<string> starPaths = new List<string>();
            double averageRating = (double)(CleanlinessRating + CorrectnessRating) / 2;
            Debug.WriteLine(averageRating);
            while (averageRating >= 1)
            {
                starPaths.Add("../../../Resources/Images/OwnerImages/StarFull.png");
                averageRating--;
            }
            if (averageRating > 0.25)
                starPaths.Add("../../../Resources/Images/OwnerImages/StarHalfFull.png");
            while (starPaths.Count < 5)
                starPaths.Add("../../../Resources/Images/OwnerImages/idemo.png");
            Star1 = starPaths[0];
            Star2 = starPaths[1];
            Star3 = starPaths[2];
            Star4 = starPaths[3];
            Star5 = starPaths[4];
        }
    }
}
