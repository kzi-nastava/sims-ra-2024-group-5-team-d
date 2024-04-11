using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels
{
    public class TourViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

       public User User { get; set; }
        public string Description { get; set; }

        public Location Location { get; set; }

        public double Duration { get; set; }

        public string ImagesPath { get; set; }
        public int Capacity { get; set; }
        public LANGUAGE Language { get; set; }


        public TourViewModel() { }

        public TourViewModel(int id, string name, string description, double duration, string imagesPath, Location location, User user)
        {
            Id = id;
            Name = name;
            Description = description;
            Duration = duration;
            ImagesPath = imagesPath;
            Location = location;
            User = user;
        }

        public TourViewModel(int id, string name, string description, Location location, double duration, string imagesPath, int capacity, LANGUAGE language,User user)
        {
            Id = id;
            Name = name;
            Description = description;
            Location = location;
            Duration = duration;
            ImagesPath = imagesPath;
            Capacity = capacity;
            Language = language;
            User = user;
        }
    }
}
