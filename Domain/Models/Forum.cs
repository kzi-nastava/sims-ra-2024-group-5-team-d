using BookingApp.Domain.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ToastNotifications.Position;

namespace BookingApp.Domain.Models
{
    public class Forum : ISerializable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Location Location { get; set; }
        public int IdUser { get; set; }
        public DateTime DateCreated{ get; set; }
        public bool Active { get; set; }
        public Forum()
        {

        }
        public Forum(int id, string title, string description, Location location, int idUser, DateTime dateCreated, bool active)
        {
            Id = id;
            Title = title;
            Description = description;
            Location = location;
            IdUser = idUser;
            DateCreated = dateCreated;
            Active = active;
        }
        public Forum( string title, string description, Location location, int idUser, DateTime dateCreated, bool active)
        {
            
            Title = title;
            Description = description;
            Location = location;
            IdUser = idUser;
            DateCreated = dateCreated;
            Active = active;
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Title, Description, Location.Id.ToString(), IdUser.ToString(), DateCreated.ToString("d/M/yyyy h:mm:ss tt"), Active.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Title = values[1];
            Description = values[2];
            Location = new Location() { Id = Convert.ToInt32(values[3]) };
            IdUser = Convert.ToInt32(values[4]);
            DateCreated = DateTime.ParseExact(values[5], "d/M/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
            Active = Convert.ToBoolean(values[6]);
        }
        public bool IsActive() {
            return Active == true;
        }
    }
}
