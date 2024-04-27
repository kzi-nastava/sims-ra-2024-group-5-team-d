using BookingApp.Domain.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BookingApp.Domain.Models
{
    public enum STATE{
        PENDING,
        ACCEPTED,
        UNVALID
    }
    public class TourRequest : ISerializable
    {
        public int Id { get; set; }
        public int TouristId { get; set; }
        public STATE Status { get; set; }
        public Location Location { get; set; }
        public string Description { get; set; }
        public LANGUAGE Language { get; set; }
        public DateTime RangeFrom { get; set; }
        public DateTime RangeTo { get; set; }
        public int TourRealisationId { get; set; }
        public int Capacity { get; set; }

        public TourRequest() { }
        public TourRequest(int id, int userId, STATE status, Location location, string description, LANGUAGE language, DateTime rangeFrom, DateTime rangeTo, int capacity)
        {
            Id = id;
            TouristId = userId;
            Status = status;
            Location = location;
            Description = description;
            Language = language;
            RangeFrom = rangeFrom;
            RangeTo = rangeTo;
            TourRealisationId = -1;
            Capacity = capacity;
        }

        public string[] ToCSV()
        {
            return new string[] { Id.ToString(), TouristId.ToString(), Status.ToString(), Location.Id.ToString(), Description, Language.ToString(), RangeFrom.ToString("dd/MM/yyyy"), RangeTo.ToString("dd/MM/yyyy"), TourRealisationId.ToString(), Capacity.ToString() };
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TouristId = Convert.ToInt32(values[1]);
            Status = (STATE)Enum.Parse(typeof(STATE), values[2]);
            Location = new Location() { Id = Convert.ToInt32(values[3]) };
            Description = Convert.ToString(values[4]);
            Language = (LANGUAGE)Enum.Parse(typeof(LANGUAGE), values[5]);
            RangeFrom = Convert.ToDateTime(values[6]);
            RangeTo = Convert.ToDateTime(values[7]);
            TourRealisationId = Convert.ToInt32(values[8]);
            Capacity = Convert.ToInt32(values[9]);
        }

        public bool IsAcceptable()
        {
            return DateTime.Now <= RangeFrom.AddDays(-3);
        }
    }
}
