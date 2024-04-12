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
        private IAccommodationReservationRepository accommodationReservationRepository;

        public GuestRequestService()
        {
            guestRequestRepository = Injector.CreateInstance<IGuestRequestRepository>();
            accommodationReservationRepository = Injector.CreateInstance<IAccommodationReservationRepository>();
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
        public List<GuestRequest> RequestsByUser(User user)
        {
            List<GuestRequest> requests = guestRequestRepository.GetAll();
            List<GuestRequest> requestByUser = requests.Where(request => accommodationReservationRepository.GetById(request.ReservationId).UserId == user.Id).ToList();
            return requestByUser;
        }
    }
}
