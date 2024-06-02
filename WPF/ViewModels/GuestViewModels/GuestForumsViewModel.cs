using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.Commands;
using BookingApp.WPF.Views.GuestWindows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels.GuestViewModels
{
    public class GuestForumsViewModel
    {
        public ICommand CreateForumCommand { get; set; }
        public ICommand CloseForumCommand { get; set; }
        public ICommand OpenMoreCommand { get; set; }
        public ObservableCollection<ForumViewModel> Forums { get; set; }
        public ObservableCollection<ForumViewModel> MyForums { get; set; }
        public User LoggedInUser { get; set; }       
        public InboxViewModel SelectedRequest { get; set; }
        public ForumViewModel SelectedForum { get; set; }

        private string comment;
        public string Comment
        {
            get => comment;
            set
            {
                if (value != comment)
                {
                    comment = value;
                }
            }
        }
        private string title;
        public string Title
        {
            get => title;
            set
            {
                if (value != title)
                {
                    title = value;
                }
            }
        }
        private int locationId = 10;
        public int LocationId
        {
            get => locationId;
            set
            {
                if (value != locationId)
                {
                    locationId = value;
                }
            }
        }
        private LocationService locationService;
        private NotificationsService notificationsService;
        private SuperForumService superForumService;
        private ForumService forumService;
        public GuestNotificationsService guestNotificationsService;
        NotifierService notifierService;
        public GuestForumsViewModel(User user)
        {
            InitializeServices();
            CreateForumCommand = new RelayCommand(CreateForum);
            CloseForumCommand = new RelayParameterCommand(CloseForum);
            OpenMoreCommand = new RelayParameterCommand(OpenComments);
            Forums = new ObservableCollection<ForumViewModel>();
            MyForums = new ObservableCollection<ForumViewModel>();
            LoggedInUser = user;
            notifierService = new NotifierService();

            forumService.GetAll().ForEach(forum =>
            {
                bool isForumCreatedByLoggedInUser = forumService.IsUserCreateForum(LoggedInUser, forum) && forum.IsActive();
                Forums.Add(new ForumViewModel(forum, isForumCreatedByLoggedInUser, superForumService.IsSuperForum(forum)));

            });
            forumService.GetAllByUser(LoggedInUser).ForEach(forum =>
            {
                bool isForumCreatedByLoggedInUser = forumService.IsUserCreateForum(LoggedInUser, forum) && forum.IsActive();

                MyForums.Add(new ForumViewModel(forum, isForumCreatedByLoggedInUser, superForumService.IsSuperForum(forum)));
            });

                          
        }

        private void InitializeServices()
        {
            notificationsService = new NotificationsService();
            forumService = new ForumService();
            locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            superForumService = new SuperForumService();
            guestNotificationsService = new GuestNotificationsService();
        }

        public void CreateForum()
        {
            Forum forum = new Forum(Title, Comment, locationService.GetById(LocationId), LoggedInUser.Id, DateTime.UtcNow, true);
            forum = forumService.Save(forum);
            notificationsService.CreateForumNotifications(forum);
            Forums.Add(new ForumViewModel(forum, true, superForumService.IsSuperForum(forum)));
            notifierService.ShowSuccess("You have successfully created a forum!");
        }
        public void CloseForum(Object param)
        {
            ForumViewModel forumViewModel = param as ForumViewModel;
            if (forumViewModel != null)
            {
                Forum forum = forumService.GetById(forumViewModel.ForumId);
                forum.Active = false;               
                forumService.Update(forum);

                for(int i=0; i<Forums.Count(); i++)
                {
                    if(Forums[i]==forumViewModel)
                    {
                        Forums[i].IsActive = false;
                        Forums[i].IsVisible = false;
                    }
                }
            }
        }
        private void OpenComments(object forum)
        {
            ForumViewModel forumViewModel = (ForumViewModel)forum;
            if (forumViewModel != null)
            {
                GuestWindow.contentControl.Content = new ForumCommentsUserControl(LoggedInUser, forumViewModel);
            }
        }

    }
}
