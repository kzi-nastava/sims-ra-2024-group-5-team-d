using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Appl.UseCases
{
    public class TourReservationService
    {
        private ITourReservationRepository _tourReservationRepository;
        private ITourRealisationRepository tourRealisationRepository;
        private ITourRatingRepository ratingRepository;
        public TourReservationService() 
        {
            tourRealisationRepository = Injector.CreateInstance<ITourRealisationRepository>();
            _tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
            ratingRepository = Injector.CreateInstance<ITourRatingRepository>();
        }

        public List<TourReservation> GetTourReservationsForTourist(User tourist)
        {
            return _tourReservationRepository.GetAllTourReservations().Where(reservation => reservation.User.Id == tourist.Id).ToList();
        }
        public List<TourReservation> GetPastTourReservationsForTourist(User tourist)
        {
            return GetTourReservationsForTourist(tourist).Where(tourReservation => tourRealisationRepository.GetTourRealisationById(tourReservation.TourRealisationId).IsFinished == true).ToList();
        }

        public TourReservation GetById(int id)
        {
            return _tourReservationRepository.GetTourReservationById(id);
        }

        public bool WasTourRated(int tourReservationId)
        {
            foreach(TourRating rating in ratingRepository.GetAllTourRatings())
            {
                if(rating.TourReservationId == tourReservationId)
                {
                    return true;
                }
            }
            return false;
        }

        public TourReservation GetLiveTourReservation(int userId)
        {
            foreach (TourReservation reservation in _tourReservationRepository.GetAllTourReservations())
            {
                if(reservation.User.Id == userId && tourRealisationRepository.GetTourRealisationById(reservation.TourRealisationId).IsLive)
                {
                    return reservation;
                }
            }

            return null;
        }
    }
}
