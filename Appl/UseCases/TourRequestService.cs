using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Domain.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Appl.UseCases
{
    public class TourRequestService
    {
        private ITourRequestRepository _repository;
        private TourRealisationService tourRealisationService;
        public TourRequestService()
        {
            _repository = Injector.CreateInstance<ITourRequestRepository>();
            tourRealisationService = new TourRealisationService();
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
            List<TourRequest> tourRequest = _repository.GetAll().ToList();
            return tourRequest.MinBy(x => tourRealisationService.GetById(x.TourRealisationId).StartTime);
        }
        public TourRequest GetLastTourRequest()
        {
            List<TourRequest> tourRequest = _repository.GetAll().ToList();
            return tourRequest.MaxBy(x => tourRealisationService.GetById(x.TourRealisationId).StartTime);
        }
    }
}
