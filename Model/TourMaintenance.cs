using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookingApp.Model
{
    public class TourMaintenance
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public int TourId { get; set; }
        public int AvailableSeats { get; set; }

        public TourMaintenance() { }
        public TourMaintenance(int id, DateTime startTime, int tourId, int availableSeats)
        {
            Id = id;
            StartTime = startTime;
            TourId = tourId;
            AvailableSeats = availableSeats;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            StartTime = Convert.ToDateTime(values[1]);
            TourId = Convert.ToInt32(values[2]);
            AvailableSeats = Convert.ToInt32(values[3]);
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), StartTime.ToString(), TourId.ToString(), AvailableSeats.ToString() };
            return csvValues;
        }

    }
}
