using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookingApp.Model
{
    public class TourRealisation : ISerializable
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public int TourId { get; set; }
        public int AvailableSeats { get; set; }
        public User User { get; set; }

        public TourRealisation() { }
        public TourRealisation(DateTime startTime, int tourId, int availableSeats, User user)
        {
            StartTime = startTime;
            TourId = tourId;
            AvailableSeats = availableSeats;
            User = user;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            StartTime = Convert.ToDateTime(values[1]);
            TourId = Convert.ToInt32(values[2]);
            AvailableSeats = Convert.ToInt32(values[3]);
            User = new User() { Id = Convert.ToInt32(values[4]) };
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), StartTime.ToString(), TourId.ToString(), AvailableSeats.ToString(),User.Id.ToString() };
            return csvValues;
        }

    }
}
