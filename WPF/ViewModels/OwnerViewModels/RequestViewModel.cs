using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    public class RequestViewModel
    {
        public int RequestId { get; set; }
        public string GuestName { get; set; }
        public string AccommodationName { get; set; }
        public Location Location { get; set; }
        public DateTime PreviousCheckIn { get; set; }
        public DateTime PreviousCheckOut { get; set; }
        public DateTime NewCheckIn { get; set; }
        public DateTime NewCheckOut { get; set; }
        public string Message { get; set; }

        public RequestViewModel(int requestId, string guestName, string accommodationName, Location location, DateTime previousCheckIn, DateTime previousCheckOut, DateTime newCheckIn, DateTime newCheckOut, string message)
        {
            RequestId = requestId;
            GuestName = guestName;
            AccommodationName = accommodationName;
            Location = location;
            PreviousCheckIn = previousCheckIn;
            PreviousCheckOut = previousCheckOut;
            NewCheckIn = newCheckIn;
            NewCheckOut = newCheckOut;
            Message = message;
        }

    }
}
