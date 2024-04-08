using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Appl.UseCases
{
    public class AccommodationService
    {
        private IAccommodationRepository accommodationRepository;
        public AccommodationService() 
        {
            accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
        }
        public bool IsRatedByUser(GuestRating guestRating, User user)
        {
            return accommodationRepository.GetByUser(user).Any(accommodation => accommodation.Id == guestRating.AccommodationId);
        }
        public bool IsReservationForUserAccommodation(AccommodationReservation reservation, User user)
        {
            return accommodationRepository.GetByUser(user).Any(accommodation => accommodation.Id == reservation.AccommodationId);
        }
    }
}
