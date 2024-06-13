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
        private AccommodationRatingService accommodationRatingService;
        private AccommodationReservationService accommodationReservationService;
        private AccommodationService accommodationService;
        private UserService userService;
        public RateOwnerService()
        {
            userService = new UserService();
            accommodationRatingService = new AccommodationRatingService();
            accommodationService = new AccommodationService();
            accommodationReservationService = new AccommodationReservationService();
        }

        public void RateOwner(AccommodationRating accommodationRating)
        {

            Accommodation accommodation = accommodationService.GetById(accommodationRating.AccommodationId);
            double averageRating=accommodationRating.GetAverageRating();


            List<AccommodationReservation> allReservationsForOwner = accommodationReservationService.GetAllReservationsForOwner(accommodation.Owner).Where(reservation=>reservation.IsFinished()).ToList();
            List<AccommodationReservation> allReservationsForAccommodation = accommodationReservationService.GetAllReservationsForAccommodation(accommodation.Id).Where(reservation => reservation.IsFinished()).ToList();
            double ownerAverageRating = GetAverageRatingSum(allReservationsForOwner, averageRating);
            double accommodationAverageRating = GetAverageRatingSum(allReservationsForAccommodation, averageRating);

            accommodation.AverageRating = accommodationAverageRating;
            accommodationRatingService.Save(accommodationRating);
            accommodationService.Update(accommodation);
            if (HasEnoughReviews(accommodation.Owner))
                UpdateOwnerStatus(ownerAverageRating, accommodation.Owner);
        }
        private bool HasEnoughReviews(User owner)
        {
            return accommodationRatingService.GetNumberOfRatingsForOwner(owner) >= 50;
        }
        private double GetAverageRatingSum(List<AccommodationReservation> ReservationsForAccommodation,double newRating) {
            double ratingSum = newRating;
            int count = 1;
            ReservationsForAccommodation.ForEach(acReservation => {
                AccommodationRating accommodationRating = accommodationRatingService.GetByReservationId(acReservation.Id);
                if (accommodationRating != null)
                {
                    ratingSum += accommodationRating.GetAverageRating();
                    count += 1;
                }
            }
           );
            return ratingSum / count;
        }
        public void UpdateOwnerStatus(double averageOwnerRating, User owner)
        {
            if (averageOwnerRating >= 4.5)
                userService.PromoteUser(owner);
            else if (averageOwnerRating < 4.5)
                userService.DemoteUser(owner);
        }
    }
}
