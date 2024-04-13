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
        public TourReservationService() 
        {
            tourRealisationRepository = Injector.CreateInstance<ITourRealisationRepository>();
            _tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
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
    }
}
