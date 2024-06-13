using BookingApp.Domain.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Models
{
    public class AccommodationRenovation : ISerializable
    {
        public int Id { get; set; }
        public int AccommodationId { get; set; }
        public DateTime RenovateFrom { get; set; }
        public DateTime RenovateTo { get; set; }
        public string RenovationReason { get; set; }
        public AccommodationRenovation()
        {

        }
        public AccommodationRenovation(int id, int accommodationId, DateTime renovateFrom, DateTime renovateTo, string renovationReason)
        {
            Id = id;
            AccommodationId = accommodationId;
            RenovateFrom = renovateFrom;
            RenovateTo = renovateTo;
            RenovationReason = renovationReason;
        }
        public AccommodationRenovation(int accommodationId, DateTime renovateFrom, DateTime renovateTo,string renovationReason)
        {
            AccommodationId = accommodationId;
            RenovateFrom = renovateFrom;
            RenovateTo = renovateTo;
            RenovationReason = renovationReason;
        }

        public void FromCSV(string[] values)
        {

            Id = int.Parse(values[0]);
            AccommodationId = int.Parse(values[1]);
            RenovateFrom = DateTime.ParseExact(values[2], "d/M/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
            RenovateTo = DateTime.ParseExact(values[3], "d/M/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
            RenovationReason = values[4];
        }

        public bool IsCancelable()
        {
            return DateTime.Now.AddDays(5) <= RenovateFrom;
        }
        public string[] ToCSV()
        {
            return new string[] { Id.ToString(), AccommodationId.ToString(), RenovateFrom.ToString("d/M/yyyy h:mm:ss tt"), RenovateTo.ToString("d/M/yyyy h:mm:ss tt"),RenovationReason };
        }

        public bool IsInRange(DateTime from, DateTime to)
        {
            return from<RenovateTo && to>RenovateFrom;
        }
    }
}
