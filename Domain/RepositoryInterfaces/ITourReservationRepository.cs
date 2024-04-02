using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface ITourReservationRepository
    {
        public List<TourReservation> GetAllTourReservations();
        public TourReservation GetTourReservationById(int id);
        public void SaveReservation(TourReservation reservation);
    }
}
