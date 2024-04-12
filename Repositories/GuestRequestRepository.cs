using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Domain.Serializer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repositories
{
    public class GuestRequestRepository: IGuestRequestRepository
    {

        private const string FilePath = "../../../Resources/Data/guestRequests.csv";

        private readonly Serializer<GuestRequest> _serializer;

        private List<GuestRequest> guestRequests;

        public GuestRequestRepository()
        {
            _serializer = new Serializer<GuestRequest>();
            guestRequests = _serializer.FromCSV(FilePath);
        }

        public List<GuestRequest> GetAll()
        {
            guestRequests = _serializer.FromCSV(FilePath);
            return guestRequests;
        }
        public GuestRequest Save(GuestRequest request)
        {
            request.Id = NextId();
            guestRequests = _serializer.FromCSV(FilePath);
            guestRequests.Add(request);
            _serializer.ToCSV(FilePath, guestRequests);
            return request;
        }

        public int NextId()
        {
            guestRequests = _serializer.FromCSV(FilePath);
            if (guestRequests.Count < 1)
            {
                return 1;
            }
            return guestRequests.Max(c => c.Id) + 1;
        }

        public void Delete(GuestRequest request)
        {
            guestRequests = _serializer.FromCSV(FilePath);
            GuestRequest founded = guestRequests.Find(r => r.Id == request.Id);
            guestRequests.Remove(founded);
            _serializer.ToCSV(FilePath, guestRequests);
        }

        public GuestRequest Update(GuestRequest request)
        {
            guestRequests = _serializer.FromCSV(FilePath);
            GuestRequest current = guestRequests.Find(r => r.Id == request.Id);
            int index = guestRequests.IndexOf(current);
            guestRequests.Remove(current);
            guestRequests.Insert(index, request);       // keep ascending order of ids in file 
            _serializer.ToCSV(FilePath, guestRequests);
            return request;
        }
        public GuestRequest GetById(int id)
        {
            guestRequests = _serializer.FromCSV(FilePath);
            return guestRequests.Find(request => request.Id == id);
        }
        public List<GuestRequest> GetByUser(User user)
        {
            guestRequests = _serializer.FromCSV(FilePath);
            return guestRequests.FindAll(request => request.Id == user.Id);
        }
    }
}
