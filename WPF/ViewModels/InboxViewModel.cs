using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels
{
    public class InboxViewModel
    {
        public string ImagesPath { get; set; } 
        public DateTime NewReservedFrom { get; set; }
        public DateTime NewReservedTo { get; set; }
        public string Commentar { get; set; }
        public string AccommodationName { get; set; }
        public InboxViewModel() 
        {

        }
        public InboxViewModel(string imagesPath, DateTime newReservedFrom, DateTime newReservedTo, string commentar, string accommodationName)
        {
            ImagesPath = imagesPath;
            NewReservedFrom = newReservedFrom;
            NewReservedTo = newReservedTo;
            Commentar = commentar;
            AccommodationName = accommodationName;
        }
    }
}
