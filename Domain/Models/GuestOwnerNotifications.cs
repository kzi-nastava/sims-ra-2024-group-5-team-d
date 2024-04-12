using BookingApp.Domain.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Models
{
    public enum Type
    {
        UnratedGuestNotification,
        RequestNotification,
        CancelledReservationNotification,
        ForumNotification
    }
    public class GuestOwnerNotifications : ISerializable
    {
        public int Id { get; set; }
        public User Recipient { get; set; }
        public int ReferenceId { get; set; }
        public Type NotificationType { get; set; }
        public bool IsRead { get; set; }
        public DateTime DateCreated { get; set; }
        public GuestOwnerNotifications()
        {

        }
        public GuestOwnerNotifications(int id,int referenceId,Type notificationType,DateTime dateCreated, User recipient)
        {
            Id = id;
            Recipient = recipient;
            NotificationType = notificationType;
            DateCreated = dateCreated;
            ReferenceId = referenceId;
            IsRead = false;
        }
        public GuestOwnerNotifications(int referenceId, Type notificationType, DateTime dateCreated,User recipient)
        {
            NotificationType = notificationType;
            Recipient = recipient;
            DateCreated = dateCreated;
            ReferenceId = referenceId;
            IsRead = false;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Recipient.Id = Convert.ToInt32(values[1]);
            ReferenceId = Convert.ToInt32(values[2]);
            NotificationType = (Type)Enum.Parse(typeof(Type), values[3]);
            IsRead = Convert.ToBoolean(values[4]);
            DateCreated = DateTime.ParseExact(values[5], "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);


        }

        public string[] ToCSV()
        {
            return new string[] { Id.ToString(), Recipient.Id.ToString(), ReferenceId.ToString(), NotificationType.ToString(), IsRead.ToString(), DateCreated.ToString() };
        }
        private bool IsRequest()
        {
            return NotificationType == Type.RequestNotification;
        }
        private bool IsForum()
        {
            return NotificationType == Type.ForumNotification;
        }
        private bool IsCancellation()
        {
            return NotificationType == Type.CancelledReservationNotification;
        }
        private bool IsUnratedGuest()
        {
            return NotificationType == Type.UnratedGuestNotification;
        }
    }
}
