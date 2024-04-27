using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Domain.Serializer;
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
        public TourReservation SaveReservation(TourReservation reservation)
        {
            reservation.Id = NextIdForReservation();
            _tourReservations = _serializerTourReservations.FromCSV(FilePathTourReservations);
            _tourReservations.Add(reservation);
            _serializerTourReservations.ToCSV(FilePathTourReservations, _tourReservations);
            return reservation;
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
        public void DeleteReservation(TourReservation reservation)
        {
            _tourReservations = _serializerTourReservations.FromCSV(FilePathTourReservations);
            TourReservation founded = _tourReservations.Find(c => c.Id == reservation.Id);
            _tourReservations.Remove(founded);
            _serializerTourReservations.ToCSV(FilePathTourReservations, _tourReservations);
        }
        public TourReservation Update(TourReservation tourReservation)
        {
            _tourReservations = _serializerTourReservations.FromCSV(FilePathTourReservations);
            TourReservation current = _tourReservations.Find(c => c.Id == tourReservation.Id);
            int index = _tourReservations.IndexOf(current);
            _tourReservations.Remove(current);
            _tourReservations.Insert(index, tourReservation);       // keep ascending order of ids in file 
            _serializerTourReservations.ToCSV(FilePathTourReservations, _tourReservations);
            return tourReservation;
        }
    }
}
