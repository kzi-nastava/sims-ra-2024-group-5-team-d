using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IGuestOwnerNotificationsRepository
    {
        List<GuestOwnerNotifications> GetAll();
        GuestOwnerNotifications GetByReferenceId(int referenceId);
        GuestOwnerNotifications GetById(int id);
        GuestOwnerNotifications Save(GuestOwnerNotifications guestOwnerNotifications);
    }
}
