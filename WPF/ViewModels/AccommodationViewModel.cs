using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels
{
    public class AccommodationViewModel
    {
        public string Star1 { get; set; }
        public string Star2 { get; set; }
        public string Star3 { get; set; }
        public string Star4 { get; set; }
        public string Star5 { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public string ImagesPath { get; set; }
        public TYPE Type { get; set; }
        public  int Capacity { get; set; }
        public int MinStay { get; set; }
        public bool? IsSuperOwner { get; set; }
        public int NumberOfRatings { get; set; }
        public double AverageRating { get; set; }
        /*private int CancellationDeadline { get; set; }
        public string ImagesPath { get; set; }
        public User Owner { get; set; }*/

        public AccommodationViewModel()
        {

        }
        public AccommodationViewModel(int id,string name,Location location,TYPE type,string imagesPath, int minStay, int capacity, bool? isSuperOwner, double averageRating, int numberOfRatings)
        {
            ImagesPath = imagesPath;
            Id = id;
            Name = name;
            Location = location;
            Type = type;
            MinStay = minStay;
            Capacity = capacity;
            IsSuperOwner = isSuperOwner;
            AverageRating = averageRating;
            List<string> starPaths = new List<string>();
            while (averageRating >= 1)
            {
                starPaths.Add("../../../Resources/Images/OwnerImages/StarFull.png");
                averageRating--;
            }
            if (averageRating > 0.25)
                starPaths.Add("../../../Resources/Images/OwnerImages/StarHalfFull.png");
            while (starPaths.Count < 5)
                starPaths.Add("../../../Resources/Images/OwnerImages/idemo.png");
            Star1 = starPaths[0];
            Star2 = starPaths[1];
            Star3 = starPaths[2];
            Star4 = starPaths[3];
            Star5 = starPaths[4];
            NumberOfRatings = numberOfRatings;
        
        }
        public AccommodationViewModel(int id, string name, Location location, TYPE type, string imagesPath, int minStay, int capacity)
        {
            ImagesPath = imagesPath;
            Id = id;
            Name = name;
            Location = location;
            Type = type;
            MinStay = minStay;
            Capacity = capacity;

        }
        public AccommodationViewModel(int id, string name, Location location, TYPE type, string imagesPath, bool? isSuperOwner,double averageRating, int numberOfRatings)
        {
            ImagesPath = imagesPath;
            Id = id;
            Name = name;
            Location = location;
            Type = type;
            IsSuperOwner =isSuperOwner;
            List<string> starPaths = new List<string>();
            while (averageRating >= 1)
            {
                starPaths.Add("../../../Resources/Images/OwnerImages/StarFull.png");
                averageRating--;
            }
            if (averageRating > 0.25)
                starPaths.Add("../../../Resources/Images/OwnerImages/StarHalfFull.png");
            while (starPaths.Count < 5)
                starPaths.Add("../../../Resources/Images/OwnerImages/idemo.png");
            Star1 = starPaths[0];
            Star2 = starPaths[1];
            Star3 = starPaths[2];
            Star4 = starPaths[3];
            Star5 = starPaths[4];
            NumberOfRatings = numberOfRatings;
        }

    }
}
