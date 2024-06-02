using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ToastNotifications.Core;

namespace BookingApp.WPF.ViewModels
{
    public class ForumViewModel :INotifyPropertyChanged
    {
        public int ForumId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Location Location { get; set; }
        public DateTime DateCreated { get; set; }
        private bool isVisible;
        public bool IsVisible
        {
            get => isVisible;
            set
            {
                if (value != isVisible)
                {

                    isVisible = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool IsSuperForum { get; set; }

        private bool isActive;
        public bool IsActive
        {
            get => isActive;
            set
            {
                if (value != isActive)
                {

                    isActive = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool IsClosed { get; set; }
        public string SuperForum { get; set; }
        private int numberOfComments;
        public int NumberOfComments {
            get => numberOfComments;
            set
            {
                if (value != numberOfComments)
                {

                    numberOfComments = value;
                    OnPropertyChanged();
                }
            }
        }



        public ForumViewModel(Forum forum, bool isVisible, bool isSuperForum)
        {
            ForumId = forum.Id;
            Title = forum.Title;
            Description = forum.Description;
            Location = forum.Location;
            DateCreated = forum.DateCreated;
            SuperForum = "../../../Resources/Images/OwnerImages/superForum.png";
            IsSuperForum = isSuperForum ;
            IsVisible = isVisible;
            IsActive = forum.Active;
        }
        public ForumViewModel(Forum forum,int numberOfComments) {        
            ForumId = forum.Id;
            Title = forum.Title;
            Description = forum.Description;
            Location = forum.Location;
            DateCreated = forum.DateCreated;
            NumberOfComments = numberOfComments;
            isActive = forum.Active;
            IsClosed = !forum.Active;
        }
        public ForumViewModel(Forum forum)
        {
            ForumId = forum.Id;
            Title = forum.Title;
            Description = forum.Description;
            Location = forum.Location;
            DateCreated = forum.DateCreated;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

  
    }

}
