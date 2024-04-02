using BookingApp.Domain.Models;
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
        private List<GuestRating> GuestRatings;
        private List<AccommodationReservation> Reservations;
        private readonly AccommodationRepository accommodationRepository;
        public UnratedGuestService(List<GuestRating> guestRatings, List<AccommodationReservation> reservations)
        {
            accommodationRepository = new AccommodationRepository();
            GuestRatings = guestRatings;
            Reservations = reservations;
        }
        public List<GuestRatingDTO> GetUnratedGuests()
        {

            List<GuestRatingDTO> GuestRatings = new List<GuestRatingDTO>();
            foreach (AccommodationReservation reservation in Reservations)
            {
                if (!IsGuestFromReservationRated(reservation))
                {
                    if (IsGuestRateable(reservation) && !IsCanceled(reservation))
                        GuestRatings.Add(CreateGuestRatingDTO(reservation));
                }
            }
            return GuestRatings;
        }
        private GuestRatingDTO CreateGuestRatingDTO(AccommodationReservation reservation)
        {
            return new GuestRatingDTO(accommodationRepository.GetAccommodationNameById(reservation.AccommodationId), reservation.ReservedFrom, reservation.ReservedTo, reservation.UserId, reservation.Id, reservation.AccommodationId);
        }
        private static bool IsCanceled(AccommodationReservation reservation)
        {
            return reservation.Cancelled == 1;
        }

        private static bool IsGuestRateable(AccommodationReservation reservation)
        {

            return (DateTime.Now - reservation.ReservedTo).TotalDays <= 5 && DateTime.Now > reservation.ReservedTo;
        }

        private bool IsGuestFromReservationRated(AccommodationReservation reservation)
        {
            return GuestRatings.Any(guestRating => guestRating.ReservationId == reservation.Id);
        }

    }
}
