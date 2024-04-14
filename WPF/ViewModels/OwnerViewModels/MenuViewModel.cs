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
        public ICommand SwitchViewCommand { get; set; }
        private User loggedInUser;
        public MenuViewModel(User user)
        {
            loggedInUser = user;
            SwitchViewCommand = new RelayParameterCommand(SwitchView);
        }
        public void SwitchView(object parameter)
        {
            if (parameter != null)
            {
                switch (Convert.ToInt32(parameter))
                {
                    case 0:
                        {
                            OwnerMainWindow.contentControl.Content = new RequestsUserControl(loggedInUser);
                            break;
                        }
                        case 1:
                        {
                            //OwnerMainWindow.contentControl.Content = new Renovations();
                            break;
                        }
                        case 2:
                        {
                            OwnerMainWindow.contentControl.Content = new OwnerReviewUserControl(loggedInUser);
                            break;
                        }
                        case 3:
                        {
                           // OwnerMainWindow.contentControl.Content = new ForumNoMenu();
                            break;
                        }
                } 
            }

        }
    }
}
