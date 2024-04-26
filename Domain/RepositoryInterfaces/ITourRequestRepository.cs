using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface ITourRequestRepository
    {
        public List<TourRequest> GetAll();
        public TourRequest GetById(int id);
        public TourRequest Save(TourRequest request);
        public int NextId();
        public void Delete(TourRequest request);
        public void DeleteById(int id);
        public TourRequest Update(TourRequest request);
        public List<TourRequest> GetRequestsForTourist(User tourist);

    }
}
