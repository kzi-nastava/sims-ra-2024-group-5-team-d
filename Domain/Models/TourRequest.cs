using BookingApp.Domain.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BookingApp.Domain.Models
{
    public enum STATE{
        PENDING,
        ACCEPTED,
        INVALID
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
        public int TourReservationId { get; set; }
        public int Capacity { get; set; }

        public TourRequest() { }
        public TourRequest(int id, int userId, STATE status, Location location, string description, LANGUAGE language, DateTime rangeFrom, DateTime rangeTo,int tourReservationId, int capacity)
        {
            Id = id;
            TouristId = userId;
            Status = status;
            Location = location;
            Description = description;
            Language = language;
            RangeFrom = rangeFrom;
            RangeTo = rangeTo;
            TourReservationId = tourReservationId;
            Capacity = capacity;
        }

        public string[] ToCSV()
        {
            return new string[] { Id.ToString(), TouristId.ToString(), Status.ToString(), Location.Id.ToString(), Description, Language.ToString(), RangeFrom.ToString("d.M.yyyy", CultureInfo.InvariantCulture), RangeTo.ToString("d.M.yyyy", CultureInfo.InvariantCulture), TourReservationId.ToString(), Capacity.ToString() };
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TouristId = Convert.ToInt32(values[1]);
            Status = (STATE)Enum.Parse(typeof(STATE), values[2]);
            Location = new Location() { Id = Convert.ToInt32(values[3]) };
            Description = Convert.ToString(values[4]);
            Language = (LANGUAGE)Enum.Parse(typeof(LANGUAGE), values[5]);
            RangeFrom = DateTime.ParseExact(values[6], "d.M.yyyy", CultureInfo.InvariantCulture);
            RangeTo = DateTime.ParseExact(values[7], "d.M.yyyy", CultureInfo.InvariantCulture);
            TourReservationId = Convert.ToInt32(values[8]);
            Capacity = Convert.ToInt32(values[9]);
        }

        public bool IsAcceptable()
        {
            return DateTime.Now <= RangeFrom.AddDays(-2);
        }
        public bool IsAcceptableInsideComplex()
        {
            return Status == STATE.PENDING;
        }
    }
}
