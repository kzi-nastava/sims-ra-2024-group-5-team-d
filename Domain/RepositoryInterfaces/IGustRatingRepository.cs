using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IGuestRatingRepository
    {
        public List<GuestRating> GetAll();
        public GuestRating Save(GuestRating guestRating);
        public void Delete(GuestRating guestRating);
        public GuestRating Update(GuestRating guestRating);
        public List<GuestRating> GetByGuest(User guest);
    }
}
