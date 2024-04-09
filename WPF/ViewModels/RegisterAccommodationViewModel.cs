using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.Commands;
using BookingApp.WPF.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels
{
    public class RegisterAccommodationViewModel
    {
        public ICommand SaveCommand { get; private set; }
        public ICommand UploadCommand { get; private set; }
        public string Name { get; set; }
        public int LocationId { get; set; }
        public int Type { get; set; }
        public int CancellationDeadline { get; set; }
        public int MaxCapacity { get; set; }
        public int MinDaysToStay { get; set; }
        public string ImagesPath { get; set; }
        public User Owner { get; set; }
        private ImageUploaderService imageUploaderService = new ImageUploaderService();
        private readonly IAccommodationRepository accommodationRepository;
        private readonly ILocationRepository locationRepository;
        private List<string> imagesPath;
        public RegisterAccommodationViewModel(User user)
        {
            imagesPath = new List<string>();
            locationRepository = Injector.CreateInstance<ILocationRepository>();
            Owner = user;
            accommodationRepository=Injector.CreateInstance<IAccommodationRepository>();
            imageUploaderService = new ImageUploaderService();
            SaveCommand = new RelayCommand(Save);
            UploadCommand = new RelayCommand(UploadPicture);

        }
        private void Save()
        {
            string folderPath = imageUploaderService.CreateAccommodationFolder(imagesPath);

            Accommodation newAccommodation = new Accommodation(Name, locationRepository.GetById(LocationId), (TYPE)Type, MinDaysToStay, CancellationDeadline, MaxCapacity, folderPath, Owner);
            Accommodation savedAccommodation = accommodationRepository.Save(newAccommodation);
        }
        private void UploadPicture()
        {
            string imagePath = imageUploaderService.UploadImage();
            if (imagePath != null)
                imagesPath.Add(imagePath);
        }
    }
}
