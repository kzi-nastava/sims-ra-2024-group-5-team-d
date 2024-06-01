using BookingApp.Domain.Models;
using BookingApp.WPF.Views.GuestWindows;
using BookingApp.WPF.Views.OwnerView;
using BookingApp.WPF.Views.TouristGuide;
using BookingApp.WPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.Views.TouristView;
using System.Diagnostics;

namespace BookingApp.Appl.UseCases
{
    public class SignInService
    {
        private readonly UserService userService;
        public SignInService()
        {
            userService = new UserService();
        }
        public string CkeckCredentials(string username,string password)
        {
            User user = userService.GetByUsername(username);
            if (user != null && user.HasJob)
            {

                if (user.Password == password)
                {

                    MacLogin(user);
                    return "Success";
                }
                else
                {
                   return "Wrong password!";
                }
            }
            else
            {
                return "Wrong username!";
            }
        }

        internal void MacLogin(User user, bool isFirstLogIn=false)
        {
            if (user.Type.ToString().Equals("Guest"))
            {
                if (isFirstLogIn)
                {
                    WizzardWindow wizzardWindow = new WizzardWindow();
                    wizzardWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    wizzardWindow.Show();
                    GuestWindow guestWindow = new GuestWindow(user);
                    guestWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    guestWindow.Show();

                }
                else
                {
                    GuestWindow guestWindow = new GuestWindow(user);
                    guestWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    guestWindow.ShowDialog();
                }
            }
            else if (user.Type.ToString().Equals("Owner"))
            {
                OwnerMainWindow ownerWindow = new OwnerMainWindow(user);
                ownerWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                ownerWindow.ShowDialog();
            }
            else if (user.Type.ToString().Equals("Tourist"))
            {
                TouristHomeWindow touristWindow = new TouristHomeWindow(user);
                touristWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                touristWindow.ShowDialog();
            }
            else
            {
                if (user.HasJob)
                {
                    SideBar touristGuideWindow = new SideBar(user);
                    touristGuideWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    touristGuideWindow.ShowDialog();
                }
            }
        }
    }
}
