using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.WPF.Views.GuestWindows
{
    /// <summary>
    /// Interaction logic for AccommodationGalleryUserControl.xaml
    /// </summary>
    public partial class AccommodationGalleryUserControl : UserControl, INotifyPropertyChanged
    {
        public User LoggedInUser {  get; set; } 
        public AccommodationViewModel Accommodation { get; set; }

        private bool canGoBack;
        public bool CanGoBack
        {
            get => canGoBack;
            set
            {
                if (value != canGoBack)
                {
                    canGoBack = value;
                    OnPropertyChanged(nameof(canGoBack));
                }
            }
        }
        private bool canGoNext;
        public bool CanGoNext
        {
            get => canGoNext;
            set
            {
                if (value != canGoNext)
                {
                    canGoNext = value;
                    OnPropertyChanged(nameof(canGoNext));
                }
            }
        }
        public List<string> ImagesPath { get; set; }

        private int currentImageIndex = 0;

        private string imagePath;
        public string ImagePath
        {
            get => imagePath;
            set
            {
                if (value != imagePath)
                {
                    imagePath = value;
                    OnPropertyChanged(nameof(imagePath));
                }
            }
        }
        public string AccommodationName { get; set; }
        private AccommodationService accommodationService;
        public event PropertyChangedEventHandler? PropertyChanged;
        private ImageUploaderService imageUploaderService;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public AccommodationGalleryUserControl(User user, AccommodationViewModel accommodation)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;
            Accommodation = accommodation;
            ImagesPath = new List<string>();
            accommodationService = new AccommodationService();
            imageUploaderService = new ImageUploaderService();

            AccommodationName = accommodationService.GetAccommodationNameById(accommodation.Id);

            if (!Directory.Exists(Accommodation.ImagesPath))
            {
                Accommodation.ImagesPath = System.IO.Path.GetDirectoryName(Accommodation.ImagesPath);
            }
                ImagesPath = imageUploaderService.GetImagePaths(Accommodation.ImagesPath);
            

            ImagePath = ImagesPath[0];
            CanGoNext = true;
            CanGoBack = false;
        }

        private void BackToAccommodation_Click(object sender, MouseButtonEventArgs e)
        {
            GuestWindow.contentControl.Content = new AccommodationUserControl(LoggedInUser, Accommodation);
        }

        private void BackPicture_Click(object sender, MouseButtonEventArgs e)
        {
            if (currentImageIndex != 0)
            {
                CanGoBack = true;
                CanGoNext = true;
                currentImageIndex--;
                ImagePath = ImagesPath[currentImageIndex];
            }
            if (currentImageIndex == 0)
            {
                CanGoBack = false;
            }

        }

        private void NextPicture_Click(object sender, MouseButtonEventArgs e)
        {
            if (ImagesPath.Count()-1 > currentImageIndex)
            {
                CanGoBack = true;
                currentImageIndex++;
                ImagePath = ImagesPath[currentImageIndex];

            }
            if (ImagesPath.Count() - 1 == currentImageIndex)
            {
                CanGoNext = false;
            }
        }
    }
}
