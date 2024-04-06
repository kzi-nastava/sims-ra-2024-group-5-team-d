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
        private IGuestRatingRepository guestRatingRepository;
        private IAccommodationRepository accommodationRepository;
        private IAccommodationReservationRepository accommodationReservationRepository;
        private AccommodationReservationService accommodationReservationService;
        private GuestRatingService guestRatingService;

        public CheckForUnratedGuestsService()
        {
            accommodationReservationService = new AccommodationReservationService();
            guestRatingService = new GuestRatingService();
            guestRatingRepository=Injector.CreateInstance<IGuestRatingRepository>();
            accommodationRepository=Injector.CreateInstance<IAccommodationRepository>();
            accommodationReservationRepository = Injector.CreateInstance<IAccommodationReservationRepository>();
        }
        public bool CheckForUnratedGuestsByLoggedInUser(User loggedInUser)
        {
            List<GuestRating> guestRatingsByLoggedInUser= guestRatingService.GetAllGuestRatingsByUser(loggedInUser);
            List<AccommodationReservation> reservationsForLoggedInUserAccommodations= accommodationReservationService.GetAccommodationReservationsForUser(loggedInUser);
            return AreThereUnratedGuests(guestRatingsByLoggedInUser,reservationsForLoggedInUserAccommodations);
        }

        //Da li ovo jos treba razdvoji ovaj poslednji return
        private bool AreThereUnratedGuests(List<GuestRating> guestRatingsByLoggedInUser,List<AccommodationReservation> reservationsForLoggedInUserAccommodations)
        {
            List<AccommodationReservation> potentialUnratedReservations = accommodationReservationService.FindRateableAccommodationReservations(reservationsForLoggedInUserAccommodations);
            return !potentialUnratedReservations.All(p => accommodationReservationService.IsGuestFromReservationRated(p, guestRatingsByLoggedInUser));
        }
      
    }
}
