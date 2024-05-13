using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookingApp.Appl.UseCases
{
    public class ProcessRequestService
    {
        private GuestRequestService guestRequestService;
        private AccommodationReservationService accommodationReservationService;
        private NotificationsService notificationService;
        public ProcessRequestService(GuestRequestService guestRequestService)
        {
            notificationService = new NotificationsService(Injector.CreateInstance<INotificationRepository>());
            this.guestRequestService = guestRequestService;
            accommodationReservationService = new AccommodationReservationService(Injector.CreateInstance<IAccommodationReservationRepository>());
        }
        public void AcceptRequest(GuestRequest guestRequest)
        {
            UpdateGuestRequest(guestRequest,null);
            UpdateReservation(guestRequest);
            SendNotification(guestRequest);
          
        }
        public void DenyRequest(GuestRequest guestRequest, string comment)
        {
            UpdateGuestRequest(guestRequest, comment);
            SendNotification(guestRequest);
        }
        private void UpdateGuestRequest(GuestRequest guestRequest,string? comment)
        {
            if (comment==null)
                guestRequest.Status = STATUS.APPROVED;
            else
            {
                guestRequest.Status = STATUS.REJECTED;
                guestRequest.Comment = comment;
            }
            guestRequestService.Update(guestRequest);
        }
        private void UpdateReservation(GuestRequest guestRequest)
        {
            AccommodationReservation reservation = accommodationReservationService.GetById(guestRequest.ReservationId);
            reservation.ReservedFrom = guestRequest.NewReservedFrom;
            reservation.ReservedTo = guestRequest.NewReservedTo;
            reservation.RescheduledReservation = 1;
            accommodationReservationService.Update(reservation);
        }
        private void SendNotification(GuestRequest guestRequest)
        {
            int receiverId = accommodationReservationService.GetById(guestRequest.ReservationId).UserId;
            notificationService.CreateNotification(receiverId, guestRequest.Id, Domain.Models.Type.REQUEST);
        }
    }
}
