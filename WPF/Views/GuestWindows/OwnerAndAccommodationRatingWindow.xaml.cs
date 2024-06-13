using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using BookingApp.WPF.ViewModels;
using ceTe.DynamicPDF.Imaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using System.Windows.Shapes;
using System.Xml.Linq;

namespace BookingApp.WPF.Views.GuestWindows
{
    /// <summary>
    /// Interaction logic for OwnerAndAccommodationRatingWindow.xaml
    /// </summary>
    public partial class OwnerAndAccommodationRatingWindow : Window, INotifyPropertyChanged
    {
        private int _cleanliness;
        public int Cleanliness
        {
            get { return _cleanliness; }
            set
            {
                if (_cleanliness != value)
                {
                    _cleanliness = value;
                    OnPropertyChanged("Cleanliness");
                }
            }
        }

        private int _correctness;
        public int Correctness
        {
            get { return _correctness; }
            set
            {
                if (_correctness != value)
                {
                    _correctness = value;
                    OnPropertyChanged("Correctness");
                }
            }
        }

        private string _comment;
        public string Comment
        {
            get { return _comment; }
            set
            {
                if (_comment != value)
                {
                    _comment = value;
                    OnPropertyChanged("Comment");
                }
            }
        }
        private int levelOfRenovation=0;
        public int LevelOfRenovation
        {
            get { return levelOfRenovation; }
            set
            {
                if (levelOfRenovation != value)
                {
                    levelOfRenovation = value;
                    OnPropertyChanged("LevelOfRenovation");
                }
            }
        }
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

        private int currentImageIndex = -1;

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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public User LoggedInUser;
        public AccommodationReservation AccommodationReservation;
        public int accommodationId;
        public DateTime dateTime;
        private AccommodationRatingService accommodationRatingService;
        private RateOwnerService rateOwnerService;
        private List<string> imagesPath;
        private ImageUploaderService imageUploaderService;
        private AccommodationService accommodationService;
        NotifierService notifierService;

        public string AccommodationName {  get; set; }

        public OwnerAndAccommodationRatingWindow(User user, AccommodationReservation accommodationReservation, int accommodationId)
        {
            rateOwnerService = new RateOwnerService();
            InitializeComponent();
            DataContext = this;
            imagesPath = new List<string>();
            LoggedInUser = user;
            accommodationRatingService = new AccommodationRatingService();
            imageUploaderService = new ImageUploaderService(accommodationRatingService);
            accommodationService = new AccommodationService();
            AccommodationReservation = accommodationReservation;
            this.accommodationId = accommodationId;
            dateTime = DateTime.Now;
            AccommodationName = accommodationService.GetAccommodationNameById(accommodationId);
            CanGoNext = false;
            CanGoBack = false;
            notifierService = new NotifierService();
        }

        private void RateOwnerAndAccommodation(object sender, RoutedEventArgs e)
        {
            string folderPath = imageUploaderService.CreateGuestFolder(imagesPath);
            rateOwnerService.RateOwner(new AccommodationRating(accommodationId, LoggedInUser.Id, AccommodationReservation.Id, Cleanliness, Correctness, Comment, DateOnly.FromDateTime(dateTime), folderPath, LevelOfRenovation));
            Close();
            notifierService.ShowSuccess("You have successfully rated the owner and accommodation!");
        }
        private void UploadPhotoClick(object sender, RoutedEventArgs e)
        {
            string imagePath = imageUploaderService.UploadImage();
            if (imagePath != null)
            {

                imagesPath.Add(imagePath);
                if (imagesPath.Count() > 1)
                {
                    CanGoBack = true;
                }
                ImagePath = imagePath;
                currentImageIndex = imagesPath.Count() - 1;
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton && radioButton.Tag != null && int.TryParse(radioButton.Tag.ToString(), out int level))
            {
                LevelOfRenovation = level;
            }
        }

        private void NextPicture(object sender, MouseButtonEventArgs e)
        {
            if (imagesPath.Count()-1 > currentImageIndex)
            {
                CanGoBack = true;
                currentImageIndex++;
                ImagePath = imagesPath[currentImageIndex];

            }
            if (imagesPath.Count() - 1 == currentImageIndex)
            {
                CanGoNext = false;
            }
        }

        private void BackPicture(object sender, MouseButtonEventArgs e)
        {
            if (currentImageIndex != 0)
            {
                CanGoBack = true;
                CanGoNext = true;
                currentImageIndex--;
                ImagePath = imagesPath[currentImageIndex];
            }
            if (currentImageIndex == 0)
            {
                CanGoBack = false;
            }
        }

        public void DeletePhotoClick(object sender, RoutedEventArgs e)
        {
            if(imagesPath.Count()!=0)
            if(currentImageIndex == 0)
            {
                
                if(currentImageIndex == imagesPath.Count() - 1)
                {
                    imagesPath.RemoveAt(currentImageIndex);
                    ImagePath = "";
                }
                else
                {
                    imagesPath.RemoveAt(currentImageIndex);
                    ImagePath = imagesPath[currentImageIndex];

                }
            }
            else
            {
                imagesPath.RemoveAt(currentImageIndex);
                currentImageIndex--;
                ImagePath = imagesPath[currentImageIndex];
            }
            
        }
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string newText = textBox.Text.Insert(textBox.SelectionStart, e.Text);

            if (!int.TryParse(newText, out int newValue))
            {
                textBox.BorderBrush = Brushes.Red;
                e.Handled = true;
                return;
            }

            if (newValue < 1 || newValue > 5)
            {
                textBox.BorderBrush = Brushes.Red;
                e.Handled = true;
                return;
            }

            textBox.ClearValue(TextBox.BorderBrushProperty);
        }

    }
}
