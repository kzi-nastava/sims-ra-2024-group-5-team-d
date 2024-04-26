using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels
{
    public class ForumsViewModel
    {
        private User loggedInUser;
        public ObservableCollection<ForumViewModel> Forums { get; set; }
        private ForumService forumService;
        public ForumsViewModel(User user)
        {
            loggedInUser = user;
            Forums = new ObservableCollection<ForumViewModel>();
            forumService = new ForumService();
            forumService.GetAll().ForEach(forum=>Forums.Add(new ForumViewModel(forum)));
        }
    }
}
