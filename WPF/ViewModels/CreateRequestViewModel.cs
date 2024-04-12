using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.WPF.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;

namespace BookingApp.WPF.ViewModels
{
    public class CreateRequestViewModel
    {
        public ICommand SendRequestCommand { get; private set; }
        public DateTime NewReservedFrom { get; set; }
        public DateTime NewReservedTo { get; set; }
        int reservationId { get; set; }
        private GuestRequestService guestRequestService; 

        public CreateRequestViewModel(User user, int reservationId)
        {
            SendRequestCommand = new RelayCommand(SendRequest);
            this.reservationId = reservationId;
            guestRequestService = new GuestRequestService();

        } 
        public void SendRequest()
        {
            GuestRequest guestRequest = new GuestRequest(reservationId, NewReservedFrom, NewReservedTo, "" , STATUS.INPROCESS);
            guestRequestService.Save(guestRequest);
        }
    }
}
