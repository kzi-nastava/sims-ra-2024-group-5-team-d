using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    public class AccommodationsViewModel
    {
        public static ObservableCollection<AccommodationViewModel> Accommodations { get; set; }
        private readonly AccommodationService accommodationService;
        private AccommodationRatingService accommodationRatingService;
        public AccommodationsViewModel(User user)
        {
            accommodationService = new AccommodationService();
            accommodationRatingService = new AccommodationRatingService();
            Accommodations = new ObservableCollection<AccommodationViewModel>();
            List<Accommodation> ownerAccommodations = accommodationService.GetByUser(user);
            ownerAccommodations.Sort((x, y) => y.IsSuperOwner.CompareTo(x.IsSuperOwner));
            ownerAccommodations.ForEach(accommodation => Accommodations.Add(new AccommodationViewModel(accommodation.Id, accommodation.Name, accommodation.Location, accommodation.Type, accommodation.ImagesPath, accommodation.IsSuperOwner, accommodation.AverageRating, accommodationRatingService.GetNumberOfRatingsForAccommodation(accommodation))));

        }
    }
}
