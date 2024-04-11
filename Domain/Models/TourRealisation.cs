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
            StartTime = DateTime.ParseExact(values[1], "dd/MM/yyyy HH:mm:ss", CultureInfo.GetCultureInfo("en-US"), DateTimeStyles.None);

            TourId = Convert.ToInt32(values[2]);
            AvailableSeats = Convert.ToInt32(values[3]);
            User = new User() { Id = Convert.ToInt32(values[4]) };
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), StartTime.ToString(), TourId.ToString(), AvailableSeats.ToString(), User.Id.ToString() };
            return csvValues;
        }

        public bool IsOnSelectedDate(DateOnly date)
        {
            Debug.WriteLine("AAAA" +DateOnly.FromDateTime(StartTime).ToString());
            Debug.WriteLine("BBBB" +date.ToString());
            return DateOnly.FromDateTime(StartTime) == date;
        }

    }
}
