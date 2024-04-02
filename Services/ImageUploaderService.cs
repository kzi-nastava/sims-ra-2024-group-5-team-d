using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class ImageUploaderService
    {
        private List<string> imagesPath;
        private string accommodationFolderPath = "../../../AccommodationImages/Accommodation";
        
        public ImageUploaderService()
        {
            imagesPath = new List<string>();
        }

        public void UploadImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                imagesPath.Add(filePath);
            }
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

        public void SaveImages()
        {
            foreach (string imagePath in imagesPath)
            {
                string targetImagePath = Path.Combine(accommodationFolderPath, Path.GetFileName(imagePath));
                File.Copy(imagePath, targetImagePath);
            }
        }
}
}
