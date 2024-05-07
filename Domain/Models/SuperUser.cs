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
    public class SuperUser: ISerializable
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime ValidFrom { get; set; }
        public SuperUser()
        {
        }
        public SuperUser(int id, int userId, DateTime validFrom)
        {
            Id = id;
            UserId = userId;
            ValidFrom = validFrom;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            UserId = Convert.ToInt32(values[1]);
            ValidFrom = Convert.ToDateTime(values[2]);
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), UserId.ToString(), ValidFrom.ToString() };
            return csvValues;
        }
    }
}
