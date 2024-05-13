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
        public AccommodationReservationService(IAccommodationReservationRepository accommodationReservationRepository)
        {
            this.accommodationReservationRepository = accommodationReservationRepository;
        }
        public AccommodationReservationService(IAccommodationReservationRepository accommodationReservationRepository,AccommodationService accommodationService)
        {
            this.accommodationReservationRepository = accommodationReservationRepository;
            this.accommodationService = accommodationService;
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
            return accommodationService.IsUserOwnerOfAccommodation(owner, reservation.AccommodationId);
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

        public bool HasReservationOnLocation(User user, Location location)
        {
            List<AccommodationReservation> reservations = GetByUser(user).Where(reservation=>reservation.IsFinished()).ToList();
            return reservations.Any(reservation=>accommodationService.GetById(reservation.AccommodationId).Location.Id==location.Id);
        }
        public int GetNumberOfReservationsLastYear(User user)
        {
            return GetByUser(user).Where(reservation=>reservation.IsInLastYear() && !reservation.IsCanceled()).Count();
        }
        public int GetNumberOfReservationsForOwner(User owner)
        {
            return GetAllReservationsForOwner(owner).Where(reservation=>reservation.IsFinished()).Count();
        }

        public Accommodation GetMostPopularAccommodationForOwner(User owner)
        {
            Accommodation mostPopular = null;
            int numberOfReservations = 0;
            accommodationService.GetByUser(owner).ForEach(accommodation=>
            { int numberOfReservationsForAccommodation = GetNumberOfReservationForAccommodation(accommodation);
                if(numberOfReservationsForAccommodation > numberOfReservations)
                {
                    mostPopular = accommodation;
                    numberOfReservations = numberOfReservationsForAccommodation;
                }
            });
            return mostPopular;
        }

        private int GetNumberOfReservationForAccommodation(Accommodation accommodation)
        {
            return GetAllReservationsForAccommodation(accommodation.Id).Where(reservation => reservation.IsFinished()).Count();
        }

        public Accommodation GetMostBusyAccommodationForOwner(User owner)
        {
            Accommodation mostPopular = null;
            int busyness = 0;
            accommodationService.GetByUser(owner).ForEach(accommodation =>
            {
                int busynessForAccommodation = GetBusynessForAccommodation(accommodation);
                Debug.WriteLine("Busyness for accommodation " + accommodation.Name + " is " + busynessForAccommodation);
                if (busynessForAccommodation > busyness)
                {
                    mostPopular = accommodation;
                    busyness = busynessForAccommodation;
                }
            });
            return mostPopular;
        }
        private int GetBusynessForAccommodation(Accommodation accommodation)
        {
            return GetAllReservationsForAccommodation(accommodation.Id).Where(reservation => reservation.IsFinished()).Sum(reservation=>reservation.CalculateNumberOfReservedDays());
        }
    }
}
