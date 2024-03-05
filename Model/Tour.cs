using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BookingApp.Model

{
    public enum LANGUAGE { ENGLISH, SERBIAN , GERMAN};
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

        public Tour() { }

        public Tour(string name, string location, string description, LANGUAGE language, int capacity, double duration, string imagesPath)
        {
            Name = name;
            Location = location;
            Description = description;
            Language = language;
            MaxCapacity = capacity;
            Duration = duration;
            ImagesPath = imagesPath;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            Location = new Location() { Id = Convert.ToInt32(values[2]) };
            Description = values[3];
            Language = (LANGUAGE)Convert.ToInt32(values[4]);
            MaxCapacity = Convert.ToInt32(values[5]);
            Duration = Convert.ToDouble(values[6]);
            ImagesPath = values[7];
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Name, Location.Id.ToString(), Description,Language.ToString(), MaxCapacity.ToString(),Duration.ToString(),ImagesPath };
            return csvValues;
        }
    }
}
