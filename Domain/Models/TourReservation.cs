using BookingApp.Domain.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ToastNotifications.Position;

namespace BookingApp.Domain.Models
{
    public class TourReservation : ISerializable
    {
        public int Id { get; set; }
        public int TourRealisationId { get; set; }
        public User User { get; set; }
        public TourReservation() { }
        public TourReservation(int id, int tourRealisationId)
        {
            Id = id;
            TourRealisationId = tourRealisationId;
        }
        public TourReservation(int id, int tourRealisationId, User user)
        {
            Id=id;
            User = user;
            TourRealisationId=tourRealisationId;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TourRealisationId = Convert.ToInt32(values[1]);
            User = new User() { Id = Convert.ToInt32(values[2]) };
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), TourRealisationId.ToString(), User.Id.ToString() };
            return csvValues;
        }

    }
}
