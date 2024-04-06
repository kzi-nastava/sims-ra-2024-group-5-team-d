using BookingApp.Domain.Serializer;
using System;

namespace BookingApp.Domain.Models
{
    public class User : ISerializable
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Type { get; set; }


        public User() { }

        public User(string username, string password, string type)
        {
            Username = username;
            Password = password;
            Type = type;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Username, Password, Type };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Username = values[1];
            Password = values[2];
            Type = values[3];
        }
    }
}
