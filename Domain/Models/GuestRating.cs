using BookingApp.Domain.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookingApp.Domain.Models
{
    public class GuestRating : ISerializable
    {
        public int Id { get; set; }
        public int AccommodationId { get; set; }
        public int GuestId { get; set; }
        public int ReservationId { get; set; }
        public int CleanlinessRating { get; set; }
        public int RuleComplianceRating { get; set; }
        public string Comment { get; set; }
        public GuestRating()
        {
        }
        public GuestRating(int accommodationId, int guestId, int reservationId, int cleanlinessRating, int ruleComplianceRating, string comment)
        {
            AccommodationId = accommodationId;
            GuestId = guestId;
            ReservationId = reservationId;
            CleanlinessRating = cleanlinessRating;
            RuleComplianceRating = ruleComplianceRating;
            Comment = comment;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            AccommodationId = Convert.ToInt32(values[1]);
            GuestId = Convert.ToInt32(values[2]);
            ReservationId = Convert.ToInt32(values[3]);
            CleanlinessRating = Convert.ToInt32(values[4]);
            RuleComplianceRating = Convert.ToInt32(values[5]);
            Comment = values[6];
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), AccommodationId.ToString(), GuestId.ToString(), ReservationId.ToString(), CleanlinessRating.ToString(), RuleComplianceRating.ToString(), Comment };
            return csvValues;
        }
    }
}
