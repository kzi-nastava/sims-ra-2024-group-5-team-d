using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface ITourRatingRepository
    {
        public List<TourRating> GetAllTourRatings();
        public int NextIdForTourRating();
        public TourRating UpdateTourRating(TourRating tour);
        public TourRating GetTourRatingById(int id);
        public void DeleteTourRating(TourRating tour);
        public TourRating Save(TourRating rating);
    }
}
