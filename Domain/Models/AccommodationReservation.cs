using BookingApp.Domain.Serializer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookingApp.Domain.Models
{
    public class AccommodationReservation : ISerializable
    {
        public int Id { get; set; }
        public int AccommodationId { get; set; }
        public int UserId { get; set; }
        public DateTime ReservedFrom { get; set; }
        public DateTime ReservedTo { get; set; }
        public int Cancelled { get; set; }
        public int RescheduledReservation { get; set; }
        public int RecommendedRenovation { get; set; }
        public int NumberOfPeople { get; set; }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            AccommodationId = Convert.ToInt32(values[1]);
            UserId = Convert.ToInt32(values[2]);
            ReservedFrom = DateTime.ParseExact(values[3], "d/M/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
            ReservedTo = DateTime.ParseExact(values[4], "d/M/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
            Cancelled = Convert.ToInt32(values[5]);
            RescheduledReservation = Convert.ToInt32(values[6]);
            RecommendedRenovation = Convert.ToInt32(values[7]);
            NumberOfPeople = Convert.ToInt32(values[8]);
        }
        public AccommodationReservation()
        {
        }

        public AccommodationReservation(int accommodationId, int userId, DateTime reservedFrom, DateTime reservedTo, int cancelled, int rescheduledReservation, int recommendedRenovation,int numberOfPeople)
        {
            AccommodationId = accommodationId;
            UserId = userId;
            ReservedFrom = reservedFrom;
            ReservedTo = reservedTo;
            Cancelled = cancelled;
            RescheduledReservation = rescheduledReservation;
            RecommendedRenovation = recommendedRenovation;
            NumberOfPeople = numberOfPeople;

        }
        public AccommodationReservation(int accommodationId, int userId, DateTime reservedFrom, DateTime reservedTo,int numberOfPeople)
        {
            AccommodationId = accommodationId;
            UserId = userId;
            ReservedFrom = reservedFrom;
            ReservedTo = reservedTo;
            Cancelled = 0;
            RescheduledReservation = 0;
            RecommendedRenovation = 0;
            NumberOfPeople = numberOfPeople;
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), AccommodationId.ToString(), UserId.ToString(), ReservedFrom.ToString("d/M/yyyy h:mm:ss tt"), ReservedTo.ToString("d/M/yyyy h:mm:ss tt"), Cancelled.ToString(), RescheduledReservation.ToString(), RecommendedRenovation.ToString(),NumberOfPeople.ToString() };
            return csvValues;
        }
        public bool IsCanceled()
        {
            return Cancelled == 1;
        }

        public bool IsActive()
        {
            return !IsCanceled() && DateTime.Now < ReservedTo;
        }

        public bool IsFinished()
        {
            return !IsCanceled() && DateTime.Now > ReservedTo;
        }
        public bool IsRateable()
        {
            return DateTime.Now>ReservedTo && ReservedTo.AddDays(5) >= DateTime.Now;
        }
        public bool IsCancellable(int cancellationDeadLine)
        {
            DateTime currentTime = DateTime.Now;

            if (ReservedFrom > (currentTime.AddHours(24)) && ReservedFrom > (currentTime.AddDays(cancellationDeadLine)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public int CalculateNumberOfReservedDays()
        {
            return (ReservedTo-ReservedFrom).Days;
        }
        public bool IsMadeOrEndedInSelectedYear(int year)
        {
            return ReservedFrom.Year == year || ReservedTo.Year == year;
        }
        public bool IsMadeInSelectedYear(int year)
        {
            return ReservedFrom.Year == year;
        }
        public bool IsMadeInSelectedMonth(int month)
        {
            return ReservedFrom.Month == month;
        }
        public bool IsInSelectedMonth(int month)
        {
            return ReservedFrom.Month == month || ReservedTo.Month == month;
        }
        /*public int CalculateNumberOfDaysInSelectedYear(int year)
        {
            if (ReservedFrom.Year == ReservedTo.Year)
                return (ReservedTo - ReservedFrom).Days;
            else if (ReservedFrom.Year != year && ReservedTo.Year == year)
                return (ReservedTo - new DateTime(ReservedTo.Year, 1, 1)).Days;
            else if (ReservedTo.Year != year && ReservedFrom.Year == year)
                return (new DateTime(ReservedFrom.Year, 12, 31) - ReservedFrom).Days;
            else
                return 0;
        }*/
        public int CalculateNumberOfDaysInSelectedYear(int year)
        {
            if (IsReservationInSameYear(year))
            {
                return CalculateDaysForSameYear();
            }
            else if (IsReservationStartingPreviousYear(year))
            {
                return CalculateDaysForReservationStartingPreviousYear(year);
            }
            else if (IsReservationEndingNextYear(year))
            {
                return CalculateDaysForReservationEndingNextYear(year);
            }

            return 0;
        }

        private bool IsReservationInSameYear(int year)
        {
            return ReservedFrom.Year == year && ReservedTo.Year == year;
        }

        private bool IsReservationStartingPreviousYear(int year)
        {
            return ReservedFrom.Year < year && ReservedTo.Year == year;
        }

        private bool IsReservationEndingNextYear(int year)
        {
            return ReservedFrom.Year == year && ReservedTo.Year > year;
        }

        private int CalculateDaysForSameYear()
        {
            return (ReservedTo - ReservedFrom).Days;
        }

        private int CalculateDaysForReservationStartingPreviousYear(int year)
        {
            return (ReservedTo - new DateTime(year, 1, 1)).Days;
        }

        private int CalculateDaysForReservationEndingNextYear(int year)
        {
            return (new DateTime(year, 12, 31) - ReservedFrom).Days;
        }
        public int CalculateNumberOfDaysInSelectedMonthInYear(int month, int year)
        {
            if (ReservedFrom.Year != year && ReservedFrom.Month == month)
                return 0;
            else if (ReservedTo.Month == month && ReservedFrom.Month != month)
                return (ReservedTo - new DateTime(ReservedTo.Year, ReservedTo.Month, 1)).Days;
            else if (ReservedFrom.Month == month && ReservedTo.Month != month)
                return (new DateTime(ReservedFrom.Year, ReservedFrom.Month, DateTime.DaysInMonth(ReservedFrom.Year, ReservedFrom.Month)) - ReservedFrom).Days;
            else if (ReservedFrom.Month == ReservedTo.Month)
                return (ReservedTo - ReservedFrom).Days;
            else return 0;
        }
        public bool IsOutOfRange(DateTime fromDate, DateTime toDate)
        {
            return fromDate > ReservedTo || toDate < ReservedFrom;
        }
        //MOZDA JE OVA BOLJA OD GORNJE
        /*public int CalculateNumberOfDaysInSelectedMonthInYear(int month, int year)
{
    DateTime startOfMonth = new DateTime(year, month, 1);
    DateTime endOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));

    if (ReservedFrom > endOfMonth || ReservedTo < startOfMonth)
        return 0;
    else if (ReservedFrom >= startOfMonth && ReservedTo <= endOfMonth)
        return (ReservedTo - ReservedFrom).Days;
    else if (ReservedFrom <= startOfMonth && ReservedTo >= endOfMonth)
        return DateTime.DaysInMonth(year, month);
    else if (ReservedFrom <= startOfMonth)
        return (ReservedTo - startOfMonth).Days;
    else if (ReservedTo >= endOfMonth)
        return (endOfMonth - ReservedFrom).Days;
    else
        return 0;
}*/
        /*
         *  public bool IsReservationRated(List<GuestRating> guestRatings)
        {
            return guestRatings.Any(q => q.ReservationId == Id);
        }
        public bool IsReservationRated(int reservationId)
        {
            return reservationId == Id;
        }*/
        public bool IsInLastYear()
        {
            return ReservedFrom<DateTime.UtcNow && ReservedFrom>DateTime.UtcNow.AddYears(-1) && ReservedTo<DateTime.UtcNow;
        }
    }

}
