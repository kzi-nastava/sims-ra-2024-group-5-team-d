using BookingApp.Domain.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BookingApp.Domain.Models

{
    public enum LANGUAGE { ENGLISH, SERBIAN, GERMAN };
    public class Tour : ISerializable

    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public string Description { get; set; }
        public LANGUAGE Language { get; set; }
        public int MaxCapacity { get; set; }
        public double Duration { get; set; }
        public string ImagesPath { get; set; }
        public User User { get; set; }
        public Tour() { }

        public Tour(string name, Location location, string description, LANGUAGE language, int capacity, double duration, string imagesPath, User user)
        {

            Name = name;
            Location = location;
            Description = description;
            Language = language;
            MaxCapacity = capacity;
            Duration = duration;
            ImagesPath = imagesPath;
            User = user;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            Location = new Location() { Id = Convert.ToInt32(values[2]) };
            Description = values[3];
            Language = (LANGUAGE)Enum.Parse(typeof(LANGUAGE), values[4]);
            MaxCapacity = Convert.ToInt32(values[5]);
            Duration = Convert.ToDouble(values[6]);
            ImagesPath = values[7];
            User = new User() { Id = Convert.ToInt32(values[8]) };
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Name, Location.Id.ToString(), Description, Language.ToString(), MaxCapacity.ToString(), Duration.ToString(), ImagesPath, User.Id.ToString() };
            return csvValues;
        }
    }
}
