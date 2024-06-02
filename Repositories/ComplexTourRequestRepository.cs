using BookingApp.Appl.UseCases;
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
    public class ComplexTourRequestRepository : IComplexTourRequestRepository
    {

        private const string FilePathComplexTourRequest = "../../../Resources/Data/complexTourRequests.csv";
        

        private readonly Serializer<ComplexTourRequest> _serializerComplexRequests;


        

        private List<ComplexTourRequest> _complexTourRequests;

        public ComplexTourRequestRepository()
        {
            _serializerComplexRequests = new Serializer<ComplexTourRequest>();
            
            _complexTourRequests = _serializerComplexRequests.FromCSV(FilePathComplexTourRequest);
        }

        public void Delete(ComplexTourRequest request)
        {
            _complexTourRequests = _serializerComplexRequests.FromCSV(FilePathComplexTourRequest);
            ComplexTourRequest founded = _complexTourRequests.Find(c => c.Id == request.Id);
            _complexTourRequests.Remove(founded);
            _serializerComplexRequests.ToCSV(FilePathComplexTourRequest, _complexTourRequests);
        }

        public List<ComplexTourRequest> GetAll(User tourist)
        {
            _complexTourRequests = _serializerComplexRequests.FromCSV(FilePathComplexTourRequest);
            return _complexTourRequests.FindAll(c => c.TouristId == tourist.Id);
        }

        public List<ComplexTourRequest> GetAll()
        {
            return _serializerComplexRequests.FromCSV(FilePathComplexTourRequest);
        }

        public ComplexTourRequest GetById(int id)
        {
            _complexTourRequests = _serializerComplexRequests.FromCSV(FilePathComplexTourRequest);
            foreach (ComplexTourRequest tR in _complexTourRequests)
            {
                if (tR.Id == id)
                {
                    return tR;
                }
            }
            return null;
        }

        public int NextId()
        {
            _complexTourRequests = _serializerComplexRequests.FromCSV(FilePathComplexTourRequest);
            if (_complexTourRequests.Count < 1)
            {
                return 1;
            }
            return _complexTourRequests.Max(c => c.Id) + 1;
        }

        public ComplexTourRequest Save(ComplexTourRequest request)
        {
            request.Id = NextId();

            _complexTourRequests = _serializerComplexRequests.FromCSV(FilePathComplexTourRequest);
            _complexTourRequests.Add(request);
            _serializerComplexRequests.ToCSV(FilePathComplexTourRequest, _complexTourRequests);
            return request;
        }

        public ComplexTourRequest Update(ComplexTourRequest request)
        {
            _complexTourRequests = _serializerComplexRequests.FromCSV(FilePathComplexTourRequest);
            ComplexTourRequest current = _complexTourRequests.Find(c => c.Id == request.Id);
            int index = _complexTourRequests.IndexOf(current);
            _complexTourRequests.Remove(current);
            _complexTourRequests.Insert(index, request);
            _serializerComplexRequests.ToCSV(FilePathComplexTourRequest, _complexTourRequests);
            return request;
        }
    }
}
