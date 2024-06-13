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
        private TourRealisationService tourRealisationService;
        public TourReservationService() 
        {
            tourRealisationService = new TourRealisationService();
            _tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
        }

        public List<TourReservation> GetTourReservationsForTourist(User tourist)
        {
            return _tourReservationRepository.GetAllTourReservations().Where(reservation => reservation.TourRealisationId != -1 && reservation.User.Id == tourist.Id).ToList();
        }
        public List<TourReservation> GetPastTourReservationsForTourist(User tourist)
        {
            return GetTourReservationsForTourist(tourist)
                    .Where(tourReservation =>
                        tourReservation.TourRealisationId != -1 &&
                        tourRealisationService.GetTourRealisationById(tourReservation.TourRealisationId).IsFinished == true)
                    .ToList();
        }

        public TourReservation GetById(int id)
        {
            return _tourReservationRepository.GetTourReservationById(id);
        }

        public TourReservation Save(TourReservation reservation)
        {
            return _tourReservationRepository.SaveReservation(reservation);
        }

        public int NextId()
        {
            return _tourReservationRepository.NextIdForReservation();
        }

        public TourReservation GetLiveTourReservation(int userId)
        {
            foreach (TourReservation reservation in _tourReservationRepository.GetAllTourReservations())
            {
                if(reservation.User.Id == userId && reservation.TourRealisationId != -1 && tourRealisationService.GetTourRealisationById(reservation.TourRealisationId).IsLive)
                {
                    return reservation;
                }
            }

            return null;
        }
        public List<TourReservation> GetAll()
        {
            return _tourReservationRepository.GetAllTourReservations();
        }
        public void DeleteTourReservation(TourReservation id)
        {
            _tourReservationRepository.DeleteReservation(id);
            return;
        }
        public TourReservation Update(TourReservation tourReservation)
        {
            return _tourReservationRepository.Update(tourReservation);
        }
    }
}
