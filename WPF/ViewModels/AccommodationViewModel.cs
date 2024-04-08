using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels
{
    public class AccommodationViewModel
    {
        private int Id { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public string ImagesPath { get; set; }
        public TYPE Type { get; set; }
        /*private int Capacity { get; set; }
        private int MinStay { get; set; }
        private int CancellationDeadline { get; set; }
        public string ImagesPath { get; set; }
        public User Owner { get; set; }*/

        public AccommodationViewModel()
        {

        }
        public Accommodation ViewModelToModel() {
            return null;
        }
        public AccommodationViewModel(int id,string name,Location location,TYPE type,string imagesPath )
        {

            ImagesPath = imagesPath;
            Id = id;
            Name = name;
            Location = location;
            Type = type;
        }
    }
}
