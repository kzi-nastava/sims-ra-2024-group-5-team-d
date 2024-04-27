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

namespace BookingApp.WPF.ViewModels.GuestViewModels
{
    public class ForumReadMoreCommentsViewModel
    {

        public ICommand AddCommentGuestCommand { get; set; }
        public int ForumId { get; set; }
        public ForumViewModel Forum { get; set; }
        private ForumService forumService;
        public ObservableCollection<ForumCommentViewModel> Comments { get; set; }
        private ForumCommentService forumCommentService;
        private User loggedInUser;
        private int forumId;
        public string Comment { get; set; }
        public ForumReadMoreCommentsViewModel(int forumId, User user)
        {
            this.forumId = forumId;
            forumService = new ForumService();
            ForumId = forumId;
            loggedInUser = user;
            forumCommentService = new ForumCommentService();
            Forum = new ForumViewModel(forumService.GetById(forumId));
            Comments = new ObservableCollection<ForumCommentViewModel>();
            forumCommentService.GetByForumId(forumId).ForEach(comment => {
                Comments.Add(new ForumCommentViewModel(comment, "Kuca", user));
            });

            AddCommentGuestCommand = new RelayCommand(AddCommentGuest);

        }

        public void AddCommentGuest()
        {
            ForumComment comment = new ForumComment(forumId, loggedInUser.Id, Comment, DateTime.Now);
            comment = forumCommentService.Save(comment);
            Comments.Add(new ForumCommentViewModel(comment, "Kuca", loggedInUser));
        }
    }
}
