using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BookingApp.Appl.UseCases
{
    class StarsCalculationService
    {
        public string Star1Path { get; set; }
        public string Star2Path { get; set; }
        public string Star3Path { get; set; }
        public string Star4Path { get; set; }
        public string Star5Path { get; set; }
        public double Duration { get; set; }

        public double Rating { get; set; }
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
