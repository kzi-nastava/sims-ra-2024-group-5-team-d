using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToastNotifications.Core;

namespace BookingApp.Appl.UseCases
{
    public class NotificationsService
    {
        private INotificationRepository notificationRepository;
        private IAccommodationReservationRepository accommodationReservationRepository;
        private IAccommodationRepository accommodationRepository;
        private IUserRepository userRepository;
        public NotificationsService()
        {
            userRepository = Injector.CreateInstance<IUserRepository>();
            accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
            accommodationReservationRepository = Injector.CreateInstance<IAccommodationReservationRepository>();
            notificationRepository = Injector.CreateInstance<INotificationRepository>();
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
                    return "Rate your guest that stayed at " + accommodationRepository.GetById(accommodationReservationRepository.GetById(notification.LinkId).AccommodationId).Name;
                case Domain.Models.Type.REQUEST:
                    return "You have a new request click to see more";
                case Domain.Models.Type.FORUM:
                    return "New forum oppened on location where you have accommodation";
                case Domain.Models.Type.CANCEL:
                    return "Your guest has cancelled reservation at " + accommodationRepository.GetById(accommodationReservationRepository.GetById(notification.LinkId).AccommodationId).Name;
                case Domain.Models.Type.LIVETOUR:
                    return "NEKO" + " je dodat na turu";
                case Domain.Models.Type.TOURREQUEST:
                    return  "DOBAVI IME VODICA" + " has just accepted your request " + "(Location: DOBAVI LOKACIJU)";
                case Domain.Models.Type.NEWTOUR:
                    return "A new tour has been created on " + "NEKOM JEZIKU / NEKA LOKACIJA";
                default:
                    return "";
            }
        }
        public User GetSender(Notification notification)
        {
            return userRepository.GetById(accommodationReservationRepository.GetById(notification.LinkId).UserId);
        }
        public void CreateNotification(int receiverId, int linkId, Domain.Models.Type type)
        {
            Notification notification = new Notification(receiverId, linkId, type, DateTime.Now, false);
            Save(notification);
        }


        public void CreateForumNotifications(Forum forum)
        {
            accommodationRepository.GetAll().ForEach(accommodation =>
            {
                if (accommodation.Location.Id == forum.Location.Id)
                {
                    Notification notification = GetForumNotificationByForumId(forum.Id);
                    if(!NotificationExists(notification))
                    CreateNotification(accommodation.Owner.Id, forum.Id,Domain.Models.Type.FORUM);
                }
            });
        }
        private Notification GetForumNotificationByForumId(int forumId)
        {
            return GetAll().Where(notification => (notification.LinkId == forumId && notification.isForum())).FirstOrDefault();
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
    }
}
