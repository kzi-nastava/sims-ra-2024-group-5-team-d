using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Appl.UseCases
{
    public class TourRatingService
    {
        private ITourRatingRepository tourRatingRepository { get; set; }
        public TourRatingService()
        {
            tourRatingRepository = Injector.CreateInstance<ITourRatingRepository>();
        }

        public List<TourRating> GetAllTourRatings()
        {
            return tourRatingRepository.GetAllTourRatings();
        }
        public int NextIdForTourRating()
        {
            return tourRatingRepository.NextIdForTourRating();
        }
        public TourRating UpdateTourRating(TourRating rating)
        {
            return tourRatingRepository.UpdateTourRating(rating);
        }
        public TourRating GetTourRatingById(int id)
        {
            return tourRatingRepository.GetTourRatingById(id);
        }
        public void DeleteTourRating(TourRating rating)
        {
            tourRatingRepository.DeleteTourRating(rating);
        }
        public TourRating Save(TourRating rating)
        {
            return tourRatingRepository.Save(rating);

        }
    }
}
