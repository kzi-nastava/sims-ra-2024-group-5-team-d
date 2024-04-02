using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class ReservationRepository
    {

        private const string FilePath = "../../../Resources/Data/reservations.csv";

        private readonly Serializer<AccommodationReservation> _serializer;

        private List<AccommodationReservation> _reservations;

        public ReservationRepository()
        {
            _serializer = new Serializer<AccommodationReservation>();
            _reservations = _serializer.FromCSV(FilePath);
        }
        public List<AccommodationReservation> GetAll()
        {
            _reservations = _serializer.FromCSV(FilePath);
            return _reservations;
        }

        public AccommodationReservation Save(AccommodationReservation reservation)
        {
            reservation.Id = NextId();
            _reservations = _serializer.FromCSV(FilePath);
            _reservations.Add(reservation);
            _serializer.ToCSV(FilePath, _reservations);
            return reservation;
        }

        public int NextId()
        {
            _reservations = _serializer.FromCSV(FilePath);
            if (_reservations.Count < 1)
            {
                return 1;
            }
            return _reservations.Max(c => c.Id) + 1;
        }

        public void Delete(AccommodationReservation reservation)
        {
            _reservations = _serializer.FromCSV(FilePath);
            AccommodationReservation founded = _reservations.Find(r => r.Id == reservation.Id);
            _reservations.Remove(founded);
            _serializer.ToCSV(FilePath, _reservations);
        }

        public AccommodationReservation Update(AccommodationReservation reservation)
        {
            _reservations = _serializer.FromCSV(FilePath);
            AccommodationReservation current = _reservations.Find(r => r.Id == reservation.Id);
            int index = _reservations.IndexOf(current);
            _reservations.Remove(current);
            _reservations.Insert(index, reservation);       // keep ascending order of ids in file 
            _serializer.ToCSV(FilePath, _reservations);
            return reservation;
        }
        public List<AccommodationReservation> GetByUser(User user)
        {
            _reservations = _serializer.FromCSV(FilePath);
            return _reservations.FindAll(reservation => reservation.UserId == user.Id);
        }
        public List<AccommodationReservation> GetByAccommodation(Accommodation accommodation)
        {
            _reservations = _serializer.FromCSV(FilePath);
            return _reservations.FindAll(reservation => reservation.AccommodationId == accommodation.Id);
        }
        public List<AccommodationReservation> GetAllReservationsForUser(Accommodation accommodation,User user)
        {
            _reservations = _serializer.FromCSV(FilePath);
            return _reservations.FindAll(reservation =>  reservation.AccommodationId == accommodation.Id && reservation.UserId==user.Id );
        }


    }
}
