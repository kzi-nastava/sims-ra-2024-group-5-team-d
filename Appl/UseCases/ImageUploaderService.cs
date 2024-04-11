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
        private IAccommodationRepository accommodationRepository;
        private ITourRepository tourRepository;
        private IAccommodationRatingRepository accommodationRatingRepository;

        public ImageUploaderService()
        {
            accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
            tourRepository = Injector.CreateInstance<ITourRepository>();
            accommodationRatingRepository = Injector.CreateInstance<IAccommodationRatingRepository>();
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
            int folderId = accommodationRepository.NextId();
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
            int folderId = tourRepository.NextIdForTour();
            tourFolderPath = tourFolderPath + folderId;
            if (!Directory.Exists(tourFolderPath))
            {
                Directory.CreateDirectory(tourFolderPath);
            }
            SaveImages(imagesPath, tourFolderPath);
            return accommodationFolderPath;
        }
        public string CreateGuestFolder(List<string> imagesPath)
        {
            int folderId = accommodationRatingRepository.NextId();
            guestFolderPath = guestFolderPath + folderId;
            if (!Directory.Exists(guestFolderPath))
            {
                Directory.CreateDirectory(guestFolderPath);
            }
            SaveImages(imagesPath, guestFolderPath);
            return guestFolderPath;
        }

        private void SaveImages(List<string>imagesPath,string folderPath)
        {
            foreach (string imagePath in imagesPath)
            {
                string targetImagePath = Path.Combine(folderPath, Path.GetFileName(imagePath));
                File.Copy(imagePath, targetImagePath);
            }
        }
    }
}
