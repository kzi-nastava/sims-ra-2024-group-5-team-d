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
        private GuestRequestService guestRequestService;

        public GuestInboxService() 
        {
            guestRequestService = new GuestRequestService();
        }
        
        public List<GuestRequest> GetApprovedRequests (User user)
        {
            return guestRequestService.RequestsByUser(user).Where(request => request.IsApproved()).ToList();
        }

        public List<GuestRequest> GetInProcessRequests (User user)
        {
            return guestRequestService.RequestsByUser(user).Where(request => request.IsInProcess()).ToList();
        }

        public List<GuestRequest> GetRejectedRequests(User user)
        {
            return guestRequestService.RequestsByUser(user).Where(request=> request.IsRejected()).ToList();
        }
















    }
}
