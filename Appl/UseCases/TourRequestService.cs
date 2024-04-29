using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Domain.Serializer;
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
        public TourRequestService()
        {
            _repository = Injector.CreateInstance<ITourRequestRepository>();
            tourRealisationService = new TourRealisationService();
            tourReservationService = new TourReservationService();
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
        public TourRequest GetFirstTourRequest()
        {
            List<TourRequest> tourRequests = _repository.GetAll().ToList();

            return tourRequests
                .Where(x => tourReservationService.GetById(x.TourReservationId).TourRealisationId != -1)
                .MinBy(x => tourRealisationService.GetById(tourReservationService.GetById(x.TourReservationId).TourRealisationId).StartTime);
        }

        public TourRequest GetLastTourRequest()
        {
            List<TourRequest> tourRequests = _repository.GetAll().ToList();

            return tourRequests
                .Where(x => tourReservationService.GetById(x.TourReservationId).TourRealisationId != -1)
                .MaxBy(x => tourRealisationService.GetById(tourReservationService.GetById(x.TourReservationId).TourRealisationId).StartTime);
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
            // Initialize the dictionary to store month counts
            var monthCounts = new Dictionary<int, int>();
            for (int month = 1; month <= 12; month++)
            {
                monthCounts[month] = 0;
            }

            // Get all tour requests based on location and language
            var tourRequests = LocationId != 10 ?
                _repository.GetAll().Where(x => x.RangeFrom.Year == year && x.Location.Id == LocationId) :
                _repository.GetAll().Where(x => x.RangeFrom.Year == year && x.Language == (LANGUAGE)LanguageId);

            // Iterate over tour requests and count them for each month
            foreach (var request in tourRequests)
            {
                monthCounts[request.RangeFrom.Month]++;
            }
            return monthCounts;
        }
    }
}
