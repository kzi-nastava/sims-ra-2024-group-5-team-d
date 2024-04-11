using BookingApp.Domain.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Models
{
    public class GuestOwnerNotifications : ISerializable
    {
        public int Id { get; set; }
        public int ReferenceId { get; set; }
        public bool IsUnratedGuestNotification { get; set; }
        public bool IsRequestNotification { get; set; }
        public bool IsCancelledReservationNotification { get; set; }
        public bool IsForumNotification { get; set; }
        public GuestOwnerNotifications()
        {

        }
        public GuestOwnerNotifications(int id,int referenceId,bool isUnratedGuestNotification = false,bool isRequestNotification = false,bool isCancelledReservationNotification=false,bool isForumNotification=false)
        {
            Id = id;
            ReferenceId = referenceId;
            IsUnratedGuestNotification = isUnratedGuestNotification;
            IsRequestNotification = isRequestNotification;
            IsCancelledReservationNotification = isCancelledReservationNotification;
            IsForumNotification = isForumNotification;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            ReferenceId = Convert.ToInt32(values[1]);
            IsUnratedGuestNotification = Convert.ToBoolean(values[2]);
            IsRequestNotification = Convert.ToBoolean(values[3]);
            IsCancelledReservationNotification = Convert.ToBoolean(values[4]);
            IsForumNotification = Convert.ToBoolean(values[5]);

        }

        public string[] ToCSV()
        {

            string[] csvValues = { Id.ToString(), ReferenceId.ToString(), IsUnratedGuestNotification.ToString(), IsRequestNotification.ToString(), IsCancelledReservationNotification.ToString(),IsForumNotification.ToString() };
            return csvValues;
        }
    }
}
