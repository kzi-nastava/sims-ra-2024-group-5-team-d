using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string AvatarPath { get; set; }
        public UserViewModel(User user)
        {
            Id = user.Id;
            FullName = user.FullName;
            AvatarPath = user.AvatarPath;
        }
    }

}
