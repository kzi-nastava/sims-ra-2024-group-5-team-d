using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using BookingApp.WPF.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels
{
    public class OwnerRatingViewModel
    {
        public ICommand ForwardCommand { get; private set; }
        public ICommand BackwardCommand { get; private set; }
        public int Id { get; set; }
        public string GuestName { get; set; }
        public Location Location { get; set; }
        public int ReservationId { get; set; }
        public ObservableCollection<string> ImagesPaths { get; set; }
        public string AccommodationName { get; set; }
        public int CleanlinessRating { get; set; }
        public int CorrectnessRating { get; set; }
        public DateOnly RatingDate { get; set; }
        public string Comment { get; set; }
        private IAccommodationRepository accommodationRepository;
        private IUserRepository userRepository;
        private ImageUploaderService imageUploaderService;
        private List<string> imagesPaths;
        private int PaginationIndex = -3;
        public OwnerRatingViewModel()
        {
        }
        public void Backward()
        {
            PaginationIndex=PaginationIndex-3;
            ImagesPaths.Clear();
            for (int i = PaginationIndex; i < imagesPaths.Count; i++)
            {
                ImagesPaths.Add(imagesPaths[i]);
                if (ImagesPaths.Count > 2)
                    break;
            }

        
        }
        public void Forward()
        {
            PaginationIndex=PaginationIndex+3;
           ImagesPaths.Clear();
            for (int i = PaginationIndex; i < imagesPaths.Count; i++)
            {
                ImagesPaths.Add(imagesPaths[i]);
                if (ImagesPaths.Count > 2)
                    break;
            }
        }
        public OwnerRatingViewModel(int id,string guestName, Location location, string accommodationName, DateOnly ratingDate, string comment)
        {
            Id = id;
            GuestName = guestName;
            Location = location;
            AccommodationName = accommodationName;
            RatingDate = ratingDate;
            Comment = comment;
        }
        public OwnerRatingViewModel(AccommodationRating rating)
        {
            BackwardCommand = new RelayCommand(Backward);
            ForwardCommand = new RelayCommand(Forward);
            imageUploaderService = new ImageUploaderService();
            ReservationId = rating.ReservationId;
            CleanlinessRating = rating.Cleanliness;
            CorrectnessRating = rating.Correctness;
            accommodationRepository=Injector.CreateInstance<IAccommodationRepository>();
            userRepository=Injector.CreateInstance<IUserRepository>();
            Accommodation accommodation = accommodationRepository.GetById(rating.AccommodationId);
            Id = rating.Id;
            GuestName = userRepository.GetFullNameById(rating.GuestId);
            Location = accommodation.Location;
            AccommodationName = accommodation.Name;
            RatingDate = rating.TimeOfRating;
            Comment = rating.Comment;
            imagesPaths = imageUploaderService.GetImagePaths(rating.ImagesPath);
            ImagesPaths = new ObservableCollection<string>();
            Forward();
            //imagesPaths.ForEach(imagePath => ImagesPaths.Add(imagePath));
            //foreach (string path in imagePaths)
            //ImagesPath =accommodation.ImagesPath;
            //Debug.WriteLine("ImagesPath: "+ImagesPath);
            //ImagesPath = rating.ImagesPath;
        }
    }
}
