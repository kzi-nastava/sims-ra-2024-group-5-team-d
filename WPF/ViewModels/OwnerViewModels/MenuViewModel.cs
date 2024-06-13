using BookingApp.Domain.Models;
using BookingApp.WPF.Commands;
using BookingApp.WPF.Views.OwnerView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    public class MenuViewModel
    {
        public ICommand RequestsCommand { get; set; }
        public ICommand RenovationsCommand { get; set; }
        public ICommand ReviewsCommand { get; set; }
        public ICommand ForumsCommand { get; set; }
        public ICommand SwitchViewCommand { get; set; }
        private User loggedInUser;
        public MenuViewModel(User user)
        {
            loggedInUser = user;
            RequestsCommand = new RelayCommand(Requests);
            RenovationsCommand = new RelayCommand(Renovations);
            ReviewsCommand = new RelayCommand(Reviews);
            ForumsCommand = new RelayCommand(Forums);
        }
        public MenuViewModel()
        {
        }

        public  void Requests()
        {
            OwnerMainWindow.contentControl.Content = new RequestsUserControl(loggedInUser);
        }
        public void Renovations()
        {
            OwnerMainWindow.contentControl.Content = new RenovationsUserControl(loggedInUser);
        }
        public void Reviews()
        {
            OwnerMainWindow.contentControl.Content = new OwnerReviewUserControl(loggedInUser);
        }
        public void Forums() {
            OwnerMainWindow.contentControl.Content = new ForumUserControl(loggedInUser);
        }
        
    }
}
