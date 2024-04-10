using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels
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
        private IAccommodationRepository accommodationRepository;
        private IUserRepository userRepository;
        public OwnerRatingViewModel()
        {

        }
        public OwnerRatingViewModel(int id,string guestName, Location location, string accommodationName, DateOnly ratingDate, string comment)
        {
            Id = id;
            GuestName = guestName;
            Location = location;
            AccommodationName = accommodationName;
            RatingDate = ratingDate;
            Comment = comment;
        }
        public OwnerRatingViewModel(AccommodationRating rating)
        {
            ReservationId = rating.ReservationId;
            CleanlinessRating = rating.Cleanliness;
            CorrectnessRating = rating.Correctness;
            accommodationRepository=Injector.CreateInstance<IAccommodationRepository>();
            userRepository=Injector.CreateInstance<IUserRepository>();
            Accommodation accommodation = accommodationRepository.GetById(rating.AccommodationId);
            Id = rating.Id;
            GuestName = userRepository.GetFullNameById(rating.GuestId);
            Location = accommodation.Location;
            AccommodationName = accommodation.Name;
            RatingDate = rating.TimeOfRating;
            Comment = rating.Comment;
        }
    }
}
