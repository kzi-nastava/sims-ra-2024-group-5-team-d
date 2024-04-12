using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels
{
    public class RequestViewModel
    {
        public int RequestId { get; set; }
        public string GuestName { get; set; }
        public string AccommodationName { get; set; }
        public Location Location { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public string Message { get; set; }
      /*  public RequestViewModel(Request request)
        {
            RequestId = request.RequestId;
            GuestName = request.Guest.FirstName + " " + request.Guest.LastName;
            AccommodationName = request.Accommodation.Name;
            Location = request.Accommodation.Location;
            CheckIn = request.CheckIn;
            CheckOut = request.CheckOut;
            Message = request.Message;
        }*/
    }
}
