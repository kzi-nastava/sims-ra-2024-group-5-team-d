using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ToastNotifications.Position;

namespace BookingApp.Appl.UseCases
{
    public class AccommodationRatingService
    {
        private IAccommodationRatingRepository accommodationRatingRepository;
        private IAccommodationRepository accommodationRepository;
        private GuestRatingService guestRatingService;
        public AccommodationRatingService()
        {
            guestRatingService = new GuestRatingService();
            accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
            accommodationRatingRepository = Injector.CreateInstance<IAccommodationRatingRepository>();
        }
        public List<AccommodationRating> GetAllRatingsForOwner(User owner) {
            List<AccommodationRating> allOwnerRatings = FindAllRatingsForOwner(owner);
            List<AccommodationRating> filteredOwnerRatings = filterOwnerRatings(allOwnerRatings,owner);
            return filteredOwnerRatings;
        }
        public int GetNumberOfRatingsForOwner(User owner)
        {
            return FindAllRatingsForOwner(owner).Count;
        }
        private List<AccommodationRating> filterOwnerRatings(List<AccommodationRating>  allOwnerRatings,User owner)
        {
            return allOwnerRatings.Where(rating=> IsReservationMutualyRated(owner,rating)).ToList();
        }
        private bool IsReservationMutualyRated(User owner,AccommodationRating accommodationRating)
        {
            Debug.WriteLine("Checking if reservation is mutualy rated"+ accommodationRating.ReservationId);
            return guestRatingService.GetAllGuestRatingsByOwner(owner).Any(guestrating => guestrating.ReservationId == accommodationRating.ReservationId);
        }
        private List<AccommodationRating> FindAllRatingsForOwner(User owner)
        {
            return accommodationRatingRepository.GetAll().Where(aR => accommodationRepository.GetByUser(owner).Any(accommodation => accommodation.Id == aR.AccommodationId)).ToList();
        }
    }
}
