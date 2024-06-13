using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BookingApp.WPF.ViewModels
{
    public class TourViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public User User { get; set; }
        public string Description { get; set; }
        public int NumberOfVotes { get; set; }
        public string RatingImagePath { get; set; }
        public Location Location { get; set; }

        public string Star1Path { get; set; }
        public string Star2Path { get; set; }
        public string Star3Path { get; set; }
        public string Star4Path { get; set; }
        public string Star5Path { get; set; }
        public double Duration { get; set; }

        public double Rating { get; set; }
        public string ImagesPath { get; set; }
        public int Capacity { get; set; }
        public LANGUAGE Language { get; set; }
        private TourService tourService = new TourService();
        private UserService userService = new UserService();
        public bool? IsSuperGuide { get; set; }
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
            Rating = tourService.GetRating(id);
            NumberOfVotes = tourService.GetNumberOfVotes(id);
            SetStarPaths();
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
            Debug.WriteLine("User sa id: " + tourService.GetById(id).User.Id + " je SuperUser: " + userService.GetById(tourService.GetById(id).User.Id).IsSuperUser);
            IsSuperGuide = userService.GetById(tourService.GetById(id).User.Id).IsSuperUser;
            Language = language;
            User = user;
            Rating = tourService.GetRating(id);
            NumberOfVotes = tourService.GetNumberOfVotes(id);
            SetStarPaths();
        }

        public void SetStarPaths()
        {
            int fullStars = (int)Rating; // Number of full stars
            int halfStars = (int)((Rating - fullStars) * 2); // Number of half stars

            for (int i = 1; i <= 5; i++)
            {
                if (i <= fullStars)
                {
                    SetStarPath(i, "full_star.png");
                }
                else if (i == fullStars + 1 && halfStars > 0)
                {
                    SetStarPath(i, "half_star.png");
                    halfStars--;
                }
                else
                {
                    SetStarPath(i, "empty_star.png");
                }
            }
        }

        private void SetStarPath(int starNumber, string fileName)
        {
            string basePath = @"C:\Users\Korisnik\Desktop\ikone\";
            string fullPath = Path.Combine(basePath, fileName);

            switch (starNumber)
            {
                case 1:
                    Star1Path = fullPath;
                    break;
                case 2:
                    Star2Path = fullPath;
                    break;
                case 3:
                    Star3Path = fullPath;
                    break;
                case 4:
                    Star4Path = fullPath;
                    break;
                case 5:
                    Star5Path = fullPath;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(starNumber), "Invalid star number.");
            }
        }
    }
}
