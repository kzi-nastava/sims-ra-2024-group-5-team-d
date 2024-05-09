using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels.TourViewModels.TouristViewModels
{
    public class NotificationViewModel : INotifyPropertyChanged
    {
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public int NotificationId { get; set; }

        private bool isUnread;
        public bool IsUnread
        {
            get => isUnread;
            set
            {
                if (value != isUnread)
                {
                    isUnread = value;
                    OnPropertyChanged();
                }
            }
        }
        public string SenderName { get; set; }
        public string AvatarPath { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public NotificationViewModel(string message, Notification notification, User user)
        {
            IsUnread = !notification.IsRead;
            NotificationId = notification.Id;
            Message = message;
            Date = notification.DateCreated;
            SenderName = user.FullName;
            AvatarPath = user.AvatarPath;
        }

    }
}
