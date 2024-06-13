using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface ITourGuestRepository
    {
        public List<TourGuest> GetAllTourGuests();
        public TourGuest UpdateTourGuest(TourGuest tourGuest);
        public void SaveGuest(TourGuest guest);
        public int NextIdForGuest();
    }
}
