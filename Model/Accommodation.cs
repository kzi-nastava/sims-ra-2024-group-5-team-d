using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Serializer;

namespace BookingApp.Model
{
    public class Accommodation: ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
        public int Capacity { get; set; }
        public int MinStay { get; set; }
        public int CancelationDeadline { get; set; }
        public string ImagesPath { get; set; }
        public User Owner { get; set; }

        public Accommodation() {
        }

        public Accommodation(string name, string location,string type,int minStay,int cancelationDeadline, int capacity,string imagesPath, User owner)
        {
            Name = name;
            Location = location;
            Type = type;
            Capacity = capacity;
            MinStay = minStay;
            CancelationDeadline = cancelationDeadline;
            ImagesPath = imagesPath;
            Owner = owner;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Name, Location, Type, Capacity.ToString(),MinStay.ToString(),CancelationDeadline.ToString(), ImagesPath, Owner.Id.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            Location = values[2];
            Type = values[3];
            Capacity = Convert.ToInt32(values[4]);
            MinStay = Convert.ToInt32(values[5]);
            CancelationDeadline = Convert.ToInt32(values[6]);
            ImagesPath = values[7];
            Owner = new User() { Id = Convert.ToInt32(values[8]) };
        }
    }
    
}
