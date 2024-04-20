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
        public AccommodationRenovation()
        {

        }
        public AccommodationRenovation(int id, int accommodationId, DateTime renovateFrom, DateTime renovateTo)
        {
            Id = id;
            AccommodationId = accommodationId;
            RenovateFrom = renovateFrom;
            RenovateTo = renovateTo;
        }
        public AccommodationRenovation(int accommodationId, DateTime renovateFrom, DateTime renovateTo)
        {
            AccommodationId = accommodationId;
            RenovateFrom = renovateFrom;
            RenovateTo = renovateTo;
        }

        public void FromCSV(string[] values)
        {

            Id = int.Parse(values[0]);
            AccommodationId = int.Parse(values[1]);
            RenovateFrom = DateTime.ParseExact(values[2], "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
            RenovateTo = DateTime.ParseExact(values[3], "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);

        }


        public string[] ToCSV()
        {
            return new string[] { Id.ToString(), AccommodationId.ToString(), RenovateFrom.ToString("M/d/yyyy h:mm:ss tt"), RenovateTo.ToString("M/d/yyyy h:mm:ss tt") };
        }
    }
}
