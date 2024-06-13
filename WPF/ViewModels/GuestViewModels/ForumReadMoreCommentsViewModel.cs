using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.WPF.Commands;
using BookingApp.WPF.ViewModels.OwnerViewModels;
using BookingApp.WPF.Views.GuestWindows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        public ObservableCollection<ForumCommentViewModel> Comments { get; set; }

        private ForumService forumService;
        private UserService userService;
        private ForumCommentService forumCommentService;
        private AccommodationService accommodationService;
        private AccommodationReservationService accommodationReservationService;

        private User loggedInUser;
        public Location Location { get; set; }
        public string Description {  get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Comment { get; set; }
        public bool IsClosed { get; set; }
        private string Icon;
        public ForumReadMoreCommentsViewModel(User user, ForumViewModel selectedForum)
        {
            InitializeServices();
            forumService = new ForumService();
            userService = new UserService();
            forumCommentService = new ForumCommentService();
            accommodationService = new AccommodationService();
            accommodationReservationService = new AccommodationReservationService();

            Location = selectedForum.Location;
            Description = selectedForum.Description;
            Title = selectedForum.Title;
            Forum = selectedForum; 

            Author = userService.GetFullNameById(forumService.GetById(selectedForum.ForumId).IdUser);
            loggedInUser = user;
            Comments = new ObservableCollection<ForumCommentViewModel>();
            forumCommentService.GetByForumId(selectedForum.ForumId).ForEach(comment => {
                if (accommodationService.HasAccommodationOnLocation(userService.GetById(comment.CreatorId), forumService.GetById(comment.ForumId).Location))
                    Icon = "../../../Resources/Images/GuestImages/house.png"; //ako je owner koji ima smestaj
                else if (accommodationReservationService.HasReservationOnLocation(userService.GetById(comment.CreatorId), forumService.GetById(comment.ForumId).Location))
                {
;                    Icon = "../../../Resources/Images/GuestImages/star.png"; //za usera
                }
                else
                    Icon = "";
                Comments.Add(new ForumCommentViewModel(comment, Icon, userService.GetById(comment.CreatorId)));
            });

            AddCommentGuestCommand = new RelayCommand(AddCommentGuest);
            BackCommand = new RelayCommand(BackPage);

            IsClosed = forumService.GetById(selectedForum.ForumId).Active;
            
        }

        private void InitializeServices()
        {
            forumService = new ForumService();
            userService = new UserService();
            forumCommentService = new ForumCommentService();
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
