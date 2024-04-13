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
    public class TourRating : ISerializable
    {
        public int Id { get; set; }
        public int TouristLanguage { get; set; }
        public int TouristKnowladge { get; set; }
        public int TourAmusement { get; set; }
        public int TourReservationId { get; set; }
        public string ImagesPath { get; set; }
        public string Comment { get; set; }
        public bool IsValid { get; set; }
        public TourRating() { }
        public TourRating(int touristLanguage, int touristKnowladge, int tourAmusement, int tourReservationId, string imagesPath, string comment, bool isValid)
        {
            TouristLanguage = touristLanguage;
            TouristKnowladge = touristKnowladge;
            TourAmusement = tourAmusement;
            TourReservationId = tourReservationId;
            ImagesPath = imagesPath;
            Comment = comment;
            IsValid = isValid;
        }
        public TourRating(int id, int touristLanguage, int touristKnowladge, int tourAmusement, int tourReservationId, string imagesPath, string comment,bool isValid)
        {
            Id = id;
            TouristLanguage = touristLanguage;
            TouristKnowladge = touristKnowladge;
            TourAmusement = tourAmusement;
            TourReservationId = tourReservationId;
            ImagesPath = imagesPath;
            Comment = comment;
            IsValid = isValid;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), TouristLanguage.ToString(), TouristKnowladge.ToString(), TourAmusement.ToString(), TourReservationId.ToString(), ImagesPath, Comment,IsValid.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TouristLanguage = Convert.ToInt32((string)values[1]);
            TouristKnowladge = Convert.ToInt32((string)values[2]);
            TourAmusement = Convert.ToInt32((string)values[3]);
            TourReservationId = Convert.ToInt32((string)values[4]);
            ImagesPath = Convert.ToString(values[5]);
            Comment = Convert.ToString(values[6]);
            IsValid = Convert.ToBoolean(values[7]);
        }
    }
}
