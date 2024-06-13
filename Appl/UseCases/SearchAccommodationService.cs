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

    public class SearchAccommodationService
    {
        public AccommodationService accommodationService;
        private AvailableDatesForReservationService availableDatesForReservationService;
        public SearchAccommodationService() {

            accommodationService = new AccommodationService();
            availableDatesForReservationService = new AvailableDatesForReservationService();

        }
        public List<Accommodation> GetSearchedAccommodation(string accommodationName,int accommodationType,int locationId,int numberOfPeople,int numberOfDays)
        {

            List<Accommodation> foundAccommodation = new List<Accommodation>();
            accommodationService.GetAll().ForEach(accommodation =>
             {
                 if (IsWantedAccommodation(accommodation, accommodationName, accommodationType, locationId, numberOfPeople, numberOfDays))
                       foundAccommodation.Add(accommodation);
                 });
            return foundAccommodation;
        }
        private bool IsWantedAccommodation(Accommodation accommodation, string accommodationName, int accommodationType, int locationId, int numberOfPeople, int numberOfDays)
        {
            bool isAccommodationNameContained = accommodation.Name.Contains(accommodationName);
            bool isAccommodationTypeValid = accommodation.Type == (TYPE)accommodationType || accommodationType==3;
            bool isLocationValid = accommodation.Location.Id == locationId || locationId==10;
            bool isNumberOfPeopleValid = accommodation.Capacity >= numberOfPeople;
            bool isNumberOfDaysValid = accommodation.MinStay <= numberOfDays;

            return isAccommodationNameContained && isAccommodationTypeValid && isLocationValid && isNumberOfPeopleValid && isNumberOfDaysValid;
        }
        public List<Accommodation> AnytimeAnywhere(int numberOfPeople, int numberOfDays, DateTime fromDate, DateTime toDate)
        {

            List<Accommodation> foundAccommodation = new List<Accommodation>();
            accommodationService.GetAll().ForEach(accommodation =>
            {
                if (accommodation.Capacity>=numberOfPeople && accommodation.MinStay<=numberOfDays)
                {

                    if (fromDate.Date==toDate.Date) 
                    {
                        foundAccommodation.Add(accommodation);
                    }
                    else if (availableDatesForReservationService.GetAvailableDatesInGivenRange(fromDate, toDate, numberOfDays, accommodation).Count() != 0)
                    {
                         foundAccommodation.Add(accommodation);
                    }
                    
                }
            });
            return foundAccommodation;
        }


    }

   

}
