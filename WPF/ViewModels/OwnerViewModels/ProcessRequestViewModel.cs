using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.WPF.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
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
        private RequestViewModel guestRequestViewModel;
        private ProcessRequestService processRequestService;
        public ProcessRequestViewModel(User user, RequestViewModel guestRequest)
        {
            processRequestService = new ProcessRequestService();
            DenyRequestCommand = new RelayCommand(DenyRequest);
            AcceptRequestCommand = new RelayCommand(AcceptRequest);
            loggedInUser = user;
            guestRequestService = new GuestRequestService();
            this.guestRequest = guestRequestService.GetById(guestRequest.RequestId);
            guestRequestViewModel = guestRequest;
        }
        private void DenyRequest()
        {
            RequestsViewModel.Requests.Remove(guestRequestViewModel);
            processRequestService.DenyRequest(guestRequest, Comment);
        }
        private void AcceptRequest()
        {
            RequestsViewModel.Requests.Remove(guestRequestViewModel);
            processRequestService.AcceptRequest(guestRequest);
        }
    }
}
