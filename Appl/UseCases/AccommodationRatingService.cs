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
        private AccommodationService accommodationService;
        public AccommodationRatingService()
        {
            accommodationService = new AccommodationService();
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
            return guestRatingService.GetAllGuestRatingsByOwner(owner).Any(guestrating => guestrating.ReservationId == accommodationRating.ReservationId);
        }
        private List<AccommodationRating> FindAllRatingsForOwner(User owner)
        {
            return accommodationRatingRepository.GetAll().Where(aR => accommodationService.IsUserOwnerOfAccommodation(owner,aR.AccommodationId)).ToList();
        }
        public int GetNumberOfRatingsForAccommodation(Accommodation accommodation)
        {
            return accommodationRatingRepository.GetAll().Where(rating => rating.AccommodationId == accommodation.Id).Count();
        }
        public List<AccommodationRating> GetAll()
        {
              return accommodationRatingRepository.GetAll();
        }
        public AccommodationRating GetByReservationId(int reservationId)
        {
            return accommodationRatingRepository.GetByReservationId(reservationId);
        }
        public AccommodationRating GetById(int id)
        {
            return accommodationRatingRepository.GetById(id);
        }
        public AccommodationRating Update(AccommodationRating accommodationRating)
        {
            return accommodationRatingRepository.Update(accommodationRating);
        }
        public AccommodationRating Save(AccommodationRating accommodationRating)
        {
            return accommodationRatingRepository.Save(accommodationRating);
        }
        public void Delete(AccommodationRating accommodationRating)
        {
            accommodationRatingRepository.Delete(accommodationRating);
        }
        public List<AccommodationRating> GetByGuest(User guest)
        {
            return accommodationRatingRepository.GetByGuest(guest);
        }
    }
}
