using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            List<AccommodationRating> allRatings=accommodationRatingRepository.GetAll().Where(aR=> accommodationRepository.GetByUser(owner).Any(accommodation=> accommodation.Id==aR.AccommodationId)).ToList();
            allRatings.RemoveAll(rating=> guestRatingService.GetAllGuestRatingsByUser(owner).All(guestrating=> guestrating.ReservationId != rating.ReservationId));
            return allRatings;
        }
    }
}
