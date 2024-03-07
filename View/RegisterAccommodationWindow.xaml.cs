using BookingApp.Model;
using BookingApp.Repository;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for RegisterAccommodationWindow.xaml
    /// </summary>
    public partial class RegisterAccommodationWindow : Window
    {
        private string accommodationName;
        public  string AccommodationName
        {
            get => accommodationName;
            set
            {
                if (value != accommodationName)
                {
                    accommodationName = value;
                    OnPropertyChanged();
                }
            }
        }
        private int locationId=0;
        public int LocationId
        {
            get => locationId;
            set
            {
                if (value != locationId)
                {
                    locationId = value;
                    OnPropertyChanged();
                }
            }
        }
        private int accommodationType = 0;
        public int AccommodationType
        {
            get => accommodationType;
            set
            {
                if (value != accommodationType)
                {
                    accommodationType = value;
                    OnPropertyChanged();
                }
            }
        }
        private int capacity;
        public int Capacity
        {
            get => capacity;
            set
            {
                if (value != capacity)
                {
                    capacity = value;
                    OnPropertyChanged();
                }
            }
        }
        private int minStay;
        public int MinStay
        {
            get => minStay;
            set
            {
                if (value != minStay)
                {
                    minStay = value;
                    OnPropertyChanged();
                }
            }
        }
        private int cancellationDeadline=1;
        public int CancellationDeadline
        {
            get => cancellationDeadline;
            set
            {
                if (value != cancellationDeadline)
                {
                    cancellationDeadline = value;
                    OnPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private readonly AccommodationRepository _repository;
        public User LoggedInUser { get; set; }
        private List<string>imagesPath;
        public RegisterAccommodationWindow(User user)
        {
           
            InitializeComponent();
            _repository = new AccommodationRepository();
            DataContext = this;
            LoggedInUser = user;
            imagesPath = new List<string>();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string folderPath = "../../../AccommodationImages/Accommodation";
            folderPath = folderPath +_repository.NextId();
            Directory.CreateDirectory(folderPath);
            Debug.WriteLine("Folder je kreiran.");
            foreach (string imagePath in imagesPath)
            {
                string targetImagePath = System.IO.Path.Combine(folderPath, System.IO.Path.GetFileName(imagePath));
                File.Copy(imagePath, targetImagePath);
            }
            Accommodation newAccommodation = new Accommodation(accommodationName, _repository.getLocationByLocationId(locationId), (TYPE)accommodationType, minStay,cancellationDeadline,capacity,folderPath,LoggedInUser);
            Accommodation savedAccommodation = _repository.Save(newAccommodation);

            OwnerWindow.Accommodations.Add(savedAccommodation);
            Close();
        }

        private void UploadPictureButtno_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                 string filePath = openFileDialog.FileName;
                 imagesPath.Add(filePath);
                 Debug.WriteLine(filePath);
            }
            
        }
    }
}
