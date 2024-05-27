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
        private GuestRatingService guestRatingService;
        private AccommodationService accommodationService;
        public AccommodationRatingService()
        {
            accommodationService = new AccommodationService();
            guestRatingService = new GuestRatingService();
            accommodationRatingRepository = Injector.CreateInstance<IAccommodationRatingRepository>();
        }
        public AccommodationRatingService(IAccommodationRatingRepository accommodationRatingRepository)
        {
            this.accommodationRatingRepository = accommodationRatingRepository;
        }
        public AccommodationRatingService(IAccommodationRatingRepository accommodationRatingRepository, AccommodationService accommodationService,GuestRatingService guestRatingService)
        {
            this.guestRatingService = guestRatingService;
            this.accommodationService = accommodationService;
            this.accommodationRatingRepository = accommodationRatingRepository;
        }
        public AccommodationRatingService(IAccommodationRatingRepository accommodationRatingRepository,AccommodationService accommodationService)
        {
            this.accommodationService = accommodationService;
            this.accommodationRatingRepository = accommodationRatingRepository;
        }
        public bool IsReservationRated (int reservationId)
        {
            return !GetAll().Any(rating => rating.ReservationId == reservationId);
        }
        public double GetAverageRatingForOwner(User user)
        {
            return FindAllRatingsForOwner(user).Sum(rating => rating.GetAverageRating()) / GetNumberOfRatingsForOwner(user);
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
        public int NumberOf5StarRatingsForOwner(User owner)
        {
            return FindAllRatingsForOwner(owner).Where(rating => rating.GetAverageRating()>=4.5).Count();
        }
        public int NumberOf4StarRatingsForOwner(User owner)
        {
            return FindAllRatingsForOwner(owner).Where(rating => rating.GetAverageRating() >= 3.5 && rating.GetAverageRating() < 4.5).Count();
        }
        public int NumberOf3StarRatingsForOwner(User owner)
        {
            return FindAllRatingsForOwner(owner).Where(rating => rating.GetAverageRating() >= 2.5 && rating.GetAverageRating() < 3.5).Count();
        }
        public int NumberOf2StarRatingsForOwner(User owner)
        {
            return FindAllRatingsForOwner(owner).Where(rating => rating.GetAverageRating() >= 1.5 && rating.GetAverageRating()<2.5).Count();
        }
        public int NumberOf1StarRatingsForOwner(User owner)
        {
            return FindAllRatingsForOwner(owner).Where(rating => rating.GetAverageRating() < 1.5).Count();
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
        public int NextId()
        {
            return accommodationRatingRepository.NextId();
        }

        public string GenerateRenovationText(AccommodationRating rating)
        {
            switch(rating.LevelOfRenovation)
            {
                case 1:
                    return " It would be nice to renovate some small things, but everything works fine without it.";
                case 2:
                    return "Small complaints about the accommodation that, if addressed, would make it perfect.";
                case 3:
                    return "A few things that really bothered us should be renovated.";
                case 4:
                    return "There are many bad things, and renovation is really necessary.";
                case 5:
                    return "The accommodation is in very poor condition and is not worth renting unless it is renovated.";
                    default:
                    return "";
            }
        }
    }
}
