using BookingApp.Domain.Models;
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
        private GuestRatingRepository GuestRatingRepository;
        private AccommodationRepository AccommodationRepository;
        private AccommodationReservationRepository ReservationRepository;
        public CheckForUnratedGuestsService()
        {
            GuestRatingRepository = new GuestRatingRepository();
            AccommodationRepository = new AccommodationRepository();
            ReservationRepository = new AccommodationReservationRepository();

        }
        public bool CheckForUnratedGuestsByLoggedInUser(User user)
        {
            List<GuestRating> GuestRatingsByLoggedInUser=GetAllGuestRatingsByLoggedInUser(user);
            List<AccommodationReservation> ReservationsForLoggedInUserAccommodations= FindReservationsForLoggedInUserAccommodations(user);
            if (AreThereUnratedGuests(GuestRatingsByLoggedInUser,ReservationsForLoggedInUserAccommodations))
                return true;
            else
                return false;
        }

        private bool AreThereUnratedGuests(List<GuestRating> guestRatingsByLoggedInUser,List<AccommodationReservation> reservationsForLoggedInUserAccommodations)
        {
            List<AccommodationReservation> potentialUnratedGuests = reservationsForLoggedInUserAccommodations.Where(reservation => !IsReservationCanceled(reservation) && IsReservationRateable(reservation)).ToList();
            return !potentialUnratedGuests.All(p => IsGuestRated(p,guestRatingsByLoggedInUser));
        }
        //OVU METODU KORISTIM I U DRUGOM SERVISU
        public List<GuestRating> GetAllGuestRatingsByLoggedInUser(User user)
        {
            return GuestRatingRepository.GetAll().Where(guestRating => IsRatedByLoggedInUser(guestRating,user)).ToList();
        }

        private bool IsRatedByLoggedInUser(GuestRating guestRating,User user)
        {
            return AccommodationRepository.GetByUser(user).Any(accommodation => accommodation.Id == guestRating.AccommodationId);
        }

        private bool IsGuestRated(AccommodationReservation p,List<GuestRating> guestRatingsByLoggedInUser)
        {
            return guestRatingsByLoggedInUser.Any(q => q.ReservationId == p.Id);
        }

        private bool IsReservationRateable(AccommodationReservation reservation)
        {
            return (DateTime.Now - reservation.ReservedTo).Days >= 0 && (DateTime.Now - reservation.ReservedTo).Days <= 5;
        }

        private bool IsReservationCanceled(AccommodationReservation reservation)
        {
            return reservation.Cancelled == 1;
        }
        //OVU METODU KORISTIM I U DRUGOM SERVISU
        public List<AccommodationReservation> FindReservationsForLoggedInUserAccommodations(User user)
        {
           return ReservationRepository.GetAll()
                        .Where(reservation => IsReservationForLoggedInUserAccommodation(reservation,user)).ToList();
        }

        private bool IsReservationForLoggedInUserAccommodation(AccommodationReservation reservation,User user)
        {
            return AccommodationRepository.GetByUser(user).Any(accommodation => accommodation.Id == reservation.AccommodationId);
        }
    }
}
