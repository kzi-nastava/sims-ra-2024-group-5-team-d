using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Domain.Serializer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private const string FilePath = "../../../Resources/Data/notifications.csv";

        private readonly Serializer<Notification> _serializer;
        private List<Notification> _notifications;
        public NotificationRepository()
        {
            _serializer = new Serializer<Notification>();
            _notifications = _serializer.FromCSV(FilePath);
        }
        public List<Notification> GetAll()
        {
            _notifications = _serializer.FromCSV(FilePath);
            return _notifications;
        }
        public Notification GetById(int id)
        {
            _notifications = _serializer.FromCSV(FilePath);
            return _notifications.Find(notification => notification.Id == id);
        }
        public Notification Update(Notification notification)
        {
            _notifications = _serializer.FromCSV(FilePath);
            Notification current = _notifications.Find(a => a.Id == notification.Id);
            int index = _notifications.IndexOf(current);
            _notifications.Remove(current);
            _notifications.Insert(index, notification);
            _serializer.ToCSV(FilePath, _notifications);
            return notification;
        }
        public void Delete(Notification notification)
        {
            _notifications = _serializer.FromCSV(FilePath);
            Notification founded = _notifications.Find(notf => notf.Id == notification.Id);
            _notifications.Remove(founded);
            _serializer.ToCSV(FilePath, _notifications);
        }
        public Notification Save(Notification notification)
        {
            notification.Id = NextId();
            _notifications = _serializer.FromCSV(FilePath);
            _notifications.Add(notification);
            _serializer.ToCSV(FilePath, _notifications);
            return notification;
        }
        private int NextId()
        {
            _notifications = _serializer.FromCSV(FilePath);
            if (_notifications.Count < 1)
            {
                return 1;
            }
            return _notifications.Max(c => c.Id) + 1;
        }
        public List<Notification>GetByReceiverId(int receiverId)
        {
            _notifications = _serializer.FromCSV(FilePath);
            return _notifications.FindAll(notification => notification.ReceiverId == receiverId);
        }
    }
}
