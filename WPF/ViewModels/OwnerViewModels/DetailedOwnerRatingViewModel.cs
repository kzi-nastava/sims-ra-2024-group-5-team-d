using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
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

        private AccommodationService accommodationService;
        private UserService userService;
        private ImageUploaderService imageUploaderService;
        private AccommodationRatingService accommodationRatingService;

        public ObservableCollection<string> ImagesPaths { get; set; }
        private AccommodationRating accommodationRating;
        private List<string> imagesPaths;
        private int PaginationIndex = -3;
        public OwnerRatingViewModel OwnerRatingsViewModel { get; set; }
        public DetailedOwnerRatingViewModel(int accommodationRatingId)
        {
            InitializeServices();
            accommodationRating = accommodationRatingService.GetById(accommodationRatingId);
            BackwardCommand = new RelayCommand(Backward);
            ForwardCommand = new RelayCommand(Forward);
            imagesPaths = imageUploaderService.GetImagePaths(accommodationRating.ImagesPath);
            ImagesPaths = new ObservableCollection<string>();
            OwnerRatingsViewModel = new OwnerRatingViewModel(userService.GetById(accommodationRating.GuestId),accommodationRating,accommodationService.GetById(accommodationRating.AccommodationId),accommodationRatingService.GenerateRenovationText(accommodationRating));
            Forward();
        }

        private void InitializeServices()
        {
            userService = new UserService(Injector.CreateInstance<IUserRepository>());
            accommodationService = new AccommodationService(Injector.CreateInstance<IAccommodationRepository>(),userService);
            accommodationRatingService = new AccommodationRatingService(Injector.CreateInstance<IAccommodationRatingRepository>());
            imageUploaderService = new ImageUploaderService();
        }

        public void Backward()
        {
            if (PaginationIndex > 2)
            {
                PaginationIndex = PaginationIndex - 3;
                ImagesPaths.Clear();
                ShowImages();

            }
        }
        private void ShowImages() {
            for (int i = PaginationIndex; i < imagesPaths.Count; i++)
            {
                ImagesPaths.Add(imagesPaths[i]);
                if (ImagesPaths.Count > 2)
                    break;
            }
        }
        public void Forward()
        {
            if(PaginationIndex + 3 < imagesPaths.Count)
            {
                PaginationIndex = PaginationIndex + 3;
                ImagesPaths.Clear();
                ShowImages();
            }
        }
    }
}
