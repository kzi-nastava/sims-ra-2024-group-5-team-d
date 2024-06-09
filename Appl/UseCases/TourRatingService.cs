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
        private TourReservationService tourReservationService { get; set; }
        private TourRealisationService tourRealisationService { get; set; }
        public TourRatingService()
        {
            tourRatingRepository = Injector.CreateInstance<ITourRatingRepository>();
            tourRealisationService = new TourRealisationService();
            tourReservationService = new TourReservationService();
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
        public double GetAverageRatingForGuide(User guide)
        {
            var ratings = RatingsOfGuide(guide);

            if (ratings == null || !ratings.Any())
            {
                return 0;
            }
            double totalScore = ratings.Sum(rating => rating.TouristLanguage + rating.TouristKnowladge + rating.TourAmusement);

            double averageScore = totalScore / (ratings.Count * 3);

            return averageScore;
        }

        public List<TourRating> RatingsOfGuide(User guide)
        {
            return GetAllTourRatings()
                   .Where(rating =>
                       tourRealisationService.GetById(
                           tourReservationService.GetById(rating.TourReservationId).TourRealisationId
                       ).User.Id == guide.Id)
                   .ToList();
        }
        public bool WasTourRated(int tourReservationId)
        {
            foreach (TourRating rating in GetAllTourRatings())
            {
                if (rating.TourReservationId == tourReservationId)
                {
                    return true;
                }
            }
            return false;
        }

        public double GetRating(int tourReservationId)
        {
            double rating = 0.00;
            foreach (TourRating rat in GetAllTourRatings())
            {
                if (rat.TourReservationId == tourReservationId)
                {
                    rating = (double)(rat.TouristLanguage + rat.TouristKnowladge + rat.TourAmusement) / 3;
                }
            }

            return rating;
        }

    }
}
