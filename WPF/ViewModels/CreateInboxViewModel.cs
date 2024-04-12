using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels
{
    public class CreateInboxViewModel 
    {
        public ObservableCollection<InboxViewModel> ApprovedRequests { get; set; }
        public ObservableCollection<InboxViewModel> InProcessRequests { get; set; }
        public ObservableCollection<InboxViewModel> RejectedRequests { get; set; }
        public User LoggedInUser { get; set; }
        private GuestInboxService guestInboxService { get; set; }
        private AccommodationReservationService accommodationReservationService { get; set; }
        private AccommodationService accommodationService { get; set; }
        public InboxViewModel SelectedRequest { get; set; }
        public CreateInboxViewModel(User user)
        {
            ApprovedRequests = new ObservableCollection<InboxViewModel>();
            InProcessRequests = new ObservableCollection<InboxViewModel>();
            RejectedRequests = new ObservableCollection<InboxViewModel>();
            LoggedInUser = user;
            guestInboxService = new GuestInboxService();
            accommodationReservationService = new AccommodationReservationService();
            accommodationService = new AccommodationService();

            guestInboxService.GetApprovedRequests(LoggedInUser)
                            .ForEach(r => ApprovedRequests.Add(new InboxViewModel(accommodationService.GetById(accommodationReservationService.GetById(r.ReservationId).AccommodationId).ImagesPath, r.NewReservedFrom, r.NewReservedTo, r.Comment, accommodationService.GetAccommodationNameById(accommodationReservationService.GetById(r.ReservationId).AccommodationId))));

            guestInboxService.GetRejectedRequests(LoggedInUser)
                            .ForEach(r => RejectedRequests.Add(new InboxViewModel(accommodationService.GetById(accommodationReservationService.GetById(r.ReservationId).AccommodationId).ImagesPath, r.NewReservedFrom, r.NewReservedTo, r.Comment, accommodationService.GetAccommodationNameById(accommodationReservationService.GetById(r.ReservationId).AccommodationId))));
            
            guestInboxService.GetInProcessRequests(LoggedInUser)
                            .ForEach(r => InProcessRequests.Add(new InboxViewModel(accommodationService.GetById(accommodationReservationService.GetById(r.ReservationId).AccommodationId).ImagesPath, r.NewReservedFrom, r.NewReservedTo, r.Comment, accommodationService.GetAccommodationNameById(accommodationReservationService.GetById(r.ReservationId).AccommodationId))));
        }
    }
}
