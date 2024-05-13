using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface INotificationRepository
    {
        List<Notification> GetAll();
        Notification GetById(int id);
        Notification Update(Notification notification);
        void Delete(Notification notification);
        Notification Save(Notification notification);
        List<Notification> GetByReceiverId(int receiverId);
    }
}
