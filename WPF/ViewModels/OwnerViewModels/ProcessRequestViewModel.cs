using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
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

        private GuestRequestService guestRequestService;
        private ProcessRequestService processRequestService;

        public string GuestName { get; set; }
        public string Comment { get; set; }
        private User loggedInUser;
        private GuestRequest guestRequest;
        private RequestViewModel guestRequestViewModel;
        public ProcessRequestViewModel(User user, RequestViewModel guestRequest)
        {
            InitializeServices();
            DenyRequestCommand = new RelayCommand(DenyRequest);
            AcceptRequestCommand = new RelayCommand(AcceptRequest);
            loggedInUser = user;
            this.guestRequest = guestRequestService.GetById(guestRequest.RequestId);
            GuestName = guestRequest.GuestName;
            guestRequestViewModel = guestRequest;
        }

        private void InitializeServices()
        {
            guestRequestService = new GuestRequestService(Injector.CreateInstance<IGuestRequestRepository>());
            processRequestService = new ProcessRequestService(guestRequestService);
           
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
