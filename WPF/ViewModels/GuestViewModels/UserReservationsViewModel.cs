using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels
{
    public class UserReservationsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public string ImagesPath { get; set; }
        public int Capacity { get; set; }
        public DateTime ReservedFrom { get; set; }
        public DateTime ReservedTo { get; set; }
        public bool isReseravtionCancellable { get; set; }
        public bool isReservationRateable { get; set; }


        public UserReservationsViewModel() 
        {
        
        }
        public UserReservationsViewModel(int id, string name, Location location, string imagesPath, int capacity, DateTime reservedFrom, DateTime reservedTo, bool isReseravtionCancellable, bool isReservationRateable)
        {
            Id = id;
            Name = name;
            Location = location;
            ImagesPath = imagesPath;
            Capacity = capacity;
            ReservedFrom = reservedFrom;
            ReservedTo = reservedTo;
            this.isReseravtionCancellable = isReseravtionCancellable;
            this.isReservationRateable = isReservationRateable;
        }
    }

    
}
