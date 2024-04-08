using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
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
        private AccommodationService accommodationService;
        public GuestRatingService()
        {
            accommodationService = new AccommodationService();
            guestRatingRepository = Injector.CreateInstance<IGuestRatingRepository>();
        }
        public List<GuestRating> GetAllGuestRatingsByUser(User user)
        {
            return guestRatingRepository.GetAll().Where(guestRating => accommodationService.IsRatedByUser(guestRating, user)).ToList();
        }
    }
}
