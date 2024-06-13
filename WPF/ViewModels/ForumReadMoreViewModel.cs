using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
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


        private ForumService forumService;
        private AccommodationService accommodationService;
        private ForumCommentService forumCommentService;
        private AccommodationReservationService accommodationReservationService;
        private UserService userService;
        private CommentReportService commentReportService;


        public ForumViewModel Forum { get; set; }
       
        public ObservableCollection<ForumCommentViewModel> Comments { get; set; }
        private User loggedInUser;
        private int forumId;
        public bool IsAddingCommentEnabled { get; set; }
        public AddForumCommentViewModel AddForumCommentViewModel { get; set; }
        private string icon="";
        public ForumReadMoreViewModel(int forumId, User user)
        {
            InitializeServices();

            this.forumId = forumId;
            ForumId = forumId;
            loggedInUser = user;

            AddForumCommentViewModel = new AddForumCommentViewModel(user.AvatarPath);
            Forum= new ForumViewModel(forumService.GetById(forumId),forumCommentService.GetNumberOfCommentsForForum(forumId));
            Comments = new ObservableCollection<ForumCommentViewModel>();
            forumCommentService.GetByForumId(forumId).ForEach(comment => {
                bool reportable=!accommodationReservationService.HasReservationOnLocation(userService.GetById(comment.CreatorId),Forum.Location) && !userService.GetById(comment.CreatorId).IsOwner();
                if (accommodationService.HasAccommodationOnLocation(userService.GetById(comment.CreatorId), forumService.GetById(comment.ForumId).Location))
                    icon = @"..\..\..\Resources\Images\OwnerImages\home.png";
                else if (accommodationReservationService.HasReservationOnLocation(userService.GetById(comment.CreatorId), forumService.GetById(comment.ForumId).Location))
                {
                    icon = @"..\..\..\Resources\Images\OwnerImages\verified.png";
                }
                else
                    icon = "";
                Comments.Add(new ForumCommentViewModel(comment,commentReportService.GetNumberOfReportsForComment(comment.Id), icon, userService.GetById(comment.CreatorId),reportable, commentReportService.IsAlreadyReported(comment.Id, user.Id) || !accommodationService.HasAccommodationOnLocation(user, Forum.Location)));
                });
            IsAddingCommentEnabled = accommodationService.HasAccommodationOnLocation(user,Forum.Location) && Forum.IsActive;
            CommentCommand = new RelayCommand(SaveComment);
            AddCommentCommand = new RelayCommand(AddComment);
            ReportCommand = new RelayParameterCommand(Report);
        }
        private void InitializeServices()
        {
            LocationService locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            forumService = new ForumService(Injector.CreateInstance<IForumRepository>(), locationService);    
            forumCommentService = new ForumCommentService(Injector.CreateInstance<IForumCommentRepository>());
            accommodationService = new AccommodationService();
            accommodationReservationService = new AccommodationReservationService(Injector.CreateInstance<IAccommodationReservationRepository>(),accommodationService);    
            userService = new UserService(Injector.CreateInstance<IUserRepository>());
            commentReportService = new CommentReportService(Injector.CreateInstance<ICommentReportRepository>());
        }
        public void SaveComment()
        {
                ForumComment comment = new ForumComment(forumId, loggedInUser.Id, AddForumCommentViewModel.Comment,DateTime.Now);
                comment=forumCommentService.Save(comment);
                Comments.Add(new ForumCommentViewModel(comment, @"..\..\..\Resources\Images\OwnerImages\home.png", loggedInUser));
                AddForumCommentViewModel.Comment = "";  
                AddForumCommentViewModel.IsVisible = false;
                Forum.NumberOfComments++;
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
