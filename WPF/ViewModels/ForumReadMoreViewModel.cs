using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.WPF.Commands;
using BookingApp.WPF.ViewModels.OwnerViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public int ForumId { get; set; }
        public ForumViewModel Forum { get; set; }
        private ForumService forumService;
        public ObservableCollection<ForumCommentViewModel> Comments { get; set; }
        private ForumCommentService forumCommentService;
        private User loggedInUser;
        private int forumId;
        public AddForumCommentViewModel AddForumCommentViewModel { get; set; }
        public ForumReadMoreViewModel(int forumId, User user)
        {
            this.forumId = forumId;
            forumService = new ForumService();
            ForumId = forumId;
            loggedInUser = user;
            AddForumCommentViewModel = new AddForumCommentViewModel(user.AvatarPath);
            forumCommentService = new ForumCommentService();
            Forum= new ForumViewModel(forumService.GetById(forumId));
            Comments = new ObservableCollection<ForumCommentViewModel>();
            forumCommentService.GetByForumId(forumId).ForEach(comment => {
                Comments.Add(new ForumCommentViewModel(comment,"Kuca", user));
                });
            CommentCommand = new RelayCommand(SaveComment);
            AddCommentCommand = new RelayCommand(AddComment);
            
        }
        public void SaveComment()
        {
                ForumComment comment = new ForumComment(forumId, loggedInUser.Id, AddForumCommentViewModel.Comment, 0,DateTime.Now);
                comment=forumCommentService.Save(comment);
                Comments.Add(new ForumCommentViewModel(comment, "Kuca", loggedInUser));
                AddForumCommentViewModel.Comment = "";  
                AddForumCommentViewModel.IsVisible = false;
        }
        public void AddComment()
        {
            AddForumCommentViewModel.IsVisible = !AddForumCommentViewModel.IsVisible;
        }
    }
}
