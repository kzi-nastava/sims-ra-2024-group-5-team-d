using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Serializer;

namespace BookingApp.Model
{
    public enum Type
    {
        APARTMAN,
        HOTEL,
        COTTAGE
    }
    public class Accommodation: ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public Type Type { get; set; }
        public int Capacity { get; set; }
        public int MinStay { get; set; }
        public int CancellationDeadline { get; set; }
        public string ImagesPath { get; set; }
        public User Owner { get; set; }

        public Accommodation() {
        }

        public Accommodation(string name, string location,Type type,int minStay,int cancellationDeadline, int capacity,string imagesPath, User owner)
        {
            Name = name;
            Location = location;
            Type = type;
            Capacity = capacity;
            MinStay = minStay;
            CancellationDeadline = cancellationDeadline;
            ImagesPath = imagesPath;
            Owner = owner;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Name, Location.Id.toString(), Type.ToString(), Capacity.ToString(),MinStay.ToString(),CancellationDeadline.ToString(), ImagesPath, Owner.Id.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            Location = new Location() { Id = Convert.ToInt32(values[2]) };      
            Type = (Type)Enum.Parse(typeof(Type), values[3]);
            Capacity = Convert.ToInt32(values[4]);
            MinStay = Convert.ToInt32(values[5]);
            CancellationDeadline = Convert.ToInt32(values[6]);
            ImagesPath = values[7];
            Owner = new User() { Id = Convert.ToInt32(values[8]) };
        }
    }
    
}
