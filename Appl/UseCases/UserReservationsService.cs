using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Appl.UseCases
{
    public class UserReservationsService
    {
        private IAccommodationReservationRepository accommodationReservationRepository;
        public UserReservationsService() 
        {
            accommodationReservationRepository = Injector.CreateInstance<IAccommodationReservationRepository>();
            
        }

        public List<AccommodationReservation> GetActiveReservationsForUser(User loggedUser, AccommodationReservation reservation)
        {
            List<AccommodationReservation> userReservations = accommodationReservationRepository.GetByUser(loggedUser);
            return userReservations.Where(r => reservation.IsActive()).ToList();
        }
        public List<AccommodationReservation> GetFinishedReservationsForUser(User loggedUser, AccommodationReservation reservation)
        {
            List<AccommodationReservation> userReservations = accommodationReservationRepository.GetByUser(loggedUser);
            return userReservations.Where(r => reservation.IsFinished()).ToList();

        }
        public List<AccommodationReservation> GetCancelledReservationsForUser(User loggedUser, AccommodationReservation reservation)
        {
            List<AccommodationReservation> userReservations = accommodationReservationRepository.GetByUser(loggedUser);
            return userReservations.Where(r => reservation.IsCanceled()).ToList();
            
        }
    }
}
