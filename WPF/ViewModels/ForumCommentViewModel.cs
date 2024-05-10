using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels
{
    public class ForumCommentViewModel:INotifyPropertyChanged
    {
        public int ForumCommentId { get; set; }
        public string Comment { get; set; }
        public string CreatorFullName { get; set; }
        public string AvatarPath { get; set; }
        public string IconPath { get; set; }
        private int reports { get; set; }
        public int Reports
        {
            get => reports;
            set
            {
                if (value != reports)
                {
                    reports = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool IsReportable { get; set; }
        private bool isReported;
        public bool IsReported
        {
            get => isReported;
            set
            {
                if (value != isReported)
                {
                    isReported = value;
                    OnPropertyChanged();
                }
            }
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ForumCommentViewModel()
        {

        }
        public ForumCommentViewModel(ForumComment comment,int numberOfReports,string iconPath, User creator, bool isReportable,bool isAlreadyReported)
        {
            ForumCommentId = comment.Id;
            Comment = comment.Comment;
            CreatorFullName = creator.FullName;
            AvatarPath = creator.AvatarPath;
            IconPath = iconPath;
            Reports = numberOfReports;
            IsReportable = isReportable;
            IsReported= !isAlreadyReported;
        }
        public ForumCommentViewModel(ForumComment comment, string iconPath, User creator)
        {
            ForumCommentId = comment.Id;
            Comment = comment.Comment;
            CreatorFullName = creator.FullName;
            AvatarPath = creator.AvatarPath;
            IconPath = iconPath;
            IsReportable = false;
            IsReported = false;
        }
    }
}
