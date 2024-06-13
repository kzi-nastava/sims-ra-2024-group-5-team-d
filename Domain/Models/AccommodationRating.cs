using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.Serializer;

namespace BookingApp.Domain.Models
{

    public class AccommodationRating: ISerializable
    {
        public int Id { get; set; }
        public int AccommodationId { get; set; }
        public int GuestId { get; set; }
        public int ReservationId { get; set; }
        public int Cleanliness { get; set; }
        public int Correctness {  get; set; }
        public DateOnly TimeOfRating { get; set; }
        public int LevelOfRenovation { get; set; }
        public string Comment { get; set; }
        public string ImagesPath { get; set; }

        public AccommodationRating() 
        {
        
        }
        public AccommodationRating(int accommodationId, int guestId, int reservationId, int cleanlinessRating, int correctness, string comment, DateOnly timeOfRating, string imagesPath, int levelOfRenovation)
        {
            AccommodationId = accommodationId;
            GuestId = guestId;
            ReservationId = reservationId;
            Cleanliness = cleanlinessRating;
            Correctness = correctness;
            Comment = comment;
            TimeOfRating = timeOfRating;
            ImagesPath = imagesPath;
            LevelOfRenovation = levelOfRenovation;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            AccommodationId = Convert.ToInt32(values[1]);
            GuestId = Convert.ToInt32(values[2]);
            ReservationId = Convert.ToInt32(values[3]);
            Cleanliness = Convert.ToInt32(values[4]);
            Correctness = Convert.ToInt32(values[5]);
            Comment = values[6];
            TimeOfRating = DateOnly.ParseExact(values[7], "d/M/yyyy", CultureInfo.InvariantCulture);
            ImagesPath = values[8];
            LevelOfRenovation = Convert.ToInt32(values[9]);
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), AccommodationId.ToString(), GuestId.ToString(), ReservationId.ToString(), Cleanliness.ToString(), Correctness.ToString(), Comment, TimeOfRating.ToString("d/M/yyyy"), ImagesPath, LevelOfRenovation.ToString() };
            return csvValues;
        }
        public double GetAverageRating()
        {
            return (double)(Cleanliness+Correctness)/2;
        }
    }
}
