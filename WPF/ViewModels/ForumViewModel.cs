using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels
{
    public class ForumViewModel
    {
        public int ForumId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Location Location { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsVisible {  get; set; }
        public bool IsSuperForum { get; set; }
        public string SuperForum { get; set; }
        public ForumViewModel(Forum forum, bool isVisible, bool isSuperForum)
        {
            ForumId = forum.Id;
            Title = forum.Title;
            Description = forum.Description;
            Location = forum.Location;
            DateCreated = forum.DateCreated;
            SuperForum = "../../../Resources/Images/OwnerImages/StarFull.png";
            IsSuperForum = isSuperForum;
            IsVisible = isVisible;
        }
    }

}
