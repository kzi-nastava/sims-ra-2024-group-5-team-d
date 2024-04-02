using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookingApp.Model
{
    public  class AccommodationReservation : ISerializable
    {
        public int Id { get; set; }
        public int AccommodationId { get; set; }
        public int UserId { get; set; } 
        public DateTime ReservedFrom { get; set; }
        public DateTime ReservedTo { get; set; }
        public int Cancelled { get; set; }
        public int RescheduledReservation { get; set; }
        public int RecommendedRenovation { get; set; }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            AccommodationId = Convert.ToInt32(values[1]);
            UserId = Convert.ToInt32(values[2]);
            ReservedFrom = DateTime.ParseExact(values[3], "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
            ReservedTo = DateTime.ParseExact(values[4], "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
            Cancelled = Convert.ToInt32(values[5]);
            RescheduledReservation = Convert.ToInt32(values[6]);
            RecommendedRenovation = Convert.ToInt32(values[7]);
        }
        public  AccommodationReservation()
        {
        }

        public AccommodationReservation(int accommodationId, int userId, DateTime reservedFrom, DateTime reservedTo, int cancelled, int rescheduledReservation, int recommendedRenovation)
        {
            AccommodationId = accommodationId;
            UserId = userId;
            ReservedFrom = reservedFrom;
            ReservedTo = reservedTo;
            Cancelled = cancelled;
            RescheduledReservation = rescheduledReservation;
            RecommendedRenovation = recommendedRenovation;
        }
        public AccommodationReservation(int accommodationId,int userId, DateTime reservedFrom, DateTime reservedTo)
        {
            AccommodationId = accommodationId;
            UserId = userId;
            ReservedFrom = reservedFrom;
            ReservedTo = reservedTo;
            Cancelled = 0;
            RescheduledReservation = 0;
            RecommendedRenovation = 0;
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), AccommodationId.ToString(), UserId.ToString(), ReservedFrom.ToString(), ReservedTo.ToString(),Cancelled.ToString(), RescheduledReservation.ToString(),RecommendedRenovation.ToString() };
            return csvValues;
        }
    }   

}
