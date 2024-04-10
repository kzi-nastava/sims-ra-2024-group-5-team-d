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

        //DA LI OVDE ODVOJITI SAVE FUNKCIJE (SAVE UPDATE I UPDATEOWNERSTATUS JER JE REDOSLED BITAN)
        public void RateOwner(AccommodationRating accommodationRating)
        {
            accommodationRatingRepository.Save(accommodationRating);
            Accommodation accommodation = accommodationRepository.GetById(accommodationRating.AccommodationId);
            Accommodation updatedAccommodation=UpdateAverageRating(accommodationRating.AccommodationId,accommodationRating.Cleanliness, accommodation);
            accommodationRepository.Update(updatedAccommodation);
            double averageOwnerRating=GetOwnerAverageRating(accommodationRating.GetAverageRating(), accommodation);
            accommodationService.UpdateOwnerStatus(averageOwnerRating,accommodation.Owner);
        }
        private double GetOwnerAverageRating(double averageRating,Accommodation accommodation)
        {
            List<AccommodationReservation> allReservationsForOwner = accommodationReservationService.GetAllReservationsForOwner(accommodation.Owner);
            double OwnerRatingSum = averageRating;
            int count = 1;
            allReservationsForOwner.ForEach(acReservation => {
                AccommodationRating accommodationRating = accommodationRatingRepository.GetByReservationId(acReservation.Id);
                if (accommodationRating != null)
                {
                    OwnerRatingSum +=accommodationRating.GetAverageRating();
                    count += 1;
                }
            }
           );
            return  OwnerRatingSum / count;
         

        }
        private Accommodation UpdateAverageRating(int accommodationId,int newRating, Accommodation accommodation)
        {
            List<AccommodationReservation> allReservationsForAccommodation=accommodationReservationService.GetAllReservationsForAccommodation(accommodationId);
            int accommodationRatingSum = newRating;
            int count = 1;
             allReservationsForAccommodation.ForEach(acReservation => {
                AccommodationRating accommodationRating=accommodationRatingRepository.GetByReservationId(acReservation.Id);
                if (accommodationRating != null)
                {
                     accommodationRatingSum += accommodationRating.Cleanliness;
                   count+=1;
                }                
              }
            );
            int averageRating = accommodationRatingSum / count;
            accommodation.AverageRating = averageRating;
            return accommodation;
        }
    }
}
