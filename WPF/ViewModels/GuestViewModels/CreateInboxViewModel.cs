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
        public ICommand CreateForumCommand { get; set; }
        public ICommand CloseForumCommand {  get; set; }
        public ICommand OpenMoreCommand { get; set; }
        public ObservableCollection<InboxViewModel> ApprovedRequests { get; set; }
        public ObservableCollection<InboxViewModel> InProcessRequests { get; set; }
        public ObservableCollection<InboxViewModel> RejectedRequests { get; set; }
        public ObservableCollection<ForumViewModel> Forums { get; set; }

        public int NumberOfApprovedNotifications { get; set; }
        public int NumberOfRejectedNotifications { get; set; }
        public int NumberOfNotifications { get; set; }
        public User LoggedInUser { get; set; }
        private GuestInboxService guestInboxService { get; set; }
        private AccommodationReservationService accommodationReservationService { get; set; }
        private AccommodationService accommodationService { get; set; }
        private ForumService forumService;
        public InboxViewModel SelectedRequest { get; set; }
        public ForumViewModel SelectedForum { get; set; }
        public GuestNotificationsService guestNotificationsService { get; set; }

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
        private LocationService locationService;
        public CreateInboxViewModel(User user)
        {
            CreateForumCommand = new RelayCommand(CreateForum);
            CloseForumCommand = new RelayParameterCommand(CloseForum);
            OpenMoreCommand = new RelayParameterCommand(OpenComments);
            ApprovedRequests = new ObservableCollection<InboxViewModel>();
            InProcessRequests = new ObservableCollection<InboxViewModel>();
            RejectedRequests = new ObservableCollection<InboxViewModel>();
            Forums = new ObservableCollection<ForumViewModel>();
            LoggedInUser = user;
            guestInboxService = new GuestInboxService();
            accommodationReservationService = new AccommodationReservationService();
            accommodationService = new AccommodationService();
            forumService = new ForumService();
            locationService = new LocationService();

            guestNotificationsService = new GuestNotificationsService();

            NumberOfApprovedNotifications = guestNotificationsService.GetNumerOfApprovedRequest(LoggedInUser);
            NumberOfRejectedNotifications = guestNotificationsService.GetNumerOfRejectedRequest(LoggedInUser);


            guestInboxService.GetApprovedRequests(LoggedInUser)
                            .ForEach(r => ApprovedRequests.Add(new InboxViewModel(accommodationService.GetById(accommodationReservationService.GetById(r.ReservationId).AccommodationId).ImagesPath, r.NewReservedFrom, r.NewReservedTo, r.Comment, accommodationService.GetAccommodationNameById(accommodationReservationService.GetById(r.ReservationId).AccommodationId))));

            guestInboxService.GetRejectedRequests(LoggedInUser)
                            .ForEach(r => RejectedRequests.Add(new InboxViewModel(accommodationService.GetById(accommodationReservationService.GetById(r.ReservationId).AccommodationId).ImagesPath, r.NewReservedFrom, r.NewReservedTo, r.Comment, accommodationService.GetAccommodationNameById(accommodationReservationService.GetById(r.ReservationId).AccommodationId))));
            
            guestInboxService.GetInProcessRequests(LoggedInUser)
                            .ForEach(r => InProcessRequests.Add(new InboxViewModel(accommodationService.GetById(accommodationReservationService.GetById(r.ReservationId).AccommodationId).ImagesPath, r.NewReservedFrom, r.NewReservedTo, r.Comment, accommodationService.GetAccommodationNameById(accommodationReservationService.GetById(r.ReservationId).AccommodationId))));

            forumService.GetAll().ForEach(forum => 
            {
                bool isUserCreateForum = forumService.IsUserCreateForum(LoggedInUser, forum);
                Forums.Add(new ForumViewModel(forum, isUserCreateForum));
            
            });

        }
        public void CreateForum()
        {
            Forum forum = new Forum(Title, Comment, locationService.GetById(LocationId), LoggedInUser.Id, DateTime.UtcNow, true);
            forumService.Save(forum);
        }
        public void CloseForum(Object param)
        {
            ForumViewModel forumViewModel = param as ForumViewModel;
            if(forumViewModel!=null )
            {
                
                Forum forum = forumService.GetById(forumViewModel.ForumId);
                forum.Active = false;
                forumService.Update(forum);

            }
        }
        private void OpenComments(object forum)
        {
            ForumViewModel forumViewModel = (ForumViewModel)forum;
            if (forumViewModel != null)
            {
                GuestWindow.contentControl.Content = new ForumCommentsUserControl(forumViewModel.ForumId, LoggedInUser);
            }
        }

    }
}
