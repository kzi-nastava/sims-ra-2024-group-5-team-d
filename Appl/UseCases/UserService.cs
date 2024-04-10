using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Appl.UseCases
{
    public class UserService
    {
        private IAccommodationRepository accommodationRepository;
        public UserService()
        {
            accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();

        }
        public bool IsUserSuperOwner(User user)
        {
            return accommodationRepository.GetByUser(user).Any(accommodation => accommodation.IsSuperOwner);
        }
    }
}
