using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    class ProfileAccommodationViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
        public int Capacity { get; set; }
        public string ImagesPath { get; set; }
        public ProfileAccommodationViewModel(Accommodation accommodation)
        {
            Id = accommodation.Id;
            Name = accommodation.Name;
            Location = accommodation.Location.ToString();
            Type = accommodation.Type.ToString();
            Capacity = accommodation.Capacity;
            ImagesPath = accommodation.ImagesPath;
        }
    }
}
