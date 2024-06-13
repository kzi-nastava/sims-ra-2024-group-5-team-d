using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    public class UnratedGuestViewModel
    {
        public int ReservationId { get; set; }
        public string FullName { get; set; }
        public string Surname { get; set; }
        public DateTime ReservedFrom { get; set; }
        public DateTime ReservedTo { get; set; }
        public Location Location { get; set; }
        public string AccommodationName { get; set; }
        public string AvatarPath { get; set; }

        public UnratedGuestViewModel()
        {

        }
        public UnratedGuestViewModel(string fullName, string avatarPath)
        {
            FullName = fullName;
            AvatarPath = avatarPath;
        }
        public UnratedGuestViewModel(int reservationId, string name, DateTime reservedFrom, DateTime reservedTo, Location location, string accommodationName,string avatarPath)
        {
            ReservationId = reservationId;
            FullName = name;
            ReservedFrom = reservedFrom;
            ReservedTo = reservedTo;
            Location = location;
            AccommodationName = accommodationName;
            AvatarPath = avatarPath;
        }
    }
}
