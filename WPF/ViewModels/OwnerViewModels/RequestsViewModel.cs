using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.WPF.Commands;
using BookingApp.WPF.Views.OwnerView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    public class RequestsViewModel
    {
        public ICommand AcceptRequestCommand { get; set; }
        public ICommand DenyRequestCommand { get; set; }
        public static ObservableCollection<RequestViewModel> Requests { get; set; }

        private User loggedInUser;
        private GuestRequestService guestRequestService;
        private AccommodationReservationService accommodationReservationService;
        private UserService userService;
        private AccommodationService accommodationService;
        private AvailableDatesForReservationService availableDatesForReservationService;
        private RequestViewModel SelectedRequest;
        private ProcessRequestService processRequestService;
        private string message = "";
        public RequestsViewModel(User user)
        {
            processRequestService = new ProcessRequestService();
            availableDatesForReservationService = new AvailableDatesForReservationService();
            accommodationService = new AccommodationService();
            userService = new UserService();
            accommodationReservationService = new AccommodationReservationService();
            guestRequestService = new GuestRequestService();
            AcceptRequestCommand = new RelayParameterCommand(AcceptRequest);
            DenyRequestCommand = new RelayParameterCommand(DenyRequest);
            loggedInUser = user;
            Requests = new ObservableCollection<RequestViewModel>();

            guestRequestService.GetAllRequestsForOwner(user).ForEach(guestRequest =>
            {
                AccommodationReservation reservation = accommodationReservationService.GetById(guestRequest.ReservationId);
                User guest = userService.GetById(reservation.UserId);
                Accommodation accommodation = accommodationService.GetById(reservation.AccommodationId);
                message = availableDatesForReservationService.GenerateMessageForAvailableDates(guestRequest,accommodation);
                Requests.Add(new RequestViewModel(guestRequest.Id, guest.FullName, accommodation.Name, accommodation.Location, reservation.ReservedFrom, reservation.ReservedTo, guestRequest.NewReservedFrom, guestRequest.NewReservedTo, message));
            });
        }
        private void AcceptRequest(object parameter)
        {
            if (parameter != null)
            {
                SelectedRequest = parameter as RequestViewModel;
                if (SelectedRequest.Message == "")
                {
                    Requests.Remove(SelectedRequest);
                    processRequestService.AcceptRequest(guestRequestService.GetById(SelectedRequest.RequestId));
                }
                else
                {
                    AcceptRequestWindow acceptRequest = new AcceptRequestWindow(loggedInUser, SelectedRequest);
                    acceptRequest.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    acceptRequest.ShowDialog();
                }
            }
        }
        private void DenyRequest(object parameter)
        {
            if (parameter != null)
            {
                SelectedRequest = parameter as RequestViewModel;
                DenyRequest denyRequest = new DenyRequest(loggedInUser, SelectedRequest);
                denyRequest.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                denyRequest.ShowDialog();
            }
        }
    }
}
