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

        public ImageUploaderService()
        {
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

        public string CreateAccommodationFolder(int folderId)
        {
            accommodationFolderPath = accommodationFolderPath + folderId;
            if (!Directory.Exists(accommodationFolderPath))
            {
                Directory.CreateDirectory(accommodationFolderPath);
            }
            return accommodationFolderPath;
        }
        public string CreateTourFolder(int folderId)
        {
            tourFolderPath = tourFolderPath + folderId;
            if (!Directory.Exists(accommodationFolderPath))
            {
                Directory.CreateDirectory(tourFolderPath);
            }
            return tourFolderPath;
        }

        public void SaveImages(List<string>imagesPath,string folderPath)
        {
            foreach (string imagePath in imagesPath)
            {
                string targetImagePath = Path.Combine(folderPath, Path.GetFileName(imagePath));
                File.Copy(imagePath, targetImagePath);
            }
        }
    }
}
