using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookingApp.Model
{
    public class TourReservation
    {
        public int TourGuestId { get; set; }
        public int TourMaintenanceId { get; set; }
        public TourReservation() { }
        public TourReservation(int tourGuestId, int tourMaintenanceId)
        {
            TourGuestId = tourGuestId;
            TourMaintenanceId = tourMaintenanceId;
        }
        public void FromCSV(string[] values)
        {
            TourGuestId = Convert.ToInt32(values[0]);
            TourMaintenanceId = Convert.ToInt32(values[1]);
        }

        public string[] ToCSV()
        {
            string[] csvValues = { TourGuestId.ToString(), TourMaintenanceId.ToString() };
            return csvValues;
        }

    }
}
