using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.Commands;
using BookingApp.WPF.Views;
using BookingApp.WPF.Views.OwnerView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    public class RegisterAccommodationViewModel
    {

        NotifierService notifier;
        public ICommand SetMainPictureCommand { get; private set; }
        public ICommand ForwardCommand { get; private set; }
        public ICommand BackwardCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand UploadCommand { get; private set; }

        private ImageUploaderService imageUploaderService;
        private LocationService locationService;
        private AccommodationService accommodationService;

        public string Name { get; set; }
        public int LocationId { get; set; }
        public int Type { get; set; }
        public int CancellationDeadline { get; set; }
        public int MaxCapacity { get; set; }
        public int MinDaysToStay { get; set; }
        public ObservableCollection<string> ImagesPaths { get; set; }
        public User Owner { get; set; }
        private List<string> imagesPath;
       
        private User loggedInUser;
        private int PaginationIndex = 0;
        private string mainImagePath;
        public RegisterAccommodationViewModel(User user)
        {
            InitializeServices();
            BackwardCommand = new RelayCommand(Backward);
            ForwardCommand = new RelayCommand(Forward);
            SetMainPictureCommand = new RelayParameterCommand(SetMainPicture);
            ImagesPaths = new ObservableCollection<string>();
            loggedInUser = user;
            imagesPath = new List<string>();
            Owner = user;
            SaveCommand = new RelayCommand(Save);
            UploadCommand = new RelayCommand(UploadPicture);

        }

        private void InitializeServices()
        {
            notifier = new NotifierService();
            locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            accommodationService = new AccommodationService(Injector.CreateInstance<IAccommodationRepository>());
            imageUploaderService = new ImageUploaderService(accommodationService);
        }

        private void SetMainPicture(object obj)
        {
            string ImagePath = obj as string;
            if (ImagePath != null)
            {
                mainImagePath = @"\"+Path.GetFileName(ImagePath);
                Debug.WriteLine(mainImagePath);
            }
        }
        public void Backward()
        {
            if (PaginationIndex > 0)
            {
                PaginationIndex--;
                ShowImage();
            }
        }
        public void Forward()
        {
            if (PaginationIndex < imagesPath.Count-1)
            {
                PaginationIndex++;
                ShowImage();
            }
        }
        private void ShowImage()
        {
            ImagesPaths.Clear();
            ImagesPaths.Add(imagesPath[PaginationIndex]);
        }

        private void Save()
        {
            string folderPath = imageUploaderService.CreateAccommodationFolder(imagesPath);
            folderPath = folderPath + mainImagePath;
            Accommodation newAccommodation = new Accommodation(Name, locationService.GetById(LocationId), (TYPE)Type, MinDaysToStay, CancellationDeadline, MaxCapacity, folderPath, Owner);
            Accommodation savedAccommodation = accommodationService.Save(newAccommodation);
            notifier.ShowSuccess("Accommodation added SUCCESSFULLY!");
        }
        private void UploadPicture()
        {
            string imagePath = imageUploaderService.UploadImage();
            if (imagePath != null)
            {
                imagesPath.Add(imagePath);
                PaginationIndex = imagesPath.Count - 1;
                ShowImage();
            }
        }
    }
}
