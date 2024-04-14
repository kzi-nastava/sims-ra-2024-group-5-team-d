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
       
        public int Id { get; set; }
        public string GuestName { get; set; }
        public Location Location { get; set; }
        public int ReservationId { get; set; }
        public string AccommodationName { get; set; }
        public int CleanlinessRating { get; set; }
        public int CorrectnessRating { get; set; }
        public DateOnly RatingDate { get; set; }
        public string Comment { get; set; }
       
        public OwnerRatingViewModel()
        {
        }
        public OwnerRatingViewModel(User guest,AccommodationRating accommodationRating,Accommodation accommodation)
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
        }
    }
}
