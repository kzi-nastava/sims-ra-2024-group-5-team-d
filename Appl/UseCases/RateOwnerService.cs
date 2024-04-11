using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Appl.UseCases
{
    public class RateOwnerService
    {
        private IAccommodationRatingRepository accommodationRatingRepository;
        private AccommodationReservationService accommodationReservationService;
        private IAccommodationRepository accommodationRepository;
        private AccommodationService accommodationService;
        public RateOwnerService()
        {
            accommodationService = new AccommodationService();
            accommodationRepository=Injector.CreateInstance<IAccommodationRepository>();
            accommodationReservationService = new AccommodationReservationService();
            accommodationRatingRepository=Injector.CreateInstance<IAccommodationRatingRepository>();
        }

        public void RateOwner(AccommodationRating accommodationRating)
        {

            Accommodation accommodation = accommodationRepository.GetById(accommodationRating.AccommodationId);
            double averageRating=accommodationRating.GetAverageRating();


            List<AccommodationReservation> allReservationsForOwner = accommodationReservationService.GetAllReservationsForOwner(accommodation.Owner);
            List<AccommodationReservation> allReservationsForAccommodation = accommodationReservationService.GetAllReservationsForAccommodation(accommodation.Id);
            double ownerAverageRating = GetAverageRatingSum(allReservationsForOwner, averageRating);
            double accommodationAverageRating = GetAverageRatingSum(allReservationsForAccommodation, averageRating);

            accommodation.AverageRating = accommodationAverageRating;
            accommodationRatingRepository.Save(accommodationRating);
            accommodationRepository.Update(accommodation);
            Debug.WriteLine("Accommodation average rating: " + accommodationAverageRating);
            Debug.WriteLine("Owner average rating: " + ownerAverageRating);
            accommodationService.UpdateOwnerStatus(ownerAverageRating, accommodation.Owner);
        }

        private double GetAverageRatingSum(List<AccommodationReservation> ReservationsForAccommodation,double newRating) {
            double ratingSum = newRating;
            int count = 1;
            ReservationsForAccommodation.ForEach(acReservation => {
                AccommodationRating accommodationRating = accommodationRatingRepository.GetByReservationId(acReservation.Id);
                if (accommodationRating != null)
                {
                    ratingSum += accommodationRating.GetAverageRating();
                    count += 1;
                }
            }
           );
            return ratingSum / count;
        }
    }
}
