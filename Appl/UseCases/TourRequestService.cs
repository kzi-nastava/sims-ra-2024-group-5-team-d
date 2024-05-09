using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Domain.Serializer;
using HarfBuzzSharp;
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
        public TourRequestService()
        {
            _repository = Injector.CreateInstance<ITourRequestRepository>();
            tourRealisationService = new TourRealisationService();
            tourReservationService = new TourReservationService();
            tourGuestService = new TourGuestService();
            userService = new UserService();
        }
        public List<TourRequest> GetAll()
        {
            return _repository.GetAll();
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
            return _repository.GetRequestsForTourist(tourist);
        }
        public int GetRequestsInAYear(int year, int LanguageId, int LocationId)
        {
            if(LocationId != 10)
            {
                return _repository.GetAll().Where(x => x.RangeFrom.Year == year && x.Location.Id == LocationId).Count();
            }
            return _repository.GetAll().Where(x => x.RangeFrom.Year == year && x.Language == (LANGUAGE)LanguageId).Count();

        }
        public Dictionary<int, int> GetRequestsInAYearByMonths(int year, int LanguageId, int LocationId)
        {
            var monthCounts = new Dictionary<int, int>();
            for (int month = 1; month <= 12; month++)
            {
                monthCounts[month] = 0;
            }

            var tourRequests = LocationId != 10 ?
                _repository.GetAll().Where(x => x.RangeFrom.Year == year && x.Location.Id == LocationId) :
                _repository.GetAll().Where(x => x.RangeFrom.Year == year && x.Language == (LANGUAGE)LanguageId);

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

        public double AverageNumberOfGuestsOnAcceptedRequests(int year)
        {
            int totalNumberOfGuests = 0;
            int numberOfRequests = 0;

            GetAll().ForEach(req =>
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
                if (req.Status == STATE.ACCEPTED)
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

            GetAll().ForEach(request =>
            {
                if(request.TouristId == touristId)
                {
                    if(request.Location.Id == locationId && request.Status != STATE.ACCEPTED)
                    {
                        anyNotFulfilledRequestsOnGivenLocation = true;
                    }
                    else if (request.Location.Id == locationId && request.Status == STATE.ACCEPTED)
                    {
                        anyAcceptedRequestOnGivenLocation = true;
                    }
                }
            });

            return (anyNotFulfilledRequestsOnGivenLocation && !anyAcceptedRequestOnGivenLocation);
        }

        public bool IsLangaugeRequestFulfilled(LANGUAGE language, int touristId)
        {
            bool anyNotFulfilledRequestsOnGivenLanguage = false;
            bool anyAcceptedRequestOnGivenLanguage = false;

            GetAll().ForEach(request =>
            {
                if (request.TouristId == touristId)
                {
                    if (request.Language == language && request.Status != STATE.ACCEPTED)
                    {
                        anyNotFulfilledRequestsOnGivenLanguage = true;
                    }
                    else if (request.Language == language && request.Status == STATE.ACCEPTED)
                    {
                        anyAcceptedRequestOnGivenLanguage = true;
                    }
                }
            });

            return (anyNotFulfilledRequestsOnGivenLanguage && !anyAcceptedRequestOnGivenLanguage);

        }
    }
}
