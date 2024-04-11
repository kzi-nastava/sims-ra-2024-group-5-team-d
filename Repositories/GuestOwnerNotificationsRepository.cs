using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Domain.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repositories
{
    public class GuestOwnerNotificationsRepository:IGuestOwnerNotificationsRepository
    {
        private const string FilePath = "../../../Resources/Data/guestOwnerNotifications.csv";

        private readonly Serializer<GuestOwnerNotifications> _serializer;
        private List<GuestOwnerNotifications> _guestOwnerNotifications;

        public GuestOwnerNotificationsRepository()
        {
            _serializer = new Serializer<GuestOwnerNotifications>();
            _guestOwnerNotifications = _serializer.FromCSV(FilePath);
        }
        public List<GuestOwnerNotifications> GetAll()
        {
            _guestOwnerNotifications = _serializer.FromCSV(FilePath);
            return _guestOwnerNotifications;
        }
        public GuestOwnerNotifications GetByReferenceId(int referenceId)
        {
            _guestOwnerNotifications = _serializer.FromCSV(FilePath);
            return _guestOwnerNotifications.Find(notification => notification.ReferenceId == referenceId);
        }
        public GuestOwnerNotifications GetById(int id)
        {
            _guestOwnerNotifications = _serializer.FromCSV(FilePath);
            return _guestOwnerNotifications.Find(notification => notification.Id == id);
        }
    }
}
