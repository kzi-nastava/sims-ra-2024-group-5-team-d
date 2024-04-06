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
        private readonly AccommodationRepository accommodationRepository;
        private CheckForUnratedGuestsService CheckForUnratedGuestsService;
        public UnratedGuestService()
        {
            accommodationRepository = new AccommodationRepository();
            CheckForUnratedGuestsService= new CheckForUnratedGuestsService();
        }
        public List<GuestRatingDTO> GetUnratedGuests(User user)
        {
          List<GuestRating> GuestRatings= CheckForUnratedGuestsService.GetAllGuestRatingsByLoggedInUser(user);// OVDE TREBA ZAMENITI SA SERVISOM KOJI DOBAVLJA SVE RATEOVANE O DULOGOVANOG KORISNIKA
          List<AccommodationReservation> Reservations= CheckForUnratedGuestsService.FindReservationsForLoggedInUserAccommodations(user);//A OVDE TREBA ZAMENITI SA SERVISOM KOJI DOBAVLJA SVE REZERVACIJE KOJE JE VLASNIK IMAO
          List<GuestRatingDTO> GuestRatingsDTO = new List<GuestRatingDTO>();
            foreach (AccommodationReservation reservation in Reservations)
            {
                if (!IsGuestFromReservationRated(reservation,GuestRatings))
                {
                    if (IsGuestRateable(reservation) && !IsCanceled(reservation))
                        GuestRatingsDTO.Add(CreateGuestRatingDTO(reservation));
                }
            }
            return GuestRatingsDTO;
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

        private bool IsGuestFromReservationRated(AccommodationReservation reservation,List<GuestRating> guestRatings)
        {
            return guestRatings.Any(guestRating => guestRating.ReservationId == reservation.Id);
        }

    }
}
