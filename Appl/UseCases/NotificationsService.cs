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
                if (notification != null)
                {
                    if (reservation.IsRateable())
                    {
                        if (notification.IsRead == true)
                        {
                            notification.IsRead = false;
                            Update(notification);
                        }
                    }
                    else
                    {
                        if(notification.IsRead==false)
                        {
                            notification.IsRead = true;
                            Update(notification);
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
        public int GetNumberOfUnreadNotificationsForUser(User user)
        {
          return GetUnreadNotificationsCountForUser(user).Count();
        }
        public List<Notification> GetUnreadNotificationsCountForUser(User user)
        {
            return GetNotificationsForUser(user).Where(notification => notification.IsRead == false).ToList();
        }
        public List<Notification> GetNotificationsForUser(User user)
        {
           return GetAll().Where(notification => notification.ReceiverId == user.Id).ToList();
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
                    return "Rate your guest that stayed at " +accommodationRepository.GetById(accommodationReservationRepository.GetById(notification.LinkId).AccommodationId).Name;
                case Domain.Models.Type.REQUEST:
                    return "You have a new request click to see more";
                case Domain.Models.Type.FORUM:
                    return "New forum oppened on location where you have accommodation";
                case Domain.Models.Type.CANCEL:
                    return "Your guest has cancelled reservation at "+ accommodationRepository.GetById(accommodationReservationRepository.GetById(notification.LinkId).AccommodationId).Name;
                case Domain.Models.Type.LIVETOUR:
                    return "GAS";
                default:
                    return "";
            }
        }
        public User GetSender(Notification notification)
        {
            return userRepository.GetById(accommodationReservationRepository.GetById(notification.LinkId).UserId);
        }
        public void CreateNotification(int receiverId,int linkId, Domain.Models.Type type)
        {
            Notification notification = new Notification(receiverId,linkId, type, DateTime.Now, false);
            Save(notification);
        }
    }
}
