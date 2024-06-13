using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace BookingApp.Appl.UseCases
{
    public class GuestRatingService
    {
        private IGuestRatingRepository guestRatingRepository;
        private AccommodationService accommodationService;
        public GuestRatingService()
        {
            guestRatingRepository = Injector.CreateInstance<IGuestRatingRepository>();
            accommodationService = new AccommodationService();
        }
        public GuestRatingService(IGuestRatingRepository guestRatingRepository)
        {
            this.guestRatingRepository = guestRatingRepository;
        }
        public GuestRatingService(IGuestRatingRepository guestRatingRepository, AccommodationService accommodationService)
        {
            this.guestRatingRepository = guestRatingRepository;
            this.accommodationService = accommodationService;
        }
        public List<GuestRating> GetAllRatingsForGuest(User user)
        {
            
            List<GuestRating> guestRatings = GetByGuest(user);
            List<GuestRating> filteredRating = guestRatings.Where(gs=>IsGuestRatedByOwner(gs, accommodationService.GetById( gs.AccommodationId).Owner)).ToList();
            return filteredRating;

        }
        public List<GuestRating> GetAllGuestRatingsByOwner(User owner)
        {
            return guestRatingRepository.GetAll().Where(guestRating => IsGuestRatedByOwner(guestRating, owner)).ToList();
        }

        public bool IsGuestRatedByOwner(GuestRating guestRating, User owner)
        {
            return accommodationService.GetByUser(owner).Any(accommodation => accommodation.Id == guestRating.AccommodationId);
        }
        public List<GuestRating> GetAll()
        {
            return guestRatingRepository.GetAll();
        }
        public GuestRating Save(GuestRating guestRating)
        {
            return guestRatingRepository.Save(guestRating);
        }
        public void Delete(GuestRating guestRating)
        {
            guestRatingRepository.Delete(guestRating);
        }
        public GuestRating Update(GuestRating guestRating)
        {
            return guestRatingRepository.Update(guestRating);
        }
        public List<GuestRating> GetByGuest(User guest)
        {
            return guestRatingRepository.GetByGuest(guest);
        }
    }
}
