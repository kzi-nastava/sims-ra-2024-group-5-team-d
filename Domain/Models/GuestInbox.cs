using BookingApp.Domain.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Models
{
    public class GuestInbox : ISerializable
    {
        public int Id { get; set; }
        public int AccommodationId { get; set; }
        public int ReservationId { get; set; }
        public int UserId { get; set; }
        public DateTime NewReservedFrom { get; set; }
        public DateTime NewReservedTo { get; set; }
        public string ImagesPath { get; set; }
        public string Comment {  get; set; }
        public string AccommodationName {  get; set; }

        public GuestInbox() 
        { 

        }
        public GuestInbox(int id,int idAccommodation, int idReservation, int userId, DateTime newFrom, DateTime newTo, string imagesPath, string comment, string accommodationName)
        {
            Id = id;
            AccommodationId = idAccommodation;
            ReservationId = idReservation;
            UserId = userId;
            NewReservedFrom = newFrom;
            NewReservedTo = newTo;
            ImagesPath = imagesPath;
            Comment = comment;
            AccommodationName = accommodationName;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), AccommodationId.ToString(), ReservationId.ToString(), UserId.ToString(), NewReservedFrom.ToString(), NewReservedTo.ToString(), ImagesPath, Comment, AccommodationName };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            AccommodationId = Convert.ToInt32(values[1]);
            ReservationId = Convert.ToInt32(values[3]);
            UserId = Convert.ToInt32(values[2]);
            NewReservedFrom = Convert.ToDateTime(values[4]);
            NewReservedTo = Convert.ToDateTime(values[5]);
            ImagesPath = values[6];
            Comment = values[7];
            AccommodationName = values[8];
        }
    }
}
