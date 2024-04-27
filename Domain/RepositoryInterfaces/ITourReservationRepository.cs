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
        public TourReservation SaveReservation(TourReservation reservation);
        public int NextIdForReservation();
        public void DeleteReservation(TourReservation reservation);
        public TourReservation Update(TourReservation tourReservation);
    }
}
