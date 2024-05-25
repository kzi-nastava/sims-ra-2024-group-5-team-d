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
    public class SuperUser: ISerializable
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime ValidFrom { get; set; }
        public int BonusPoints { get; set; }
        public LANGUAGE Language { get; set; }
        public SuperUser()
        {
        }
        public SuperUser(int id, int userId, DateTime validFrom)
        {
            Id = id;
            UserId = userId;
            ValidFrom = validFrom;
            BonusPoints = 5;
        }
        public SuperUser(int userId, DateTime validFrom)
        {
            UserId = userId;
            ValidFrom = validFrom;
            BonusPoints = 5;
        }
        public SuperUser(int userId, DateTime validFrom,LANGUAGE language)
        {
            UserId = userId;
            ValidFrom = validFrom;
            Language = language;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            UserId = Convert.ToInt32(values[1]);
            ValidFrom = DateTime.ParseExact(values[2], "d/M/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
            BonusPoints = Convert.ToInt32(values[3]);
            Language = (LANGUAGE)Enum.Parse(typeof(LANGUAGE), values[4]);
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), UserId.ToString(), ValidFrom.ToString("d/M/yyyy h:mm:ss tt", CultureInfo.InvariantCulture), BonusPoints.ToString(),Language.ToString() };
            return csvValues;
        }
        public bool IsStillValid()
        {
            return ValidFrom >= DateTime.UtcNow.AddYears(-1);
        }
    }
}
