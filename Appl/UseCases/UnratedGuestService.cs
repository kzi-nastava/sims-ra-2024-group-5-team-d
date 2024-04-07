using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using BookingApp.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Appl.UseCases
{
    public class UnratedGuestService
    {
        private readonly IAccommodationRepository accommodationRepository;
        private GuestRatingService guestRatingService;
        private AccommodationReservationService accommodationReservationService;
        public UnratedGuestService()
        {
            accommodationReservationService = new AccommodationReservationService();
            guestRatingService = new GuestRatingService();
            accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
        }

        public List<AccommodationReservation> GetUnratedGuests(User loggedInUser)
        {
          List<GuestRating> guestRatings= guestRatingService.GetAllGuestRatingsByUser(loggedInUser);
            //Naredne 3 funkcije mogu u jednu pa da se pozivaju u drugom servisu
          List<AccommodationReservation> reservations = accommodationReservationService.GetAccommodationReservationsForUser(loggedInUser);
          List<AccommodationReservation> unratedReservations = accommodationReservationService.GetUnratedReservations(reservations,guestRatings);
          List<AccommodationReservation> rateableUnratedGuests = new List<AccommodationReservation>();
          accommodationReservationService.FindRateableAccommodationReservations(unratedReservations)
                                         .ForEach(accommodationReservation=> rateableUnratedGuests.Add(accommodationReservation));
            return rateableUnratedGuests;
        }
        private GuestRatingDTO CreateGuestRatingDTO(AccommodationReservation reservation)
        {
            return new GuestRatingDTO(accommodationRepository.GetAccommodationNameById(reservation.AccommodationId), reservation.ReservedFrom, reservation.ReservedTo, reservation.UserId, reservation.Id, reservation.AccommodationId);
        }

    }
}
