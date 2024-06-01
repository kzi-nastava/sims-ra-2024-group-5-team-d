using BookingApp.Domain.Serializer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookingApp.Domain.Models
{
    public class TourRealisation : ISerializable
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public int TourId { get; set; }
        public int AvailableSeats { get; set; }
        public User User { get; set; }
        public bool IsFinished { get; set; }
        public bool IsLive { get; set; }

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
            string[] formats = { "d/M/yyyy HH:mm:ss", "d.M.yyyy HH:mm:ss" };
            if (DateTime.TryParseExact(values[1], formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startTime))
            {
                StartTime = startTime;
            }
            TourId = Convert.ToInt32(values[2]);
            AvailableSeats = Convert.ToInt32(values[3]);
            User = new User() { Id = Convert.ToInt32(values[4]) };
            IsFinished = Convert.ToBoolean(values[5]);
            IsLive = Convert.ToBoolean(values[6]);
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), StartTime.ToString("d/M/yyyy HH:mm:ss"), TourId.ToString(), AvailableSeats.ToString(), User.Id.ToString(), IsFinished.ToString(), IsLive.ToString() };
            return csvValues;
        }


        public bool IsCancellable()
        {
            return StartTime.AddDays(-2) > DateTime.Now;
        }
        public bool IsOnSelectedDate(DateOnly date)
        {
            return DateOnly.FromDateTime(StartTime) == date;
        }

        
    }
}
