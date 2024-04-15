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
        public ICommand SelectionChangedCommand { get; set; }
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
            AcceptRequestCommand = new RelayCommand(AcceptRequest);
            DenyRequestCommand = new RelayCommand(DenyRequest);
            SelectionChangedCommand = new RelayParameterCommand(OnSelectionChanged);
            loggedInUser = user;
            Requests = new ObservableCollection<RequestViewModel>();

            guestRequestService.GetAllRequestsForOwner(user).ForEach(guestRequest =>
            {
                AccommodationReservation reservation = accommodationReservationService.GetById(guestRequest.ReservationId);
                User guest = userService.GetById(reservation.UserId);
                Accommodation accommodation = accommodationService.GetById(reservation.AccommodationId);
                List<KeyValuePair<DateTime, DateTime>> found = availableDatesForReservationService.CheckAvailableDatesInGivenRange(guestRequest.NewReservedFrom, guestRequest.NewReservedTo, (guestRequest.NewReservedTo - guestRequest.NewReservedFrom).Days, accommodation);
                if (found.Count == 0)
                    message = "No available dates for reservation";
                else
                    message = "";
                Requests.Add(new RequestViewModel(guestRequest.Id, guest.FullName, accommodation.Name, accommodation.Location, reservation.ReservedFrom, reservation.ReservedTo, guestRequest.NewReservedFrom, guestRequest.NewReservedTo, message));
            });
        }

        private void OnSelectionChanged(object parameter)
        {
            Debug.WriteLine("Selection changed");
            if (parameter != null)
            {
                SelectedRequest = parameter as RequestViewModel;
                Debug.WriteLine(SelectedRequest.RequestId);
            }
        }
        private void AcceptRequest()
        {
            if (SelectedRequest != null)
            {
                if (SelectedRequest.Message == "")
                {
                    Requests.Remove(SelectedRequest);
                    processRequestService.AcceptRequest(guestRequestService.GetById(SelectedRequest.RequestId));
                }
                else
                {
                    AcceptRequestWindow acceptRequest = new AcceptRequestWindow(loggedInUser, SelectedRequest);
                    //Window parentWindow = Window.GetWindow(this);
                    // acceptRequest.Owner = parentWindow;
                    acceptRequest.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    acceptRequest.ShowDialog();
                }
            }
        }
        private void DenyRequest()
        {
            if (SelectedRequest != null)
            {
                DenyRequest denyRequest = new DenyRequest(loggedInUser, SelectedRequest);
                //Window parentWindow = Window.GetWindow(this);
                //denyRequest.Owner = parentWindow;
                denyRequest.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                denyRequest.ShowDialog();
            }
        }
    }
}
