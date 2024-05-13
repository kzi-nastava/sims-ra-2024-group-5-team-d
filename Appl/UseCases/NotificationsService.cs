using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using ExCSS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using ToastNotifications.Core;

namespace BookingApp.Appl.UseCases
{
    public class NotificationsService
    {
        private INotificationRepository notificationRepository;

        private AccommodationReservationService accommodationReservationService;
        private AccommodationService accommodationService;
        private UserService userService;
        private TourRealisationService tourRealisationService;
        private TourService tourService;
        private TourRequestService tourRequestService;
        private TourReservationService tourReservationService;
        private TourGuestService tourGuestService;
        private ForumService forumService;
        private GuestRequestService guestRequestService;
        public NotificationsService()
        {
            tourRealisationService = new TourRealisationService();
            tourGuestService = new TourGuestService();
            tourReservationService = new TourReservationService();
            tourRequestService = new TourRequestService();
            tourService = new TourService();
            forumService = new ForumService();
            guestRequestService = new GuestRequestService();
            tourRealisationService = new TourRealisationService();
            userService = new UserService();
            accommodationService = new AccommodationService();
            accommodationReservationService = new AccommodationReservationService();
            notificationRepository = Injector.CreateInstance<INotificationRepository>();
        }
        public NotificationsService(INotificationRepository notificationRepository,UserService userService,AccommodationService accommodationService,AccommodationReservationService accommodationReservationService,GuestRequestService guestRequestService,ForumService forumService)
        {
            this.notificationRepository = notificationRepository;
            this.userService = userService;
            this.accommodationService = accommodationService;
            this.accommodationReservationService = accommodationReservationService;
            this.guestRequestService = guestRequestService;
            this.forumService = forumService;
        }
        public NotificationsService(INotificationRepository notificationRepository)
        {
            this.notificationRepository = notificationRepository;
        }
        public int GetNumberOfUnreadNotificationsForUser(User user)
        {
          return  GetUnreadNotificationsCountForUser(user).Count();
        }
        public void CreateNotificationForUnratedGuests(List<AccommodationReservation>reservations,User owner)
        {
            reservations.ForEach(reservation=> {
               Notification notification= GetRateNotificationByReservationId(reservation.Id);
                if (NotificationExists(notification))
                {
                    if (reservation.IsRateable())
                    {
                        if (notification.IsRead)
                        {
                            UnReadNotification(notification);
                        }
                    }
                    else
                    {
                        if(notification.IsRead==false)
                        {
                           ReadNotification(notification);
                        }
                    }

                }
                else
                {
                    notification = new Notification(owner.Id, reservation.Id, Domain.Models.Type.RATE, reservation.ReservedTo, false);
                    Save(notification);
                }
            });
        }
        private bool NotificationExists(Notification notification)
        {
            return notification != null;
        }
        public List<Notification> GetSortedNotificationsForUser(User user)
        {
            List<Notification> notificationsForUser = GetByReceiverId(user.Id);
            return SortNotifications(notificationsForUser);
  
        }
        public List<Notification> SortNotifications(List<Notification> notifications)
        {
            notifications.Sort((x, y) =>
            {
                int isReadComparison = x.IsRead.CompareTo(y.IsRead);
                if (isReadComparison != 0)
                {
                    return isReadComparison;
                }
                else
                {
                    return y.DateCreated.CompareTo(x.DateCreated);
                }
            });
            return notifications;
        }
        public List<Notification> GetUnreadNotificationsCountForUser(User user)
        {
            return GetNotificationsForUser(user).Where(notification => notification.IsRead == false).ToList();
        }
        public List<Notification> GetNotificationsForUser(User user)
        {
           return GetAll().Where(notification => notification.ReceiverId == user.Id).ToList();
        }
        public Notification GetRateNotificationByReservationId(int reservationId)
        {
            return GetAll().Where(notification => (notification.LinkId == reservationId && notification.IsRate())).FirstOrDefault();
        }
        public List<Notification> GetByReceiverId(int receiverId)
        {
            return notificationRepository.GetByReceiverId(receiverId);
        }
        public string GenerateMessage(Notification notification)
        {
            switch (notification.Type)
            {
                case Domain.Models.Type.RATE:
                    return "Rate your guest that stayed at " + accommodationService.GetById(accommodationReservationService.GetById(notification.LinkId).AccommodationId).Name;
                case Domain.Models.Type.REQUEST:
                    return "You have a new request click to see more";
                case Domain.Models.Type.FORUM:
                    return "New forum oppened on location where you have accommodation";
                case Domain.Models.Type.CANCEL:
                    return "Your guest has cancelled reservation at " + accommodationService.GetById(accommodationReservationService.GetById(notification.LinkId).AccommodationId).Name;
                case Domain.Models.Type.LIVETOUR:
                    foreach(User user in userService.GetAll())
                    {
                        if(user.PersonalId == tourGuestService.GetById(notification.LinkId).PersonalID)
                        {
                            return "You are currently on a live tour!";
                        }
                    }
                    return tourGuestService.GetById(notification.LinkId).FullName + " is currrently on a live tour with you! ";
                case Domain.Models.Type.TOURREQUEST:
                    return userService.GetFullNameById(tourRealisationService.GetById(tourReservationService.GetById(tourRequestService.GetById(notification.LinkId).TourReservationId).TourRealisationId).User.Id) + " has just accepted your request " + "for tour in " + tourService.GetById(tourRealisationService.GetById(tourReservationService.GetById(tourRequestService.GetById(notification.LinkId).TourReservationId).TourRealisationId).TourId).Location + " !";
                case Domain.Models.Type.NEWTOUR:
                    return "A new tour has been created by " + userService.GetFullNameById(tourService.GetById(notification.LinkId).User.Id) + " (Location: " + tourService.GetById(notification.LinkId).Location + ", " + "Language: " + tourService.GetById(notification.LinkId).Language + ")";
                case Domain.Models.Type.VOUCHER:
                    return "You have been gifted a new voucher!";
                default:
                    return "";
            }
        }
        public User GetSender(Notification notification)
        {
            switch (notification.Type)
            {
                case Domain.Models.Type.LIVETOUR:
                    return userService.GetById(tourRealisationService.GetById(tourReservationService.GetById(tourGuestService.GetById(notification.LinkId).TourReservationId).TourRealisationId).User.Id);
                case Domain.Models.Type.TOURREQUEST:
                    return userService.GetById(tourRealisationService.GetById(tourReservationService.GetById(tourRequestService.GetById(notification.LinkId).TourReservationId).TourRealisationId).User.Id);
                case Domain.Models.Type.NEWTOUR:
                    return userService.GetById(tourService.GetById(notification.LinkId).User.Id);
                case Domain.Models.Type.FORUM:
                    return userService.GetById(forumService.GetById(notification.LinkId).IdUser);
                case Domain.Models.Type.REQUEST:
                    return userService.GetById(accommodationReservationService.GetById(guestRequestService.GetById(notification.LinkId).ReservationId).UserId);
                case Domain.Models.Type.RATE:
                    return userService.GetById(accommodationReservationService.GetById(notification.LinkId).UserId);
                default:
                    return userService.GetById(accommodationReservationService.GetById(notification.LinkId).UserId);
            }

        }
        public void CreateNotification(int receiverId, int linkId, Domain.Models.Type type)
        {
            Notification notification = new Notification(receiverId, linkId, type, DateTime.Now, false);
            Save(notification);
        }


        public void CreateForumNotifications(Forum forum)
        {
            accommodationService.GetAll().ForEach(accommodation =>
            {
                if (accommodation.Location.Id == forum.Location.Id)
                {
                    Notification notification = GetForumNotificationByForumId(forum.Id,accommodation.Owner.Id);
                    if(!NotificationExists(notification))
                    CreateNotification(accommodation.Owner.Id, forum.Id,Domain.Models.Type.FORUM);
                }
            });
        }
        private Notification GetForumNotificationByForumId(int forumId,int ownerId)
        {
            return GetAll().Where(notification => (notification.LinkId == forumId && notification.isForum()&& notification.ReceiverId==ownerId)).FirstOrDefault();
        }
        public void RemoveNotification(int reservationId)
        {
            Notification notification = GetRateNotificationByReservationId(reservationId);
            ReadNotification(notification);
        }
        public void ReadNotification(Notification notification)
        {
            notification.IsRead = true;
            Update(notification);
        }
        public void UnReadNotification(Notification notification)
        {
            notification.IsRead = false;
            Update(notification);
        }
        public List<Notification>GetAll()
        {
            return notificationRepository.GetAll();
        }
        public Notification GetById(int Id)
        {
            return notificationRepository.GetById(Id);
        }
        public Notification Save(Notification notification)
        {
            return notificationRepository.Save(notification);
        }
        public void Delete(Notification notification)
        {
            notificationRepository.Delete(notification);
        }
        public Notification Update(Notification notification)
        {
            return notificationRepository.Update(notification);
        }

        public void MarkAllAsRead(int userId)
        {
            GetAll().ForEach(notification =>
            {
                if(notification.ReceiverId == userId)
                {
                    notification.IsRead = true;
                    Update(notification);
                }
            });
        }

        public void SendLiveTourNotification(int receiverId, int tourGuestId)
        {
            Save(new Notification(receiverId, tourGuestId, Domain.Models.Type.LIVETOUR, DateTime.UtcNow, false));
        }

        public void SendVoucherNotification(int receiverId, int voucherId)
        {
            Save(new Notification(receiverId, voucherId, Domain.Models.Type.VOUCHER, DateTime.UtcNow, false));
        }

        public void SendAcceptedRequestNotification(int receiverId, int requestId)
        {
            Save(new Notification(receiverId, requestId, Domain.Models.Type.TOURREQUEST, DateTime.UtcNow, false));
        }

        public void SendNotificationForWantedLocation(Tour tour)
        {
            foreach(User tourist in userService.GetAll())
            {
                if(tourist.Type == UserType.Tourist && tourRequestService.IsLocationRequestFulfilled(tour.Location.Id, tourist.Id))
                {
                    Save(new Notification(tourist.Id, tour.Id, Domain.Models.Type.NEWTOUR, DateTime.UtcNow, false));
                }
            }
        }

        public void SendNotificationForWantedLanguage(Tour tour)
        {
            foreach (User tourist in userService.GetAll())
            {
                if (tourist.Type == UserType.Tourist && tourRequestService.IsLangaugeRequestFulfilled(tour.Language, tourist.Id))
                {
                    Save(new Notification(tourist.Id, tour.Id, Domain.Models.Type.NEWTOUR, DateTime.UtcNow, false));
                }
            }
        }
    }
}
