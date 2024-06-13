using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Appl.UseCases
{
    public class AccommodationStatsService
    {
        private AccommodationReservationService accommodationReservationService;
        public AccommodationStatsService(AccommodationReservationService accommodationReservationService)
        {
            this.accommodationReservationService=accommodationReservationService;
        }

        public List<AccommodationStat> GetAccommodationStats(string selectedYear,Accommodation accommodation)
        {
            List<AccommodationStat> stats=new List<AccommodationStat>();
            List<AccommodationReservation> accommodationReservations;
            if (selectedYear == "All years")
            {
                accommodationReservations = GetSortedReservationsForAllYears(selectedYear, accommodation);
                GetYearlyAccommodationStats(accommodationReservations).ForEach(stat => stats.Add(stat));
            }
            else
            {
                accommodationReservations = GetSortedReservationsForSelectedYear(selectedYear, accommodation);
                GetMonthlyAccommodationStats(accommodationReservations, selectedYear).ForEach(stat => stats.Add(stat));
            }
            return stats;
        }
        private List<AccommodationReservation> GetSortedReservationsForSelectedYear(string selectedYear, Accommodation accommodation)
        {
            List<AccommodationReservation> Reservations = GetReservationsForSelectedYear(accommodation,selectedYear);
            Reservations=SortReservations(Reservations);
            return Reservations;
        }

        private List<AccommodationReservation> GetSortedReservationsForAllYears(string selectedYear, Accommodation accommodation)
        {
            List<AccommodationReservation> Reservations = accommodationReservationService.GetByAccommodation(accommodation);
            Reservations = SortReservations(Reservations);
            return Reservations;
        }

        public List<AccommodationReservation> SortReservations(List<AccommodationReservation> reservations)
        {
             reservations.Sort((r1, r2) => r1.ReservedFrom.CompareTo(r2.ReservedFrom));
            return reservations;
        }
        private List<AccommodationReservation> GetReservationsForSelectedYear(Accommodation accommodation,string selectedYear)
        {
           return accommodationReservationService.GetByAccommodation(accommodation)
                .Where(reservation =>  reservation.IsMadeOrEndedInSelectedYear(Convert.ToInt32(selectedYear))).ToList();
        }

        public string FindMostBusy(string selectedYear,ObservableCollection<AccommodationStat>accommodationStats)
        {
            AccommodationStat mostBusy = GetMostBusy(accommodationStats);
            return "";

           
        }


        private AccommodationStat GetMostBusy(ObservableCollection<AccommodationStat> accommodationStats)
        {
            return accommodationStats.OrderByDescending(aS => aS.Busyness).First();

        }
        private List<AccommodationStat> GetYearlyAccommodationStats(List<AccommodationReservation> reservations)
        {
            int lastBusyYear = reservations[reservations.Count - 1].ReservedTo.Year;
            int firstBusyYear = reservations[0].ReservedFrom.Year;
            List<AccommodationStat> accommodationStats = new List<AccommodationStat>();
            for (int year = lastBusyYear; year >= firstBusyYear; year--)
            {
                accommodationStats.Add(CreateAccommodationStatForYear(year, reservations));
              
            }
            return accommodationStats;
        }
        private List<AccommodationStat> GetMonthlyAccommodationStats(List<AccommodationReservation> reservations,string selectedYear)
        {
            List<AccommodationStat> accommodationStats = new List<AccommodationStat>();
            for (int i = 1; i <= 12; i++)
            {
                accommodationStats.Add(CreateAccommodationStatForMonth(i, reservations, Convert.ToInt32(selectedYear)));
            }
            return accommodationStats;
        }
        private AccommodationStat CreateAccommodationStatForYear(int year,List<AccommodationReservation> reservations)
        {
            AccommodationStat accommodationStat = new AccommodationStat
            {
                NumberOfReservations = reservations.Count(r => r.IsMadeInSelectedYear(year) && !r.IsCanceled()),
                NumberOfCancelledReservations = reservations.Where(r => r.IsMadeInSelectedYear(year)).Sum(r => r.Cancelled),
                NumberOfRecommendedRenovations = reservations.Where(r => r.IsMadeInSelectedYear(year)).Sum(r => r.RecommendedRenovation),
                NumberOfRescheduledReservations = reservations.Where(r => r.IsMadeInSelectedYear(year)).Sum(r => r.RescheduledReservation),
                Busyness = CalculateYearlyBusyness(year,reservations),
                Year = $"{year}"
            };

            return accommodationStat;
        }
        private AccommodationStat CreateAccommodationStatForMonth(int month, List<AccommodationReservation> reservations,int selectedYear)
        {
            AccommodationStat accommodationStat = new AccommodationStat
            {
                NumberOfReservations = reservations.Where(r => r.ReservedFrom.Year == selectedYear && !r.IsCanceled()).Count(r => r.ReservedFrom.Month == month),
                NumberOfCancelledReservations = reservations.Where(r => r.IsMadeInSelectedMonth(month) && r.IsMadeInSelectedYear(selectedYear)).Sum(r => r.Cancelled),
                NumberOfRecommendedRenovations = reservations.Where(r => r.IsMadeInSelectedMonth(month) && r.IsMadeInSelectedYear(selectedYear)).Sum(r => r.RecommendedRenovation),
                NumberOfRescheduledReservations = reservations.Where(r => r.IsMadeInSelectedMonth(month) && r.IsMadeInSelectedYear(selectedYear)).Sum(r => r.RescheduledReservation),
                Busyness = CalculateMonthlyBusyness(month,selectedYear,reservations),
                Year = $"{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month)}"
            };

            return accommodationStat;
        }

        private double CalculateYearlyBusyness(int year,List<AccommodationReservation> reservations)
        {
            int numberOfDaysInYear = GetNumberOfDaysInYear(year);
            int numberOfReservationsinYear = accommodationReservationService.GetNumberOfReservedDaysInYear(year,reservations);
            double busyness = (double)numberOfReservationsinYear / numberOfDaysInYear;
            return busyness;
        }
        private double CalculateMonthlyBusyness(int month,int selectedYear,List<AccommodationReservation> reservations)
        {
            int numberOfDaysInMonth = GetNumberOfDaysInMonth(month,selectedYear);
            int numberOfReservationsInMonth = accommodationReservationService.GetNumberOfReservationsInMonth(month,reservations,selectedYear);
            double busyness = (double)numberOfReservationsInMonth / numberOfDaysInMonth;
            return busyness;
        }
        private int GetNumberOfDaysInYear(int currentYear)
        {
            return DateTime.IsLeapYear(currentYear) ? 366 : 365;
        }
        private int GetNumberOfDaysInMonth(int month,int selectedYear)
        {
            return DateTime.DaysInMonth(selectedYear, month);
        }

    }
}
