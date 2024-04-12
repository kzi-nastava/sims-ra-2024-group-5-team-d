using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Appl.UseCases
{
    public class GuestOwnerNotitificationsService
    {
        private IGuestOwnerNotificationsRepository guestOwnerNotificationsRepository;
        public GuestOwnerNotitificationsService()
        {
            guestOwnerNotificationsRepository = Injector.CreateInstance<IGuestOwnerNotificationsRepository>();
        }
        public List<GuestOwnerNotifications> GetAll()
        {
            return guestOwnerNotificationsRepository.GetAll();
        }
        public GuestOwnerNotifications GetByReferenceId(int referenceId)
        {
            return guestOwnerNotificationsRepository.GetByReferenceId(referenceId);
        }
        public GuestOwnerNotifications GetById(int id)
        {
            return guestOwnerNotificationsRepository.GetById(id);
        }
        public GuestOwnerNotifications Save(GuestOwnerNotifications guestOwnerNotifications)
        {
            return guestOwnerNotificationsRepository.Save(guestOwnerNotifications);
        }
    }
}
