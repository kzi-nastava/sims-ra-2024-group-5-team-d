using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookingApp.Domain.Models
{
    public class TourReservation : ISerializable
    {
        public int Id { get; set; }
        public int TourRealisationId { get; set; }
        public TourReservation() { }
        public TourReservation(int id, int tourRealisationId)
        {
            Id = id;
            TourRealisationId = tourRealisationId;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TourRealisationId = Convert.ToInt32(values[1]);
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), TourRealisationId.ToString() };
            return csvValues;
        }

    }
}
