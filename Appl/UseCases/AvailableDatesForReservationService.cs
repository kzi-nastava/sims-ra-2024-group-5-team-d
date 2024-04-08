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
    public class AvailableDatesForReservationService
    {
        private readonly IAccommodationReservationRepository reservationRepository;
        public AvailableDatesForReservationService()
        {
            reservationRepository = Injector.CreateInstance<IAccommodationReservationRepository>();


        }
        
        public List<KeyValuePair<DateTime, DateTime>> CheckAvailableDatesInGivenRange(DateTime fromDate,DateTime toDate,int numberOfDays,Accommodation accommodation)
        {
                List<AccommodationReservation> reservedDatesForAccommodation= reservationRepository.GetByAccommodation(accommodation);
            List<KeyValuePair<DateTime, DateTime>> AvailableDates=new List<KeyValuePair<DateTime, DateTime>>();
           reservedDatesForAccommodation =FindReservedDatesInRange(accommodation,reservedDatesForAccommodation,fromDate,toDate);
            if (reservedDatesForAccommodation.Count != 0)
                AvailableDates=FindAndShowAvailableDates(fromDate,toDate, reservedDatesForAccommodation,numberOfDays);
            else
                AvailableDates=ShowAvailableDates(fromDate, toDate,numberOfDays);

            return AvailableDates;

        }
        public List<KeyValuePair<DateTime, DateTime>> FindandShowAvailableDatesForExtendendRange(DateTime fromDate,DateTime toDate, int numberOfDays, Accommodation accommodation)
        {
            List<KeyValuePair<DateTime, DateTime>> AvailableDates = new List<KeyValuePair<DateTime, DateTime>>();
            while (AvailableDates.Count < 5)
            {
                AvailableDates.Clear();
                fromDate = fromDate.AddDays(-1);
                toDate = toDate.AddDays(+1);
                AvailableDates=CheckAvailableDatesInGivenRange(fromDate, toDate, numberOfDays, accommodation);
            }
            if (AvailableDates.Count > 5)
                AvailableDates.RemoveAt(AvailableDates.Count - 1);
            return AvailableDates;
        }
        private List<KeyValuePair<DateTime, DateTime>> FindAndShowAvailableDates(DateTime fromDate,DateTime toDate ,List<AccommodationReservation> reservedDatesForAccommodation,int numberOfDays)
        {
            DateTime newFromDate = fromDate;
            DateTime newToDate;
            List < KeyValuePair < DateTime, DateTime >> AvailableDates= new List<KeyValuePair<DateTime, DateTime>> ();
            foreach (AccommodationReservation reservedDates in reservedDatesForAccommodation)
            {
                if (reservedDates.ReservedFrom < fromDate)
                {
                    newFromDate = reservedDates.ReservedTo;
                    continue;
                }
                newToDate = reservedDates.ReservedFrom;
                AvailableDates.AddRange(ShowAvailableDates(newFromDate, newToDate,numberOfDays));
                newFromDate = reservedDates.ReservedTo;

            }
            if (newFromDate < toDate)
                AvailableDates.AddRange(ShowAvailableDates(newFromDate, toDate, numberOfDays));
            return AvailableDates;
        }

        private List<KeyValuePair<DateTime, DateTime>> ShowAvailableDates(DateTime fromDate, DateTime toDate,int numberOfDays)
        {
          List<KeyValuePair<DateTime, DateTime>> AvailableDatePairs= new List<KeyValuePair<DateTime, DateTime>>();
           DateTime LastAvailableDate = toDate.AddDays(-numberOfDays);
            for (; fromDate <= LastAvailableDate; fromDate = fromDate.AddDays(1))
            {
                AvailableDatePairs.Add(MakeAvailableDatesPair(fromDate,numberOfDays));
            }
            return AvailableDatePairs;
        }
        private KeyValuePair<DateTime, DateTime> MakeAvailableDatesPair(DateTime start,int numberOfDays)
        {
            return new KeyValuePair<DateTime, DateTime>(start, start.AddDays(numberOfDays));
        }

        private List<AccommodationReservation> FindReservedDatesInRange(Accommodation accommodation,List<AccommodationReservation> reservedDatesForAccommodation,DateTime fromDate,DateTime toDate)
        {
            reservedDatesForAccommodation = reservationRepository.GetByAccommodation(accommodation);
            reservedDatesForAccommodation.RemoveAll(reservation => reservation.IsOutOfRange(fromDate,toDate) || IsReservationCancelled(reservation));
            reservedDatesForAccommodation.Sort((r1, r2) => r1.ReservedFrom.CompareTo(r2.ReservedFrom));
            return reservedDatesForAccommodation;
        }
        private static bool IsReservationCancelled(AccommodationReservation reservation)
        {
            return reservation.Cancelled == 1;
        }


    }
}
