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
    public class DenyRequestViewModel
    {
        public ICommand DenyRequestCommand { get; set; }
        public int Id { get; set; }
        public string GuestName { get; set; }
        public string Comment { get; set; }
        private User loggedInUser;
        public DenyRequestViewModel(User user, int requestId)
        {
            DenyRequestCommand = new RelayCommand(DenyRequest);
            Id = requestId;
            loggedInUser = user;
        }
        private void DenyRequest()
        {
            // Deny request
        }
    }
}
