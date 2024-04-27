using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels
{
    public class ForumCommentViewModel
    {
        public int ForumCommentId { get; set; }
        public string Comment { get; set; }
        public string CreatorFullName { get; set; }
        public string AvatarPath { get; set; }
        public string IconPath { get; set; }
        public int Reports { get; set; }
        public ForumCommentViewModel()
        {

        }
        public ForumCommentViewModel(ForumComment comment,string iconPath, User creator)
        {
            ForumCommentId = comment.Id;
            Comment = comment.Comment;
            CreatorFullName = creator.FullName;
            AvatarPath = creator.AvatarPath;
            IconPath = iconPath;
            Reports = comment.NumberOfReports;
        }
    }
}
