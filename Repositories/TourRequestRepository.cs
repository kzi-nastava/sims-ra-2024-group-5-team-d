using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Domain.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repositories
{
    public class TourRequestRepository : ITourRequestRepository
    {
        private const string FilePath = "../../../Resources/Data/tourRequests.csv";
        private readonly Serializer<TourRequest> _serializerTourRequest;

        private List<TourRequest> _tourRequests;

        public TourRequestRepository()
        {
            _serializerTourRequest = new Serializer<TourRequest>();
            _tourRequests = _serializerTourRequest.FromCSV(FilePath);
        }

        public List<TourRequest> GetAll()
        {
            return _serializerTourRequest.FromCSV(FilePath);
        }

        public TourRequest Save(TourRequest tourRequest)
        {
            tourRequest.Id = NextId();

            _tourRequests = _serializerTourRequest.FromCSV(FilePath);
            _tourRequests.Add(tourRequest);
            _serializerTourRequest.ToCSV(FilePath, _tourRequests);
            return tourRequest;
        }
        public int NextId()
        {
            _tourRequests = _serializerTourRequest.FromCSV(FilePath);
            if (_tourRequests.Count < 1)
            {
                return 1;
            }
            return _tourRequests.Max(c => c.Id) + 1;
        }
        public void Delete(TourRequest tourRequest)
        {
            _tourRequests = _serializerTourRequest.FromCSV(FilePath);
            TourRequest founded = _tourRequests.Find(c => c.Id == tourRequest.Id);
            _tourRequests.Remove(founded);
            _serializerTourRequest.ToCSV(FilePath, _tourRequests);
        }
        public void DeleteById(int id)
        {
            _tourRequests = _serializerTourRequest.FromCSV(FilePath);
            TourRequest founded = _tourRequests.Find(c => c.Id == id);
            _tourRequests.Remove(founded);
            _serializerTourRequest.ToCSV(FilePath, _tourRequests);
        }
        public TourRequest Update(TourRequest tourRequest)
        {
            _tourRequests = _serializerTourRequest.FromCSV(FilePath);
            TourRequest current = _tourRequests.Find(c => c.Id == tourRequest.Id);
            int index = _tourRequests.IndexOf(current);
            _tourRequests.Remove(current);
            _tourRequests.Insert(index, tourRequest);        
            _serializerTourRequest.ToCSV(FilePath, _tourRequests);
            return tourRequest;
        }
        public TourRequest GetById(int tourRequestId)
        {
            _tourRequests = _serializerTourRequest.FromCSV(FilePath);
            foreach (TourRequest tR in _tourRequests)
            {
                if (tR.Id == tourRequestId)
                {
                    return tR;
                }
            }
            return null;
        }
        public List<TourRequest> GetRequestsForTourist(User tourist)
        {
            _tourRequests = _serializerTourRequest.FromCSV(FilePath);
            return _tourRequests.Where(c => c.TouristId == tourist.Id).ToList();
        }
    }
}
