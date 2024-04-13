using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Appl.UseCases
{
    public class ProcessRequestService
    {
        private GuestRequestService guestRequestService;
        private AccommodationReservationService accommodationReservationService;
        private NotificationsService notificationService;
        public ProcessRequestService()
        {
            notificationService = new NotificationsService();
            guestRequestService = new GuestRequestService();
            accommodationReservationService = new AccommodationReservationService();
        }
        public void AcceptRequest(GuestRequest guestRequest)
        {
            UpdateGuestRequest(guestRequest);
            UpdateReservation(guestRequest);
            SendNotification(guestRequest);
          
        }
        public void DenyRequest(GuestRequest guestRequest, string comment)
        {
            guestRequest.Status = STATUS.REJECTED;
            guestRequest.Comment = comment;
            guestRequestService.Update(guestRequest);
            SendNotification(guestRequest);
        }
        private void UpdateGuestRequest(GuestRequest guestRequest)
        {
            guestRequest.Status = STATUS.APPROVED;
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
            notificationService.CreateNotification(receiverId, guestRequest.ReservationId, Domain.Models.Type.REQUEST);
        }
    }
}
