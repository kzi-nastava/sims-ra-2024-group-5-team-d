using BookingApp.Domain.RepositoryInterfaces;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Appl.UseCases
{
    public class ImageUploaderService
    {
        
        private string accommodationFolderPath = "../../../Resources/AccommodationImages/Accommodation";
        private string tourFolderPath = "../../../Resources/TourImages/Tour";
        private string guestFolderPath = "../../../Resources/GuestRatingImages/GuestRating";
        private string touristFolderPath = "../../../Resources/TouristRatingImages/TourRating";
        private AccommodationService accommodationService;
        private TourService tourService;
        private AccommodationRatingService accommodationRatingService;
        private TourRatingService tourRatingService;

        public ImageUploaderService()
        {
        }
        public ImageUploaderService(AccommodationService accommodationService)
        {
            this.accommodationService = accommodationService;
        }
        public ImageUploaderService(TourService tourService)
        {
            this.tourService = tourService;
        }
        public ImageUploaderService(AccommodationRatingService accommodationRatingService)
        {
            this.accommodationRatingService = accommodationRatingService;
        }
        public ImageUploaderService(TourRatingService tourRatingService)
        {
            this.tourRatingService = tourRatingService;
        }

        public string  UploadImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                return filePath;
            }
            return null;
        }

        public string CreateAccommodationFolder(List<string> imagesPath)
        {
            int folderId = accommodationService.NextId();
            accommodationFolderPath = accommodationFolderPath + folderId;
            if (!Directory.Exists(accommodationFolderPath))
            {
                Directory.CreateDirectory(accommodationFolderPath);
            }
            SaveImages(imagesPath, accommodationFolderPath);
            return accommodationFolderPath;
        }
        public string CreateTourFolder(List<string> imagesPath)
        {
            int folderId = tourService.NextIdForTour();
            tourFolderPath = tourFolderPath + folderId;
            if (!Directory.Exists(tourFolderPath))
            {
                Directory.CreateDirectory(tourFolderPath);
            }
            SaveImages(imagesPath, tourFolderPath);
            return tourFolderPath;
        }
        public string CreateGuestFolder(List<string> imagesPath)
        {
            int folderId = accommodationRatingService.NextId();
            guestFolderPath = guestFolderPath + folderId;
            if (!Directory.Exists(guestFolderPath))
            {
                Directory.CreateDirectory(guestFolderPath);
            }
            SaveImages(imagesPath, guestFolderPath);
            return guestFolderPath;
        }
        public string CreateTourRateFolder(List<string> imagesPath)
        {
            int folderId = tourRatingService.NextIdForTourRating();
            touristFolderPath = touristFolderPath + folderId;
            if (!Directory.Exists(touristFolderPath))
            {
                Directory.CreateDirectory(touristFolderPath);
            }
            SaveImages(imagesPath, touristFolderPath);
            return touristFolderPath;
        }


        private void SaveImages(List<string>imagesPath,string folderPath)
        {
            foreach (string imagePath in imagesPath)
            {
                string targetImagePath = Path.Combine(folderPath, Path.GetFileName(imagePath));
                File.Copy(imagePath, targetImagePath);
            }
        }
        public List<string> GetImagePaths(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                throw new DirectoryNotFoundException($"Folder '{folderPath}' ne postoji.");
            }

            string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            var imagePaths = Directory.GetFiles(folderPath)
                                      .Where(file => imageExtensions.Contains(Path.GetExtension(file).ToLower()))
                                      .ToList();

            return imagePaths;
        }
    }
}
