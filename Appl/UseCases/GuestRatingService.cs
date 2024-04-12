using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Appl.UseCases
{
    public class GuestRatingService
    {
        private IGuestRatingRepository guestRatingRepository;
        private IAccommodationRepository accommodationRepository;
        public GuestRatingService()
        {
            accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
            guestRatingRepository = Injector.CreateInstance<IGuestRatingRepository>();
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
