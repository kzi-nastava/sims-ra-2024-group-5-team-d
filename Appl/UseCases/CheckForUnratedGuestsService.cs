using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.Appl.UseCases
{
    public class CheckForUnratedGuestsService
    {
        private AccommodationReservationService accommodationReservationService;
        private GuestRatingService guestRatingService;

        public CheckForUnratedGuestsService()
        {
            accommodationReservationService = new AccommodationReservationService();
            guestRatingService = new GuestRatingService();
        }
        public bool CheckForUnratedGuestsByLoggedInUser(User owner)
        {
            List<GuestRating> guestRatingsByLoggedInUser= guestRatingService.GetAllGuestRatingsByOwner(owner);
            List<AccommodationReservation> reservationsForOwnerAccommodations= accommodationReservationService.GetAllReservationsForOwner(owner);
            return AreThereUnratedGuests(guestRatingsByLoggedInUser, reservationsForOwnerAccommodations);
        }

        //Da li ovo jos treba razdvoji ovaj poslednji return
        private bool AreThereUnratedGuests(List<GuestRating> guestRatingsByLoggedInUser,List<AccommodationReservation> reservationsForOwnerAccommodations)
        {
            List<AccommodationReservation> potentialUnratedReservations = accommodationReservationService.FindRateableAccommodationReservations(reservationsForOwnerAccommodations);
            return !potentialUnratedReservations.All(p => accommodationReservationService.IsGuestFromReservationRated(p, guestRatingsByLoggedInUser));
        }
      
    }
}
