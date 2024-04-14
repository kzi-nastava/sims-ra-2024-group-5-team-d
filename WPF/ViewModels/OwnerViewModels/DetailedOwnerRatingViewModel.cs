using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.WPF.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    public class DetailedOwnerRatingViewModel
    {
        public ICommand ForwardCommand { get; private set; }
        public ICommand BackwardCommand { get; private set; }
        public ObservableCollection<string> ImagesPaths { get; set; }
        private AccommodationService accommodationService;
        private UserService userService;
        private ImageUploaderService imageUploaderService;
        private AccommodationRatingService accommodationRatingService;
        private AccommodationRating accommodationRating;
        private List<string> imagesPaths;
        private int PaginationIndex = -3;
        public OwnerRatingViewModel OwnerRatingsViewModel { get; set; }
        public DetailedOwnerRatingViewModel(int accommodationRatingId)
        {
            accommodationRatingService = new AccommodationRatingService();
            accommodationRating = accommodationRatingService.GetById(accommodationRatingId);
            BackwardCommand = new RelayCommand(Backward);
            ForwardCommand = new RelayCommand(Forward);
            imageUploaderService = new ImageUploaderService();
            accommodationService = new AccommodationService();
            userService = new UserService();
            imagesPaths = imageUploaderService.GetImagePaths(accommodationRating.ImagesPath);
            ImagesPaths = new ObservableCollection<string>();
            OwnerRatingsViewModel = new OwnerRatingViewModel(userService.GetById(accommodationRating.GuestId),accommodationRating,accommodationService.GetById(accommodationRating.AccommodationId));
            Forward();
        }
        public void Backward()
        {
            PaginationIndex = PaginationIndex - 3;
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
            PaginationIndex = PaginationIndex + 3;
            ImagesPaths.Clear();
            for (int i = PaginationIndex; i < imagesPaths.Count; i++)
            {
                ImagesPaths.Add(imagesPaths[i]);
                if (ImagesPaths.Count > 2)
                    break;
            }
        }
    }
}
