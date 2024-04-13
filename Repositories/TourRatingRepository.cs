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
    public class TourRatingRepository : ITourRatingRepository
    {
        private const string FilePathTours = "../../../Resources/Data/tourRating.csv";


        private readonly Serializer<TourRating> _serializerTourRating;

        private List<TourRating> _tourRatings;
        public TourRatingRepository()
        {
            _serializerTourRating = new Serializer<TourRating>();
            _tourRatings = _serializerTourRating.FromCSV(FilePathTours);
        }
        public List<TourRating> GetAllTourRatings()
        {
            return _tourRatings = _serializerTourRating.FromCSV(FilePathTours);
        }
        public int NextIdForTourRating()
        {
            _tourRatings = _serializerTourRating.FromCSV(FilePathTours);
            if (_tourRatings.Count < 1)
            {
                return 1;
            }
            return _tourRatings.Max(c => c.Id) + 1;
        }
        public TourRating Save(TourRating rating)
        {
            rating.Id = NextIdForTourRating();
            _tourRatings = _serializerTourRating.FromCSV(FilePathTours);
            _tourRatings.Add(rating);
            _serializerTourRating.ToCSV(FilePathTours, _tourRatings);
            return rating;
        }
        public TourRating UpdateTourRating(TourRating rating)
        {
            _tourRatings = _serializerTourRating.FromCSV(FilePathTours);
            TourRating current = _tourRatings.Find(c => c.Id == rating.Id);
            int index = _tourRatings.IndexOf(current);
            _tourRatings.Remove(current);
            _tourRatings.Insert(index, rating);       // keep ascending order of ids in file 
            _serializerTourRating.ToCSV(FilePathTours, _tourRatings);
            return rating;
        }
        public TourRating GetTourRatingById(int id)
        {
            _tourRatings = _serializerTourRating.FromCSV(FilePathTours);
            foreach (TourRating t in _tourRatings)
            {
                if (t.Id == id)
                {
                    return t;
                }
            }
            return null;
        }
        public void DeleteTourRating(TourRating tour)
        {
            _tourRatings = _serializerTourRating.FromCSV(FilePathTours);
            TourRating founded = _tourRatings.Find(c => c.Id == tour.Id);
            _tourRatings.Remove(founded);
            _serializerTourRating.ToCSV(FilePathTours, _tourRatings);
        }

    }
}
