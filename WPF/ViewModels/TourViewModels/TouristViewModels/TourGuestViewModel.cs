using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels
{
    public class TourGuestViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int Years { get; set; }
        public int TourReservationId { get; set; }
        public int CheckPointId { get; set; }
        public string PersonalID { get; set; }
        public string TouristHeader { get; set; }
        public bool IsReadOnly { get; set; }
        public TourGuestViewModel() { }
        public TourGuestViewModel(int id, string fullName, int years, int tourReservationId, int checkPointId)
        {
            Id = id;
            FullName = fullName;
            Years = years;
            TourReservationId = tourReservationId;
            CheckPointId = checkPointId;
        }

        public TourGuestViewModel(int id, int tourReservationId, string touristHeader)
        {
            Id = id;
            TourReservationId = tourReservationId;
            FullName = "";
            Years = 0;
            PersonalID = "";
            TourReservationId = tourReservationId;
            CheckPointId = -1;
            TouristHeader = touristHeader;
        }
    }
}
