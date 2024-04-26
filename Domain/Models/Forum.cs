using BookingApp.Domain.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Models
{
    public class Forum : ISerializable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Location Location { get; set; }
        public int IdUser { get; set; }
        public DateTime CreatedForum{ get; set; }
        public bool Active { get; set; }
        public Forum()
        {

        }
        public Forum(int id, string title, string description, Location location, int idUser, DateTime createdForum, bool active)
        {
            Id = id;
            Title = title;
            Description = description;
            Location = location;
            IdUser = idUser;
            CreatedForum = createdForum;
            Active = active;

        }

        public string[] ToCSV()
        {
            throw new NotImplementedException();
        }

        public void FromCSV(string[] values)
        {
            throw new NotImplementedException();
        }
    }
}
