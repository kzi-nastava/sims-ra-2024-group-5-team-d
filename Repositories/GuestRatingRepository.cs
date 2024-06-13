using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Domain.Serializer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repositories
{
    public class GuestRatingRepository :IGuestRatingRepository
    {
        private const string FilePath = "../../../Resources/Data/guestRatings.csv";

        private readonly Serializer<GuestRating> _serializer;

        private List<GuestRating> _guestRatings;

        public GuestRatingRepository()
        {
            _serializer = new Serializer<GuestRating>();
            _guestRatings = _serializer.FromCSV(FilePath);
        }
        public List<GuestRating> GetAll()
        {
            _guestRatings = _serializer.FromCSV(FilePath);
            return _guestRatings;
        }

        public GuestRating Save(GuestRating guestRating)
        {
            guestRating.Id = NextId();
            _guestRatings = _serializer.FromCSV(FilePath);
            _guestRatings.Add(guestRating);
            _serializer.ToCSV(FilePath, _guestRatings);
            return guestRating;
        }

        public int NextId()
        {
            _guestRatings = _serializer.FromCSV(FilePath);
            if (_guestRatings.Count < 1)
            {
                return 1;
            }
            return _guestRatings.Max(c => c.Id) + 1;
        }

        public void Delete(GuestRating guestRating)
        {
            _guestRatings = _serializer.FromCSV(FilePath);
            GuestRating founded = _guestRatings.Find(g => g.Id == guestRating.Id);
            _guestRatings.Remove(founded);
            _serializer.ToCSV(FilePath, _guestRatings);
        }

        public GuestRating Update(GuestRating guestRating)
        {
            _guestRatings = _serializer.FromCSV(FilePath);
            GuestRating current = _guestRatings.Find(g => g.Id == guestRating.Id);
            int index = _guestRatings.IndexOf(current);
            _guestRatings.Remove(current);
            _guestRatings.Insert(index, guestRating);       // keep ascending order of ids in file 
            _serializer.ToCSV(FilePath, _guestRatings);
            return guestRating;
        }
        public List<GuestRating> GetByGuest(User guest)
        {
            _guestRatings = _serializer.FromCSV(FilePath);
            return _guestRatings.FindAll(accommodation => accommodation.GuestId == guest.Id);
        }
    }
}
