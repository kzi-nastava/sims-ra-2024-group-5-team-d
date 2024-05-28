using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.Commands;
using BookingApp.WPF.Views.GuestWindows;
using BookingApp.WPF.Views.OwnerView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;

namespace BookingApp.WPF.ViewModels
{
    public class CreateInboxViewModel 
    {
        public ObservableCollection<InboxViewModel> ApprovedRequests { get; set; }
        public ObservableCollection<InboxViewModel> InProcessRequests { get; set; }
        public ObservableCollection<InboxViewModel> RejectedRequests { get; set; }
        public ObservableCollection<ForumViewModel> Forums { get; set; }

        public int NumberOfApprovedNotifications { get; set; }
        public int NumberOfRejectedNotifications { get; set; }
        public int NumberOfNotifications { get; set; }
        public User LoggedInUser { get; set; }
        public InboxViewModel SelectedApprovedRequest { get; set; }
        public InboxViewModel SelectedRejectedRequest { get; set; }

        private GuestInboxService guestInboxService;
        private AccommodationReservationService accommodationReservationService;
        private AccommodationService accommodationService;
        private GuestNotificationsService guestNotificationsService;

        private string comment;
        public string Comment
        {
            get => comment;
            set
            {
                if (value != comment)
                {
                    comment = value;
                }
            }
        }
        private string title;
        public string Title
        {
            get => title;
            set
            {
                if (value != title)
                {
                    title = value;
                }
            }
        }
        private int locationId = 10;
        public int LocationId
        {
            get => locationId;
            set
            {
                if (value != locationId)
                {
                    locationId = value;
                }
            }
        }
        public CreateInboxViewModel(User user)
        {
            InitializeServices();
            ApprovedRequests = new ObservableCollection<InboxViewModel>();
            InProcessRequests = new ObservableCollection<InboxViewModel>();
            RejectedRequests = new ObservableCollection<InboxViewModel>();
            Forums = new ObservableCollection<ForumViewModel>();
            LoggedInUser = user;

            NumberOfApprovedNotifications = guestNotificationsService.GetNumerOfApprovedRequest(LoggedInUser);
            NumberOfRejectedNotifications = guestNotificationsService.GetNumerOfRejectedRequest(LoggedInUser);


            guestInboxService.GetApprovedRequests(LoggedInUser)
                            .ForEach(r => ApprovedRequests.Add(new InboxViewModel(accommodationService.GetById(accommodationReservationService.GetById(r.ReservationId).AccommodationId).ImagesPath, r.NewReservedFrom, r.NewReservedTo, r.Comment, accommodationService.GetAccommodationNameById(accommodationReservationService.GetById(r.ReservationId).AccommodationId))));

            guestInboxService.GetRejectedRequests(LoggedInUser)
                            .ForEach(r => RejectedRequests.Add(new InboxViewModel(accommodationService.GetById(accommodationReservationService.GetById(r.ReservationId).AccommodationId).ImagesPath, r.NewReservedFrom, r.NewReservedTo, r.Comment, accommodationService.GetAccommodationNameById(accommodationReservationService.GetById(r.ReservationId).AccommodationId))));
            
            guestInboxService.GetInProcessRequests(LoggedInUser)
                            .ForEach(r => InProcessRequests.Add(new InboxViewModel(accommodationService.GetById(accommodationReservationService.GetById(r.ReservationId).AccommodationId).ImagesPath, r.NewReservedFrom, r.NewReservedTo, r.Comment, accommodationService.GetAccommodationNameById(accommodationReservationService.GetById(r.ReservationId).AccommodationId))));
        }

        private void InitializeServices()
        {
            guestInboxService = new GuestInboxService();
            accommodationReservationService = new AccommodationReservationService();
            accommodationService = new AccommodationService();
            guestNotificationsService = new GuestNotificationsService();
        }


    }
}
