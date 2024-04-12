using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Domain.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repositories
{
    public class AccommodationRatingRepository : IAccommodationRatingRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodationRatings.csv";

        private readonly Serializer<AccommodationRating> _serializer;
        private List<AccommodationRating> _accommodationRatings;

        public AccommodationRatingRepository() 
        {
            _serializer = new Serializer<AccommodationRating>();
            _accommodationRatings = _serializer.FromCSV(FilePath);
        }
        public List<AccommodationRating> GetAll()
        {
            _accommodationRatings = _serializer.FromCSV(FilePath);
            return _accommodationRatings;
        }
        public AccommodationRating GetByReservationId(int reservationId)
        {
            _accommodationRatings = _serializer.FromCSV(FilePath);
            return _accommodationRatings.Find(accommodationRating => accommodationRating.ReservationId == reservationId);
        }
        public AccommodationRating GetById(int id)
        {
            _accommodationRatings = _serializer.FromCSV(FilePath);
            return _accommodationRatings.Find(accommodationRating => accommodationRating.Id == id);
        }
        public AccommodationRating Update(AccommodationRating accommodationRating)
        {
            _accommodationRatings = _serializer.FromCSV(FilePath);
            AccommodationRating current = _accommodationRatings.Find(a => a.Id == accommodationRating.Id);
            int index = _accommodationRatings.IndexOf(current);
            _accommodationRatings.Remove(current);
            _accommodationRatings.Insert(index, accommodationRating);      
            _serializer.ToCSV(FilePath, _accommodationRatings);
            return accommodationRating;
        }

        public AccommodationRating Save(AccommodationRating accommodationRating)
        {
            accommodationRating.Id = NextId();
            _accommodationRatings = _serializer.FromCSV(FilePath);
            _accommodationRatings.Add(accommodationRating);
            _serializer.ToCSV(FilePath, _accommodationRatings);
            return accommodationRating;
        }

        public int NextId()
        {
            _accommodationRatings = _serializer.FromCSV(FilePath);
            if (_accommodationRatings.Count < 1)
            {
                return 1;
            }
            return _accommodationRatings.Max(c => c.Id) + 1;
        }

        public void Delete(AccommodationRating accommodationRating)
        {
            _accommodationRatings = _serializer.FromCSV(FilePath);
            AccommodationRating founded = _accommodationRatings.Find(g => g.Id == accommodationRating.Id);
            _accommodationRatings.Remove(founded);
            _serializer.ToCSV(FilePath, _accommodationRatings);
        }

        public List<AccommodationRating> GetByGuest(User guest)
        {
            _accommodationRatings = _serializer.FromCSV(FilePath);
            return _accommodationRatings.FindAll(accommodation => accommodation.GuestId == guest.Id);
        }

    }
}
