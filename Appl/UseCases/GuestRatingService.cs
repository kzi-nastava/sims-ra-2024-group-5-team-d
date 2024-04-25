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
        private IAccommodationRepository accommodationRepository;
        private AccommodationService accommodationService;
        public GuestRatingService()
        {
            accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
            guestRatingRepository = Injector.CreateInstance<IGuestRatingRepository>();
            accommodationService = new AccommodationService();
        }
        public List<GuestRating> GetAllRatingsForGuest(User user)
        {
            
            List<GuestRating> guestRatings = GetByGuest(user);
            List<GuestRating> filteredRating = guestRatings.Where(gs=>IsGuestRatedByOwner(gs, accommodationService.GetById( gs.AccommodationId).Owner)).ToList();
            Debug.WriteLine("aaaaaaaaaaaaa");
            Debug.WriteLine(guestRatings.Count());
            Debug.WriteLine(filteredRating.Count());
            return filteredRating;

        }
        public List<GuestRating> GetAllGuestRatingsByOwner(User owner)
        {
            return guestRatingRepository.GetAll().Where(guestRating => IsGuestRatedByOwner(guestRating, owner)).ToList();
        }

        public bool IsGuestRatedByOwner(GuestRating guestRating, User owner)
        {
            return accommodationRepository.GetByUser(owner).Any(accommodation => accommodation.Id == guestRating.AccommodationId);
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
