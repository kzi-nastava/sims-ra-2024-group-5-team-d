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
        public NotificationsService()
        {
            notificationRepository = Injector.CreateInstance<INotificationRepository>();
        }
        public int GetNumberOfUnreadNotificationsForOwner(User owner)
        {
          return  GetUnreadNotificationsCountForOWner(owner).Count();
        }
        public List<Notification> GetUnreadNotificationsCountForOWner(User user)
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

    }
}
