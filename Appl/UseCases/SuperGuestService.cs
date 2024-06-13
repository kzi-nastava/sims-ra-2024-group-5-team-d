using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Appl.UseCases
{
    public class SuperGuestService
    {
        private AccommodationReservationService accommodationReservationService;
        private SuperUserService superUserService;
        private UserService userService;
        public SuperGuestService() { 
            accommodationReservationService = new AccommodationReservationService();
            superUserService = new SuperUserService();
            userService = new UserService();
        }
        public bool UpdateUserStatus(User user)
        {
            bool isSuperGuest = user.IsSuper();
            int numberOfReservationsInLastYear = accommodationReservationService.GetNumberOfReservationsLastYear(user);
            if (!isSuperGuest && numberOfReservationsInLastYear >= 10)
            {
                user.IsSuperUser = true;
                superUserService.Save(new SuperUser(user.Id, DateTime.UtcNow));
                userService.Update(user);
                return true;

            }
            else if (isSuperGuest && numberOfReservationsInLastYear < 10 && !superUserService.IsStillSuperUser(user))
            {
                user.IsSuperUser = false;
                userService.Update(user);
                return true;
            }
            return false;
        }
    }
}
