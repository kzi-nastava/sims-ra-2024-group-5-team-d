using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookingApp.Model
{
    public class TourGuest : ISerializable
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int Years { get; set; }
        public int TourReservationId { get; set; }
        public int CheckPointId { get; set; }
        public TourGuest() { }
        public TourGuest(int id, string fullName, int years, int tourReservationId, int checkPointId)
        {
            Id = id;
            FullName = fullName;
            Years = years;
            TourReservationId = tourReservationId;
            CheckPointId = checkPointId;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            FullName = values[1];
            Years = Convert.ToInt32(values[2]);
            TourReservationId = Convert.ToInt32(values[3]);
            CheckPointId= Convert.ToInt32(values[4]);

        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), FullName, Years.ToString(), TourReservationId.ToString(),CheckPointId.ToString() };
            return csvValues;
        }

    }
}
