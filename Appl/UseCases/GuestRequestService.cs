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
        private AccommodationReservationService accommodationReservationService;

        public GuestRequestService()
        {
            guestRequestRepository = Injector.CreateInstance<IGuestRequestRepository>();
            accommodationReservationService = new AccommodationReservationService();
        }
        public GuestRequestService(IGuestRequestRepository guestRequestRepository, AccommodationReservationService accommodationReservationService)
        {
            this.guestRequestRepository = guestRequestRepository;
            this.accommodationReservationService = accommodationReservationService;
        }
        public GuestRequestService(IGuestRequestRepository guestRequestRepository)
        {
            this.guestRequestRepository = guestRequestRepository;
        }
        public List<GuestRequest> RequestsByUser(User user)
        {
            List<GuestRequest> requests = guestRequestRepository.GetAll();
            List<GuestRequest> requestByUser = requests.Where(request => accommodationReservationService.GetById(request.ReservationId).UserId == user.Id).ToList();
            return requestByUser;
        }
        public List<GuestRequest> GetAllRequestsForOwner(User owner)
        {
            List<GuestRequest> guestRequestsInProgress = guestRequestRepository.GetAll().Where(guestRequests => guestRequests.IsInProcess()).ToList();
            List<AccommodationReservation> ownerReservations = accommodationReservationService.GetAllReservationsForOwner(owner).Where(reservation=>reservation.IsActive()).ToList();
            return guestRequestsInProgress.Where(guestRequests => ownerReservations.Any(reservation => guestRequests.ReservationId == reservation.Id)).ToList();
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
