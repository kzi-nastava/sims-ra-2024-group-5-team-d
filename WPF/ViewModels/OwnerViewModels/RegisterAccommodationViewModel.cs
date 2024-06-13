using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Validation;
using BookingApp.WPF.Commands;
using BookingApp.WPF.Views;
using BookingApp.WPF.Views.OwnerView;
using HarfBuzzSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;
using Xceed.Wpf.Toolkit.Primitives;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    public class RegisterAccommodationViewModel:ValidationBase,INotifyPropertyChanged
    {
        public ICommand SelectionChangedCommand { get; private set; }
        public ICommand SetMainPictureCommand { get; private set; }
        public ICommand RemoveCommand { get; private set; }
        public ICommand ForwardCommand { get; private set; }
        public ICommand BackwardCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand UploadCommand { get; private set; }

        NotifierService notifier;
        private ImageUploaderService imageUploaderService;
        private LocationService locationService;
        private AccommodationService accommodationService;

        private RegistrationViewModel registrationViewModel;
        public RegistrationViewModel RegistrationViewModel
        {
            get { return registrationViewModel; }
            set
            {
                registrationViewModel = value;
                OnPropertyChanged();
            }
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler? PropertyChanged;


        private ImageUploadViewModel SelectedImage;
        public ObservableCollection<ImageUploadViewModel> ImagesPaths { get; set; }

        private User loggedInUser;
        private List<string> imagesPath;
        private int PaginationIndex = 0;
        private int pageNumber = 0;
        private string mainImagePath="";
        public RegisterAccommodationViewModel(User user)
        {
            InitializeServices();
            BackwardCommand = new RelayCommand(Backward);
            SelectionChangedCommand = new RelayParameterCommand(SelectionChanged);
            ForwardCommand = new RelayCommand(Forward);
            SetMainPictureCommand = new RelayCommand(SetMainPicture);
            RemoveCommand = new RelayCommand(Remove);
            RegistrationViewModel = new RegistrationViewModel();
            ImagesPaths = new ObservableCollection<ImageUploadViewModel>();
            loggedInUser = user;
            imagesPath = new List<string>();
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
        public void SelectionChanged(object parameter)
        {
            if (parameter != null)
            {
                SelectedImage = parameter as ImageUploadViewModel;
                RegistrationViewModel.SetAsDefaultEnabled = true;
                for(int i=0;i<ImagesPaths.Count;i++)
                {
                    if (SelectedImage.ImagePath == ImagesPaths[i].ImagePath)
                    ImagesPaths[i].IsSelected = true;
                    else
                        ImagesPaths[i].IsSelected = false;
                }
                Debug.WriteLine(SelectedImage.ImagePath);
            }
        }
        private void Remove()
        {
            if (SelectedImage != null)
            {
                imagesPath.Remove(SelectedImage.ImagePath);
                if (imagesPath.Count==0)
                    RegistrationViewModel.IsUploading = false;
                if (SelectedImage.ImagePath.Contains(mainImagePath))
                    mainImagePath= "";
                ImagesPaths.Clear();
                ShowImages();
            }
        }
        private void SetMainPicture()
        {
            if (SelectedImage != null)
            {
                mainImagePath = @"\" + Path.GetFileName(SelectedImage.ImagePath);
            }
        }
        public void Backward()
        {
            if (pageNumber!=0)
            {
                pageNumber--;
                ImagesPaths.Clear();
                ShowImages();

            }
        }
        public void Forward()
        {
            if (pageNumber*4+4<imagesPath.Count)
            {
                pageNumber++;
                ImagesPaths.Clear();
                ShowImages();
            }
        }
        private void ShowImages()
        {
            RegistrationViewModel.SetAsDefaultEnabled = false;
            SelectedImage = null;
            for (int i = pageNumber*4; i < imagesPath.Count; i++)
            {
                ImagesPaths.Add(new ImageUploadViewModel(imagesPath[i]));
                if (ImagesPaths.Count > 3)
                    break;
            }
        }

        private void Save()
        {
            this.Validate();
            RegistrationViewModel.Validate();
            if (!RegistrationViewModel.IsValid && !this.IsValid)
            {
                return;
            }
            string folderPath = imageUploaderService.CreateAccommodationFolder(imagesPath);
            folderPath = folderPath + mainImagePath;
            Accommodation newAccommodation = new Accommodation(RegistrationViewModel.Name, locationService.GetById(RegistrationViewModel.LocationId), (TYPE)RegistrationViewModel.Type, RegistrationViewModel.MinDaysToStay, RegistrationViewModel.CancellationDeadline, RegistrationViewModel.MaxCapacity, folderPath, loggedInUser);
            Accommodation savedAccommodation = accommodationService.Save(newAccommodation);
            notifier.ShowSuccess("Accommodation added SUCCESSFULLY!");
            RegistrationViewModel.Reset();
            ImagesPaths.Clear();
            SelectedImage = null;
            imagesPath.Clear();
        }
        private void UploadPicture()
        {
            string imagePath = imageUploaderService.UploadImage();
            if (imagePath != null)
            {
                for(int i=0;i< imagesPath.Count;i++)
                {
                    if (imagesPath[i] == imagePath)
                    {
                        notifier.ShowError("Image already uploaded!");
                        return;
                    }
                }
                RegistrationViewModel.IsUploading = true;
                imagesPath.Add(imagePath);
                PaginationIndex = imagesPath.Count - 1;
                if (ImagesPaths.Count < 4)
                {
                    ImagesPaths.Add(new ImageUploadViewModel(imagesPath[PaginationIndex]));
                }
                else
                    Forward();    
            }
        }

        protected override void ValidateSelf()
        {
            if(ImagesPaths.Count<=0)
            {
                this.ValidationErrors["Images"] = "You must upload at least one image.";
            }
        }
    }
}
