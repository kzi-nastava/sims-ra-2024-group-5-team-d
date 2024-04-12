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
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels
{
    public class RequestsViewModel
    {
        public ICommand AcceptRequestCommand { get; set; }
        public ICommand DenyRequestCommand { get; set; }
        public ObservableCollection<RequestViewModel> Requests { get; set; }

        private User loggedInUser;
        public RequestsViewModel(User user)
        {
            AcceptRequestCommand = new RelayCommand(AcceptRequest);
            DenyRequestCommand = new RelayCommand(DenyRequest);
            loggedInUser = user;
            Requests = new ObservableCollection<RequestViewModel>();
        }
        private void AcceptRequest()
        {
            Debug.WriteLine("Accept request");
            AcceptRequestWindow denyRequest = new AcceptRequestWindow(loggedInUser, 1);
            // Accept request
        }
        private void DenyRequest()
        {
            Debug.WriteLine("Accept request");
            DenyRequest denyRequest = new DenyRequest(loggedInUser,1);
            // Deny request
        }
    }
}
