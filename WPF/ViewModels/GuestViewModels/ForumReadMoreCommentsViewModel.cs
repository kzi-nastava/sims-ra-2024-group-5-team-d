using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.WPF.Commands;
using BookingApp.WPF.ViewModels.OwnerViewModels;
using BookingApp.WPF.Views.GuestWindows;
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
        public ICommand BackCommand { get; set; }
        public ForumViewModel Forum { get; set; }
        private ForumService forumService;
        private UserService userService;
        public ObservableCollection<ForumCommentViewModel> Comments { get; set; }
        private ForumCommentService forumCommentService;
        private User loggedInUser;
        public Location Location { get; set; }
        public string Description {  get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Comment { get; set; }
        public bool IsClosed { get; set; }
        public ForumReadMoreCommentsViewModel(User user, ForumViewModel selectedForum){
            Location = selectedForum.Location;
            Description = selectedForum.Description;
            Title = selectedForum.Title;
            Forum = selectedForum; 
            forumService = new ForumService();
            userService = new UserService();
            Author = userService.GetFullNameById(forumService.GetById(selectedForum.ForumId).IdUser);
            loggedInUser = user;
            forumCommentService = new ForumCommentService();
            Comments = new ObservableCollection<ForumCommentViewModel>();
            forumCommentService.GetByForumId(selectedForum.ForumId).ForEach(comment => {
                Comments.Add(new ForumCommentViewModel(comment, "Kuca", user));
            });

            AddCommentGuestCommand = new RelayCommand(AddCommentGuest);
            BackCommand = new RelayCommand(BackPage);

            IsClosed = forumService.GetById(selectedForum.ForumId).Active;
        }

        public void AddCommentGuest()
        {
            ForumComment comment = new ForumComment(Forum.ForumId, loggedInUser.Id, Comment, DateTime.Now);
            comment = forumCommentService.Save(comment);
            Comments.Add(new ForumCommentViewModel(comment, "Kuca", loggedInUser));
        }
        public void BackPage()
        {
                GuestWindow.contentControl.Content = new InboxAccommodationUserControl(loggedInUser);
        }
    }
}
