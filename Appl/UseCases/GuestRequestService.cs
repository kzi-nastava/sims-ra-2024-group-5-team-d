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
    public class GuestRequestService
    {
        private IGuestRequestRepository guestRequestRepository;

        public GuestRequestService()
        {
            guestRequestRepository = Injector.CreateInstance<IGuestRequestRepository>();
        }
        public List<GuestRequest> GetAll()
        {
            return guestRequestRepository.GetAll();
        }
        public GuestRequest Save(GuestRequest request)
        {
            return guestRequestRepository.Save(request);
        }
        public void Delete(GuestRequest request)
        {
            guestRequestRepository.Delete(request);
        }
        public GuestRequest Update(GuestRequest request)
        {
            return guestRequestRepository.Update(request);
        }
        public GuestRequest GetById(int id)
        {
            return guestRequestRepository.GetById(id);  
        }
    }
}
