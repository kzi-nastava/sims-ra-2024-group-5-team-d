using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IAccommodationRatingRepository
    {
        public List<AccommodationRating> GetAll();
        public AccommodationRating Update(AccommodationRating accommodationRating);
        public List<AccommodationRating> GetByGuest(User guest);
        public AccommodationRating Save(AccommodationRating accommodationRating);
        public AccommodationRating GetById(int id);
        public AccommodationRating GetByReservationId(int reservationId);
        public int NextId();
        public void Delete(AccommodationRating accommodationRating);
    }
}
