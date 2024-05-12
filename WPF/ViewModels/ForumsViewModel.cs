using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.Commands;
using BookingApp.WPF.Views.OwnerView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels
{
    public class ForumsViewModel
    {
        public ICommand ReadMoreCommand { get; set; }
        private User loggedInUser;
        public ObservableCollection<ForumViewModel> Forums { get; set; }

        private ForumService forumService;
        private SuperForumService superForumService;
        public ForumsViewModel(User user)
        {
            InitializeServices();
            loggedInUser = user;
            Forums = new ObservableCollection<ForumViewModel>();
            forumService.GetAll().ForEach(forum=>Forums.Add(new ForumViewModel(forum,false, superForumService.IsSuperForum(forum)))); 
            ReadMoreCommand = new RelayParameterCommand(ReadMore);
        }
        private void InitializeServices()
        {
            LocationService locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            superForumService = new SuperForumService();
            forumService = new ForumService(Injector.CreateInstance<IForumRepository>(),locationService);
        }
        private void ReadMore(object forum)
        {
            ForumViewModel forumViewModel = (ForumViewModel)forum;
            if (forumViewModel != null)
            {
               OwnerMainWindow.contentControl.Content = new ForumReadMoreUserControl(forumViewModel.ForumId,loggedInUser);
            }
        }
    }
}
