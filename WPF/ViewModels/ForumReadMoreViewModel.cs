using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.WPF.Commands;
using BookingApp.WPF.ViewModels.OwnerViewModels;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels
{
    public class ForumReadMoreViewModel
    {
        public ICommand CommentCommand { get; set; }
        public ICommand AddCommentCommand { get; set; }
        public ICommand ReportCommand { get; set; }
        public int ForumId { get; set; }
        public ForumViewModel Forum { get; set; }
        private ForumService forumService;
        public ObservableCollection<ForumCommentViewModel> Comments { get; set; }
        private ForumCommentService forumCommentService;
        private User loggedInUser;
        private int forumId;
        private AccommodationService accommodationService;
        public bool IsAddingCommentEnabled { get; set; }
        public AddForumCommentViewModel AddForumCommentViewModel { get; set; }
        private AccommodationReservationService accommodationReservationService;
        private UserService userService;
        private CommentReportService commentReportService;
        private string icon="";
        public ForumReadMoreViewModel(int forumId, User user)
        {
            this.forumId = forumId;
            forumService = new ForumService();
            ForumId = forumId;
            loggedInUser = user;
            AddForumCommentViewModel = new AddForumCommentViewModel(user.AvatarPath);
            forumCommentService = new ForumCommentService();
            accommodationReservationService = new AccommodationReservationService();
            commentReportService = new CommentReportService();
            userService = new UserService();
            accommodationService = new AccommodationService();
            Forum= new ForumViewModel(forumService.GetById(forumId),false);
            Comments = new ObservableCollection<ForumCommentViewModel>();
            forumCommentService.GetByForumId(forumId).ForEach(comment => {
                bool reportable=!accommodationReservationService.HasReservationOnLocation(userService.GetById(comment.CreatorId),Forum.Location) && !userService.GetById(comment.CreatorId).IsOwner();
                if (accommodationService.HasAccommodationOnLocation(userService.GetById(comment.CreatorId), forumService.GetById(comment.ForumId).Location))
                    icon = @"C:\Users\lukai\Desktop\Resource\home.png";
                else if (accommodationReservationService.HasReservationOnLocation(userService.GetById(comment.CreatorId), forumService.GetById(comment.ForumId).Location))
                {
                    icon = @"C:\Users\lukai\Desktop\Resource\home.png";
                }
                else
                    icon = "";
                Comments.Add(new ForumCommentViewModel(comment,commentReportService.GetNumberOfReportsForComment(comment.Id), icon, userService.GetById(comment.CreatorId),reportable,commentReportService.IsAlreadyReported(comment.Id,user.Id)));
                });
            IsAddingCommentEnabled = accommodationService.HasAccommodationOnLocation(user,Forum.Location);
            CommentCommand = new RelayCommand(SaveComment);
            AddCommentCommand = new RelayCommand(AddComment);
            ReportCommand = new RelayParameterCommand(Report);
            
        }
        public void SaveComment()
        {
                ForumComment comment = new ForumComment(forumId, loggedInUser.Id, AddForumCommentViewModel.Comment,DateTime.Now);
                comment=forumCommentService.Save(comment);
                Comments.Add(new ForumCommentViewModel(comment, @"C:\Users\lukai\Desktop\Resource\home.png", loggedInUser));
                AddForumCommentViewModel.Comment = "";  
                AddForumCommentViewModel.IsVisible = false;
        }
        public void AddComment()
        {
            AddForumCommentViewModel.IsVisible = !AddForumCommentViewModel.IsVisible;
        }
        public void Report(object parameter)
        {
            ForumCommentViewModel commentViewModel = (ForumCommentViewModel)parameter;
            if (commentViewModel != null)
            {
                commentReportService.ReportComment(commentViewModel.ForumCommentId,loggedInUser.Id);
                for(int i=0;i<Comments.Count;i++)
                {
                      if (Comments[i].ForumCommentId == commentViewModel.ForumCommentId)
                      {
                            Comments[i].Reports++;
                            Comments[i].IsReported = false;
                            break;
                      }
                }
            }
        }
    }
}
