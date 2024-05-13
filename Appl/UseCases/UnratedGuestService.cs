using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using BookingApp.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Appl.UseCases
{
    public class UnratedGuestService
    {
        private GuestRatingService guestRatingService;
        private AccommodationReservationService accommodationReservationService;
        public UnratedGuestService(AccommodationService accommodationService)
        {
            accommodationReservationService = new AccommodationReservationService(Injector.CreateInstance<IAccommodationReservationRepository>(),accommodationService);
            guestRatingService = new GuestRatingService(Injector.CreateInstance<IGuestRatingRepository>(),accommodationService);
        }

        public List<AccommodationReservation> GetUnratedGuests(User owner)
        {
          List<GuestRating> guestRatings= guestRatingService.GetAllGuestRatingsByOwner(owner);
            //Naredne 3 funkcije mogu u jednu pa da se pozivaju u drugom servisu
          List<AccommodationReservation> ownerReservations = accommodationReservationService.GetAllReservationsForOwner(owner);
          List<AccommodationReservation> unratedReservations = accommodationReservationService.GetUnratedReservations(ownerReservations, guestRatings);
          List<AccommodationReservation> rateableUnratedGuests = new List<AccommodationReservation>();
          accommodationReservationService.FindRateableAccommodationReservations(unratedReservations)
                                         .ForEach(accommodationReservation=> rateableUnratedGuests.Add(accommodationReservation));
            return rateableUnratedGuests;
        }
    }
}
