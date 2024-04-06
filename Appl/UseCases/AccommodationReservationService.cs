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
    public class AccommodationReservationService
    {
        private IAccommodationReservationRepository accommodationReservationRepository;
        private AccommodationService accommodationService;
        public AccommodationReservationService() {

            accommodationService = new AccommodationService();
            accommodationReservationRepository = Injector.CreateInstance<IAccommodationReservationRepository>();
            
        }
        public List<AccommodationReservation> GetAccommodationReservationsForUser(User user)
        {
            return accommodationReservationRepository.GetAll()
                         .Where(reservation => accommodationService.IsReservationForUserAccommodation(reservation, user)).ToList();
        }

        public List<AccommodationReservation> FindRateableAccommodationReservations(List<AccommodationReservation> reservationsForUserAccommodations)
        {
            return reservationsForUserAccommodations.Where(reservation => !reservation.IsCanceled() && reservation.IsRateable()).ToList();
        }

        public List<AccommodationReservation> GetUnratedReservations(List <AccommodationReservation> reservations,List<GuestRating> guestRatings)
        {
            return  reservations.Where(accommodationReservation => !IsGuestFromReservationRated(accommodationReservation, guestRatings)).ToList();
        }
        //DA LI OVA FUNKCIJA MOZ EU NEKI MODEL DA SE STAVI
        public bool IsGuestFromReservationRated(AccommodationReservation p, List<GuestRating> guestRatings)
        {
            return guestRatings.Any(q => q.ReservationId == p.Id);
        }

        /* ILI OVA
        public bool IsGuestFromReservationRated(AccommodationReservation p, List<GuestRating> guestRatings)
        {
            return guestRatings.Any(q => p.IsReservationRated(q.ReservationId));
        }*/
        public int GetNumberOfReservedDaysInYear(int currentYear, List<AccommodationReservation> reservations)
        {
            return reservations.Where(r => r.IsMadeOrEndedInSelectedYear(currentYear) && !r.IsCanceled())
                .Sum(r => r.CalculateNumberOfDaysInSelectedYear(currentYear));
        }
        public int GetNumberOfReservationsInMonth(int month, List<AccommodationReservation> reservations, int selectedYear)
        {
            return reservations.Where(r => r.IsInSelectedMonth(month) && !r.IsCanceled())
                .Sum(r => r.CalculateNumberOfDaysInSelectedMonthInYear(month, selectedYear));
        }
    }
}
