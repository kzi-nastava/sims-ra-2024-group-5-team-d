using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Domain.Serializer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.Repositories
{
    public class TourGuestRepository : ITourGuestRepository
    {
        private const string FilePathTourGuests = "../../../Resources/Data/tourGuests.csv";
        private const string FilePathTourReservations = "../../../Resources/Data/tourReservations.csv";


        private readonly Serializer<TourGuest> _serializerTourGuests;
        private readonly Serializer<TourReservation> _serializerTourReservations;

        private List<TourGuest> _tourGuests;
        private List<TourReservation> _tourReservations;

        public TourGuestRepository()
        {
            _serializerTourGuests = new Serializer<TourGuest>();
            _serializerTourReservations = new Serializer<TourReservation>();
            _tourGuests = _serializerTourGuests.FromCSV(FilePathTourGuests);
            _tourReservations = _serializerTourReservations.FromCSV(FilePathTourReservations);
        }

        public List<TourGuest> GetAllTourGuests()
        {
            _tourGuests = _serializerTourGuests.FromCSV(FilePathTourGuests);
            return _tourGuests;
        }


        //U NEKOM SPECIJALOM SERVICE KOJI KOMBINUJE 2 service repositorijuma
        public List<TourGuest>? GetTourGuestsOnTourRealisation(int tourRealisation)
        {
            _tourGuests = _serializerTourGuests.FromCSV(FilePathTourGuests);
            List<TourGuest> guests = new List<TourGuest>();
            foreach (TourGuest guest in _tourGuests)
            {
                TourReservation reservation = GetTourReservationById(guest.TourReservationId);
                bool isOnRealisation =  reservation.TourRealisationId == tourRealisation;
                bool isCheckedIn = guest.CheckPointId < 0;

                if (isOnRealisation && isCheckedIn)
                {
                    guests.Add(guest);
                }
            }
            return guests;
        }



        public TourGuest UpdateTourGuest(TourGuest tourGuest)
        {
            _tourGuests = _serializerTourGuests.FromCSV(FilePathTourGuests);
            TourGuest current = _tourGuests.Find(c => c.Id == tourGuest.Id);
            int index = _tourGuests.IndexOf(current);
            _tourGuests.Remove(current);
            _tourGuests.Insert(index, tourGuest);
            _serializerTourGuests.ToCSV(FilePathTourGuests, _tourGuests);
            return tourGuest;
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
        public void SaveGuest(TourGuest guest)
        {
            guest.Id = NextIdForGuest();
            _tourGuests = _serializerTourGuests.FromCSV(FilePathTourGuests);
            _tourGuests.Add(guest);
            _serializerTourGuests.ToCSV(FilePathTourGuests, _tourGuests);
        }

        public int NextIdForGuest()
        {
            _tourGuests = _serializerTourGuests.FromCSV(FilePathTourGuests);
            if (_tourGuests.Count < 1)
            {
                return 0;
            }
            return _tourGuests.Max(c => c.Id) + 1;
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
