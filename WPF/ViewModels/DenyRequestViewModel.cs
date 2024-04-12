using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.WPF.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels
{
    public class ProcessRequestViewModel
    {
        public ICommand DenyRequestCommand { get; set; }
        public ICommand AcceptRequestCommand { get; set; }
        public string GuestName { get; set; }
        public string Comment { get; set; }
        private User loggedInUser;
        private GuestRequestService guestRequestService;
        private GuestRequest guestRequest;
        private AccommodationReservationService accommodationReservationService;
        private RequestViewModel guestRequestViewModel;
        public ProcessRequestViewModel(User user, RequestViewModel guestRequest)
        {
            accommodationReservationService = new AccommodationReservationService();
            DenyRequestCommand = new RelayCommand(DenyRequest);
            AcceptRequestCommand = new RelayCommand(AcceptRequest);
            loggedInUser = user;
            guestRequestService = new GuestRequestService();
            this.guestRequest = guestRequestService.GetById(guestRequest.RequestId);
            this.guestRequestViewModel = guestRequest;
        }
        private void DenyRequest()
        {
            RequestsViewModel.Requests.Remove(guestRequestViewModel);
            guestRequest.Status = STATUS.REJECTED;
            guestRequest.Comment = Comment;
            guestRequestService.Update(guestRequest);

        }
        private void AcceptRequest()
        {
            RequestsViewModel.Requests.Remove(guestRequestViewModel);
            guestRequest.Status = STATUS.APPROVED;
            guestRequestService.Update(guestRequest);
            AccommodationReservation reservation = accommodationReservationService.GetById(guestRequest.ReservationId);
            reservation.ReservedFrom = guestRequest.NewReservedFrom;
            reservation.ReservedTo = guestRequest.NewReservedTo;
            reservation.RescheduledReservation = 1;
            accommodationReservationService.Update(reservation);
        }
    }
}
