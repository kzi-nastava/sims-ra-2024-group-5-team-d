using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels
{
    public class TourAttendeeViewModel
    {
        public int GuestId { get; set; }
        public string FullName { get; set; }

        public bool IsPresent { get; set; }

        public string CheckpointName { get; set; }

        public TourAttendeeViewModel(int touristId, string fullName)
        {
            GuestId = touristId;
            FullName = fullName;
        }
    
    }
}
