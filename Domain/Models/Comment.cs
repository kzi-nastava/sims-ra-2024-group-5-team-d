using BookingApp.Domain.Serializer;
using System;
using System.Globalization;

namespace BookingApp.Domain.Models
{
    public class Comment : ISerializable
    {
        public int Id { get; set; }
        public DateTime CreationTime { get; set; }
        public string Text { get; set; }
        public User User { get; set; }

        public Comment() { }

        public Comment(DateTime creationTime, string text, User user)
        {
            CreationTime = creationTime;
            Text = text;
            User = user;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), CreationTime.ToString("d/M/yyyy h:mm:ss tt"), Text, User.Id.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            CreationTime = DateTime.ParseExact(values[1], "d/M/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
            Text = values[2];
            User = new User() { Id = Convert.ToInt32(values[3]) };
        }
    }
}
