using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repositories
{
    public class TourReservationRepository : ITourReservationRepository
    {
        private const string FilePathTourReservations = "../../../Resources/Data/tourReservations.csv";
        private readonly Serializer<TourReservation> _serializerTourReservations;
        private List<TourReservation> _tourReservations;

        public TourReservationRepository()
        {
            _serializerTourReservations = new Serializer<TourReservation>();
            _tourReservations = _serializerTourReservations.FromCSV(FilePathTourReservations);
        }
        public List<TourReservation> GetAllTourReservations()
        {
            _tourReservations = _serializerTourReservations.FromCSV(FilePathTourReservations);
            return _tourReservations;
        }
        public TourReservation GetTourReservationById(int id)
        {
            _tourReservations = _serializerTourReservations.FromCSV(FilePathTourReservations);
            foreach (TourReservation reservation in _tourReservations)
            {
                if (reservation.Id == id)
                {
                    return reservation;
                }
            }
            return null;
        }
        public void SaveReservation(TourReservation reservation)
        {
            reservation.Id = NextIdForReservation();
            _tourReservations = _serializerTourReservations.FromCSV(FilePathTourReservations);
            _tourReservations.Add(reservation);
            _serializerTourReservations.ToCSV(FilePathTourReservations, _tourReservations);
        }

        public int NextIdForReservation()
        {
            _tourReservations = _serializerTourReservations.FromCSV(FilePathTourReservations);
            if (_tourReservations.Count < 1)
            {
                return 0;
            }
            return _tourReservations.Max(c => c.Id) + 1;
        }
    }
}
