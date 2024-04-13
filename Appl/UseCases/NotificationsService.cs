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
        public NotificationsService()
        {
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

    }
}
