using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    class ProfileViewModel
    { 
        public string AvatarPath { get; set; }
        public string FullName { get; set; }
        public double AverageRating { get; set; }
        public string UserType { get; set; }
        public string NumberOfReservations { get; set; }
        public ProfileViewModel(User user,double averageRating,int numberOfReservations)
        {
            AvatarPath = user.AvatarPath;
            FullName = user.FullName;
            AverageRating = averageRating;
            if(user.IsSuper())
                UserType = "SUPER OWNER";
            else
                UserType = "OWNER";
            if(numberOfReservations>1 || numberOfReservations==0)
                NumberOfReservations= numberOfReservations.ToString()+" reservations";
            else
                NumberOfReservations = numberOfReservations.ToString() + " reservation";

        }
    }
}
