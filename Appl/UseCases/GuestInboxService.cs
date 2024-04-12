using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Domain.Serializer;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Appl.UseCases
{
    public class GuestInboxService
    {
        public IAccommodationReservationRepository accommodationReservationRepository;
        public GuestRequestService guestRequestService { get; set; }

        public GuestInboxService() 
        {
            accommodationReservationRepository = Injector.CreateInstance<IAccommodationReservationRepository>();
            guestRequestService = new GuestRequestService();
        }
        
        public List<GuestRequest> GetApprovedRequests (User user)
        {
            return guestRequestService.RequestsByUser(user).Where(request => request.IsApproved()).ToList();
        }

        public List<GuestRequest> GetInProcessRequests (User user)
        {
            return guestRequestService.RequestsByUser(user).Where(request => request.IsInProgress()).ToList();
        }

        public List<GuestRequest> GetRejectedRequests(User user)
        {
            return guestRequestService.RequestsByUser(user).Where(request=> request.IsRejected()).ToList();
        }
















    }
}
