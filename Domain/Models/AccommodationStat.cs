using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Models
{
    public class AccommodationStat
    {
        public int NumberOfReservations { get; set; }
        public int NumberOfCancelledReservations { get; set; }
        public int NumberOfRescheduledReservations { get; set; }
        public int NumberOfRecommendedRenovations { get; set; }
        public string Year { get; set; }
        public double Busyness { get; set; }
        public AccommodationStat() { }
        public AccommodationStat(int numberOfReservations, int numberOfCancelledReservations, string year, int nUmberOfRescheduledReservations, int numberOfRecommendedRenovations)
        {
            NumberOfReservations = numberOfReservations;
            NumberOfCancelledReservations = numberOfCancelledReservations;
            Year = year;
            NumberOfRescheduledReservations = nUmberOfRescheduledReservations;
            NumberOfRecommendedRenovations = numberOfRecommendedRenovations;
        }
    }
}
