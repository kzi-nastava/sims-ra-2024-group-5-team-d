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

        public List<AccommodationReservation> GetActiveReservationsForUser(User loggedUser)
        {
            List<AccommodationReservation> userReservations = accommodationReservationRepository.GetByUser(loggedUser);
            return userReservations.Where(r => r.IsActive()).ToList();
        }
        public List<AccommodationReservation> GetFinishedReservationsForUser(User loggedUser)
        {
            List<AccommodationReservation> userReservations = accommodationReservationRepository.GetByUser(loggedUser);
            return userReservations.Where(r => r.IsFinished()).ToList();

        }
        public List<AccommodationReservation> GetCancelledReservationsForUser(User loggedUser)
        {
            List<AccommodationReservation> userReservations = accommodationReservationRepository.GetByUser(loggedUser);
            return userReservations.Where(r => r.IsCanceled()).ToList();
            
        }
    }
}
