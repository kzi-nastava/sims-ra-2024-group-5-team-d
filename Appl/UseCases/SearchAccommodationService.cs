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

    public class SearchAccommodationService
    {
        public IAccommodationRepository accommodationRepository;
        public SearchAccommodationService() {

            accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();

        }
        public List<Accommodation> GetSearchedAccommodation(string accommodationName,int accommodationType,int locationId,int numberOfPeople,int numberOfDays)
        {

            List<Accommodation> foundAccommodation = new List<Accommodation>();
            accommodationRepository.GetAll().ForEach(accommodation =>
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
        public List<Accommodation> GetSearchedAccommodationAnytimeAnywhere(string accommodationName, int accommodationType, int locationId, int numberOfPeople, int numberOfDays)
        {

            List<Accommodation> foundAccommodation = new List<Accommodation>();
            accommodationRepository.GetAll().ForEach(accommodation =>
            {
                if (IsWantedAccommodationAnytimeAnywhere(accommodation, accommodationName, accommodationType, locationId, numberOfPeople, numberOfDays))
                    foundAccommodation.Add(accommodation);
            });
            return foundAccommodation;
        }
        private bool IsWantedAccommodationAnytimeAnywhere(Accommodation accommodation, string accommodationName, int accommodationType, int locationId, int numberOfPeople, int numberOfDays)
        {
            bool isAccommodationNameContained = accommodation.Name.Contains(accommodationName);
            bool isAccommodationTypeValid = accommodation.Type == (TYPE)accommodationType || accommodationType == 3;
            bool isLocationValid = accommodation.Location.Id == locationId || locationId == 10;
            bool isNumberOfPeopleValid = accommodation.Capacity >= numberOfPeople;
            bool isNumberOfDaysValid = accommodation.MinStay <= numberOfDays;

            return isAccommodationNameContained && isAccommodationTypeValid && isLocationValid && isNumberOfPeopleValid && isNumberOfDaysValid;
        }
    }

   

}
