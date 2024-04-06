using BookingApp.Domain.Models;
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
    public class ShowAccommodationStatsService
    {
        private readonly AccommodationReservationRepository AccommodationReservationRepository;
        public ShowAccommodationStatsService()
        {
            AccommodationReservationRepository = new AccommodationReservationRepository();
        }
        public List<AccommodationReservation> ShowMonthlyAccommodationStats(string selectedYear, Accommodation accommodation)
        {
            List<AccommodationReservation> Reservations = GetReservationsForSelectedYear(accommodation,selectedYear);
            Reservations=SortReservations(Reservations);
            return Reservations;
        }

        public List<AccommodationReservation> ShowYearlyAccommodationStats(string selectedYear, Accommodation accommodation)
        {
            List<AccommodationReservation> Reservations = AccommodationReservationRepository.GetByAccommodation(accommodation);
            Reservations = SortReservations(Reservations);
            return Reservations;
        }

        private List<AccommodationReservation> SortReservations(List<AccommodationReservation> reservations)
        {
             reservations.Sort((r1, r2) => r1.ReservedFrom.CompareTo(r2.ReservedFrom));
            return reservations;
        }
        private List<AccommodationReservation> GetReservationsForSelectedYear(Accommodation accommodation,string selectedYear)
        {
           return AccommodationReservationRepository.GetByAccommodation(accommodation)
                .Where(reservation => IsReservationInSelectedYear(reservation, Convert.ToInt32(selectedYear))).ToList();
        }

        public string FindMostBusy(string selectedYear,ObservableCollection<AccommodationStat>accommodationStats)
        {
            AccommodationStat mostBusy = GetMostBusy(accommodationStats);
            if (selectedYear == "All years")
                return "Most visited year is: " + mostBusy.RowHeader.Split(' ')[1] + "   with busyness of: " + mostBusy.Busyness;
            else
                return "Most visited month is: " + mostBusy.RowHeader.Split(' ')[1] + " with busyness of: " + mostBusy.Busyness;

           
        }


        private AccommodationStat GetMostBusy(ObservableCollection<AccommodationStat> accommodationStats)
        {
            return accommodationStats.OrderByDescending(aS => aS.Busyness).First();

        }
        public List<AccommodationStat> GetYearlyAccommodationStats(List<AccommodationReservation> reservations)
        {
            int LastBusyYear = reservations[reservations.Count - 1].ReservedTo.Year;
            int FirstBusyYear = reservations[0].ReservedFrom.Year;
            List<AccommodationStat> accommodationStats = new List<AccommodationStat>();
            for (int i = LastBusyYear; i >= FirstBusyYear; i--)
            {
                accommodationStats.Add(CreateAccommodationStatForYear(i, reservations));
              
            }
            return accommodationStats;
        }
        public List<AccommodationStat> GetMonthlyAccommodationStats(List<AccommodationReservation> reservations,string selectedYear)
        {
            List<AccommodationStat> accommodationStats = new List<AccommodationStat>();
            for (int i = 1; i <= 12; i++)
            {
                accommodationStats.Add(CreateAccommodationStatForMonth(i, reservations, selectedYear));
            }
            return accommodationStats;
        }
        private AccommodationStat CreateAccommodationStatForYear(int year,List<AccommodationReservation> reservations)
        {
            AccommodationStat accommodationStat = new AccommodationStat
            {
                NumberOfReservations = reservations.Count(r => r.ReservedFrom.Year == year),
                NumberOfCancelledReservations = reservations.Where(r => r.ReservedFrom.Year == year).Sum(r => r.Cancelled),
                NumberOfRecommendedRenovations = reservations.Where(r => r.ReservedFrom.Year == year).Sum(r => r.RecommendedRenovation),
                NumberOfRescheduledReservations = reservations.Where(r => r.ReservedFrom.Year == year).Sum(r => r.RescheduledReservation),
                Busyness = CalculateYearlyBusyness(year,reservations),
                RowHeader = $"Year: {year}"
            };

            return accommodationStat;
        }
        private AccommodationStat CreateAccommodationStatForMonth(int month, List<AccommodationReservation> reservations,string selectedYear)
        {
            AccommodationStat accommodationStat = new AccommodationStat
            {
                NumberOfReservations = reservations.Where(r => r.ReservedFrom.Year.ToString() == selectedYear).Count(r => r.ReservedFrom.Month == month),
                NumberOfCancelledReservations = reservations.Where(r => r.ReservedFrom.Month == month && r.ReservedFrom.Year.ToString() == selectedYear).Sum(r => r.Cancelled),
                NumberOfRecommendedRenovations = reservations.Where(r => r.ReservedFrom.Month == month && r.ReservedFrom.Year.ToString() == selectedYear).Sum(r => r.RecommendedRenovation),
                NumberOfRescheduledReservations = reservations.Where(r => r.ReservedFrom.Month == month && r.ReservedFrom.Year.ToString() == selectedYear).Sum(r => r.RescheduledReservation),
                Busyness = CalculateMonthlyBusyness(month,selectedYear,reservations),
                RowHeader = $"Month: {CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month)}"
            };

            return accommodationStat;
        }

        private double CalculateYearlyBusyness(int year,List<AccommodationReservation> reservations)
        {
            int numberOfDaysInYear = GetNumberOfDaysInYear(year);
            int numberOfReservationsinYear = GetNumberOfReservedDaysInYear(year,reservations);
            double busyness = (double)numberOfReservationsinYear / numberOfDaysInYear;
            return busyness;
        }
        private double CalculateMonthlyBusyness(int month,string selectedYear,List<AccommodationReservation> reservations)
        {
            int numberOfDaysInMonth = GetNumberOfDaysInMonth(month,selectedYear);
            int numberOfReservationsInMonth = GetNumberOfReservationsInMonth(month,reservations,selectedYear);
            double busyness = (double)numberOfReservationsInMonth / numberOfDaysInMonth;
            Debug.WriteLine(numberOfReservationsInMonth);
            return busyness;
        }
        private int GetNumberOfDaysInYear(int currentYear)
        {
            return DateTime.IsLeapYear(currentYear) ? 366 : 365;
        }
        private int GetNumberOfDaysInMonth(int month,string selectedYear)
        {
            return DateTime.DaysInMonth(Convert.ToInt32(selectedYear), month);
        }
        private int GetNumberOfReservedDaysInYear(int currentYear,List<AccommodationReservation> reservations)
        {
            return reservations.Where(r => (IsReservationInSelectedYear(r, currentYear)) && IsNotCancelled(r))
                .Sum(r =>
                {
                    if (r.ReservedFrom.Year == r.ReservedTo.Year)
                        return (r.ReservedTo - r.ReservedFrom).Days;
                    else if (r.ReservedFrom.Year != currentYear && r.ReservedTo.Year == currentYear)
                        return (r.ReservedTo - new DateTime(r.ReservedTo.Year, 1, 1)).Days;
                    else if (r.ReservedTo.Year != currentYear && r.ReservedFrom.Year == currentYear)
                        return (new DateTime(r.ReservedFrom.Year, 12, 31) - r.ReservedFrom).Days;
                    else
                        return 0;
                });
        }
        private int GetNumberOfReservationsInMonth(int month,List<AccommodationReservation> reservations,string selectedYear)
        {

            return reservations.Where(r => (IsReservationInSelectedMonth(r, month)) && IsNotCancelled(r))
                .Sum(r => {
                    if (r.ReservedFrom.Year.ToString() != selectedYear && r.ReservedFrom.Month == month)
                        return 0;
                    else if (r.ReservedTo.Month == month && r.ReservedFrom.Month != month)
                        return (r.ReservedTo - new DateTime(r.ReservedTo.Year, r.ReservedTo.Month, 1)).Days;
                    else if (r.ReservedFrom.Month == month && r.ReservedTo.Month != month)
                        return (new DateTime(r.ReservedFrom.Year, r.ReservedFrom.Month, DateTime.DaysInMonth(r.ReservedFrom.Year, r.ReservedFrom.Month)) - r.ReservedFrom).Days;
                    else if (r.ReservedFrom.Month == r.ReservedTo.Month)
                        return (r.ReservedTo - r.ReservedFrom).Days;
                    else return 0;
                });
        }
        private bool IsNotCancelled(AccommodationReservation r)
        {
            return r.Cancelled == 0;
        }

        private bool IsReservationInSelectedYear(AccommodationReservation reservation, int selectedYear)
        {
            return reservation.ReservedFrom.Year == selectedYear || reservation.ReservedTo.Year == selectedYear;
        }

        private bool IsReservationInSelectedMonth(AccommodationReservation reservation, int month)
        {
            return reservation.ReservedFrom.Month == month || reservation.ReservedTo.Month == month;
        }
    }
}
