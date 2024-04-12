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
        private IAccommodationRepository accommodationRepository;
        public AccommodationReservationService() {

            accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
            accommodationReservationRepository = Injector.CreateInstance<IAccommodationReservationRepository>();
            
        }
        public List<AccommodationReservation> GetAllReservationsForOwner(User owner)
        {
            return accommodationReservationRepository.GetAll()
                         .Where(reservation => IsReservationForOwnerAccommodation(reservation, owner)).ToList();
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
        public List<AccommodationReservation> GetAllReservationsForAccommodation(int accommodationId)
        {
            return accommodationReservationRepository.GetAll()
                .Where(reservation => reservation.AccommodationId == accommodationId).ToList();
        }
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
        public bool IsReservationForOwnerAccommodation(AccommodationReservation reservation, User owner)
        {
            return accommodationRepository.GetByUser(owner).Any(accommodation => accommodation.Id == reservation.AccommodationId);
        }
        public List<AccommodationReservation> GetAll()
        {
            return accommodationReservationRepository.GetAll();
        }
        public AccommodationReservation Save(AccommodationReservation reservation)
        {
            return accommodationReservationRepository.Save(reservation);
        }
        public void Delete(AccommodationReservation reservation)
        {
            accommodationReservationRepository.Delete(reservation);
        }
        public AccommodationReservation Update(AccommodationReservation reservation)
        {
            return accommodationReservationRepository.Update(reservation);
        }
        public List<AccommodationReservation> GetByUser(User user)
        {
            return accommodationReservationRepository.GetByUser(user);
        }
        public AccommodationReservation GetById(int id)
        {
            return accommodationReservationRepository.GetById(id);
        }
        public List<AccommodationReservation> GetByAccommodation(Accommodation accommodation)
        {
            return accommodationReservationRepository.GetByAccommodation(accommodation);
        }
    }
}
