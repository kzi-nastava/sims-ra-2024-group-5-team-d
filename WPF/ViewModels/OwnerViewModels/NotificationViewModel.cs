using System;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    public class NotificationViewModel
    {
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public int NotificationId { get; set; }
        public bool IsUnread { get; set; }
        public string SenderName { get; set; }
        public NotificationViewModel()
        {
        }
        public NotificationViewModel(string message, DateTime date,int notificationId,bool isRead,string senderName)
        {
            IsUnread = !isRead;
            NotificationId = notificationId;
            Message = message;
            Date = date;
            SenderName = senderName;
        }
    }
}