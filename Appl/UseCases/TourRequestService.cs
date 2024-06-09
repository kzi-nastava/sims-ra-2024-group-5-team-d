using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Domain.Serializer;
using HarfBuzzSharp;
using iText.Commons.Bouncycastle.Cert.Ocsp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Appl.UseCases
{
    public class TourRequestService
    {
        private ITourRequestRepository _repository;
        private TourRealisationService tourRealisationService;
        private TourReservationService tourReservationService;
        private TourGuestService tourGuestService;
        private UserService userService;
        private LocationService locationService;
        private ComplexSimpleRequestPairService complexSimpleRequestPairService;
        public TourRequestService()
        {
            _repository = Injector.CreateInstance<ITourRequestRepository>();
            tourRealisationService = new TourRealisationService();
            tourReservationService = new TourReservationService();
            tourGuestService = new TourGuestService();
            userService = new UserService();
            complexSimpleRequestPairService = new ComplexSimpleRequestPairService();
            locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
        }
        public List<TourRequest> GetAll()
        {
            return _repository.GetAll().Where(req => IsNotPartOfComplexRequest(req)).ToList();
        }
        public List<TourRequest> GetAllSimpleRequests()
        {
            List<TourRequest> simpleRequests = new List<TourRequest>();
            complexSimpleRequestPairService.GetAll().ForEach(pair =>
            {
                if (!simpleRequests.Contains(GetById(pair.SimpleRequestId)))
                {
                    simpleRequests.Add(GetById(pair.SimpleRequestId));
                }
            });

            return simpleRequests;
        }
        public TourRequest Save(TourRequest tourRequest)
        {
            return _repository.Save(tourRequest);
        }
        public int NextId()
        {
            return _repository.NextId();
        }
        public void Delete(TourRequest tourRequest)
        {
            _repository.Delete(tourRequest);
        }
        public void DeleteById(int id)
        {
            _repository.DeleteById(id);
        }
        public TourRequest Update(TourRequest tourRequest)
        {
            return _repository.Update(tourRequest);
        }
        public TourRequest GetById(int tourRequestId)
        {
            return _repository.GetById(tourRequestId);
        }
        public List<TourRequest> GetRequestsForTourist(User tourist)
        {
            return _repository.GetRequestsForTourist(tourist).Where(req => IsNotPartOfComplexRequest(req)).ToList();
        }
        public int GetRequestsInAYear(int year, int LanguageId, int LocationId, User tourist)
        {
            if(LocationId != 10)
            {
                return GetRequestsForTourist(tourist).Where(x => x.RangeFrom.Year == year && x.Location.Id == LocationId && IsNotPartOfComplexRequest(x)).Count();
            }
            return GetRequestsForTourist(tourist).Where(x => x.RangeFrom.Year == year && x.Language == (LANGUAGE)LanguageId && IsNotPartOfComplexRequest(x)).Count();
        }
        public Dictionary<int, int> GetRequestsInAYearByMonths(int year, int LanguageId, int LocationId, User tourist)
        {
            var monthCounts = new Dictionary<int, int>();
            for (int month = 1; month <= 12; month++)
            {
                monthCounts[month] = 0;
            }

            var tourRequests = LocationId != 10 ?
                GetRequestsForTourist(tourist).Where(x => x.RangeFrom.Year == year && x.Location.Id == LocationId && IsNotPartOfComplexRequest(x)) :
                GetRequestsForTourist(tourist).Where(x => x.RangeFrom.Year == year && x.Language == (LANGUAGE)LanguageId && IsNotPartOfComplexRequest(x));

            foreach (var request in tourRequests)
            {
                monthCounts[request.RangeFrom.Month]++;
            }
            return monthCounts;
        }

        public void Validate()
        {
            GetAll().ForEach(req =>
            {
                if(!req.IsAcceptable() && req.Status == STATE.PENDING)
                {
                    req.Status = STATE.INVALID;
                    Update(req);
                }
            });
        }


        public LANGUAGE GetMostWantedLanguage()
        {
            var requests = GetAll(); // Assuming this method returns a list of requests

            // Group requests by language and count occurrences of each language
            var languageCounts = requests
                .Where(req => req.RangeFrom >= DateTime.Now.AddYears(-1))
                .GroupBy(req => req.Language)
                .Select(group => new
                {
                    Language = group.Key,
                    Count = group.Count()
                })
                .OrderByDescending(x => x.Count); // Order by count in descending order

            // Now you can access the most chosen language
            var mostChosenLanguage = languageCounts.FirstOrDefault().Language;

            return mostChosenLanguage;
        }

        public Location GetMostWantedLocation()
        {
            var requests = GetAll(); // Assuming this method returns a list of requests

            // Initialize a dictionary to store location counts
            var locationCounts = new Dictionary<int, int>();

            // Count occurrences of each location
            foreach (var req in requests.Where(req => req.RangeFrom >= DateTime.Now.AddYears(-1)))
            {
                if (locationCounts.ContainsKey(req.Location.Id))
                {
                    locationCounts[req.Location.Id]++;
                }
                else
                {
                    locationCounts[req.Location.Id] = 1;
                }
            }

            // Find the location with the highest count
            int maxCount = 0;
            int mostChosenLocationId = 0;
            foreach (var kvp in locationCounts)
            {
                if (kvp.Value > maxCount)
                {
                    maxCount = kvp.Value;
                    mostChosenLocationId = kvp.Key;
                }
            }
            return locationService.GetById(mostChosenLocationId);
        }
      
        public double AverageNumberOfGuestsOnAcceptedRequests(int year, User tourist)
        {
            int totalNumberOfGuests = 0;
            int numberOfRequests = 0;

            GetRequestsForTourist(tourist).ForEach(req =>
            {
                if (req.RangeFrom.Year == year && req.Status == STATE.ACCEPTED)
                {
                    tourGuestService.GetAllTourGuests().ForEach(x =>
                    {
                        if(x.TourReservationId == req.TourReservationId)
                        {
                            totalNumberOfGuests++;
                        }
                    });
                    numberOfRequests++;
                }
            });


            return (totalNumberOfGuests/(double)numberOfRequests);
        }

        public double AllTimeAverageNumberOfPeopleOnAcceptedRequests()
        {
            int totalNumberOfGuests = 0;
            int numberOfRequests = 0;

            GetAll().ForEach(req =>
            {
                if (req.Status == STATE.ACCEPTED && !GetAllSimpleRequests().Contains(req))
                {
                    tourGuestService.GetAllTourGuests().ForEach(x =>
                    {
                        if (x.TourReservationId == req.TourReservationId)
                        {
                            totalNumberOfGuests++;
                        }
                    });
                    numberOfRequests++;
                }
            });


            return (totalNumberOfGuests / (double)numberOfRequests);
        }

        public bool IsLocationRequestFulfilled(int locationId, int touristId)
        {
            bool anyNotFulfilledRequestsOnGivenLocation = false;
            bool anyAcceptedRequestOnGivenLocation = false;

            GetRequestsForTourist(userService.GetById(touristId)).ForEach(request =>
            {
                    if(request.Location.Id == locationId && request.Status != STATE.ACCEPTED)
                    {
                        anyNotFulfilledRequestsOnGivenLocation = true;
                    }
                    else if (request.Location.Id == locationId && request.Status == STATE.ACCEPTED)
                    {
                        anyAcceptedRequestOnGivenLocation = true;
                    }
            });

            return (anyNotFulfilledRequestsOnGivenLocation && !anyAcceptedRequestOnGivenLocation);
        }

        public bool IsLangaugeRequestFulfilled(LANGUAGE language, int touristId)
        {
            bool anyNotFulfilledRequestsOnGivenLanguage = false;
            bool anyAcceptedRequestOnGivenLanguage = false;

            GetRequestsForTourist(userService.GetById(touristId)).ForEach(request =>
            {                
                    if (request.Language == language && request.Status != STATE.ACCEPTED)
                    {
                        anyNotFulfilledRequestsOnGivenLanguage = true;
                    }
                    else if (request.Language == language && request.Status == STATE.ACCEPTED)
                    {
                        anyAcceptedRequestOnGivenLanguage = true;
                    }
            });

            return (anyNotFulfilledRequestsOnGivenLanguage && !anyAcceptedRequestOnGivenLanguage);
        
        }

        public bool IsNotPartOfComplexRequest(TourRequest request)
        {
            foreach(TourRequest tR in GetAllSimpleRequests())
            {
                if (request.Id == tR.Id)
                    return false;
            }
            return true;
        }

        public bool ShouldAcceptedRequestStillBeValid(TourRequest request)
        {
            return DateTime.Now <= tourRealisationService.GetById(tourReservationService.GetById(request.TourReservationId).TourRealisationId).StartTime.AddDays(-2);
        }
    }
}
