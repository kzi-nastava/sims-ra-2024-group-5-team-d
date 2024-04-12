using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Appl.UseCases
{
    public class TourGuestService
    {
        private ITourGuestRepository _repository;
        private ITourRealisationRepository tourRealisationRepository;
        private ITourReservationRepository tourReservationRepository;

        public TourGuestService()
        {
            _repository = Injector.CreateInstance<ITourGuestRepository>();
            tourRealisationRepository = Injector.CreateInstance<ITourRealisationRepository>();
            tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
        }
        public List<TourGuest>? GetTourGuestsOnTourRealisation(int tourRealisation)
        {
            List<TourGuest> guests = new List<TourGuest>();
            foreach (TourGuest guest in _repository.GetAllTourGuests())
            {
                TourReservation reservation = tourReservationRepository.GetTourReservationById(guest.TourReservationId);
                bool isOnRealisation = reservation.TourRealisationId == tourRealisation;
                bool isCheckedIn = guest.CheckPointId < 0;

                if (isOnRealisation && isCheckedIn)
                {
                    guests.Add(guest);
                }
            }
            return guests;
        }

        public int NextIdForGuest()
        {
            return _repository.NextIdForGuest();
        }
    }
}
