using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private IUserRepository userRepository;

        private TourService tourService;

        public TourGuestService()
        {
            _repository = Injector.CreateInstance<ITourGuestRepository>();
            tourRealisationRepository = Injector.CreateInstance<ITourRealisationRepository>();
            tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
            userRepository = Injector.CreateInstance<IUserRepository>();
            tourService = new TourService();
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

        public void Save(TourGuest guest) 
        {
            _repository.SaveGuest(guest);
        }

        public int NextIdForGuest()
        {
            return _repository.NextIdForGuest();
        }
        public List<TourGuest> GetTourGuestsOnTour(int tourId)
        {
            List<TourGuest> guests = new List<TourGuest>();
            foreach (TourGuest guest in _repository.GetAllTourGuests())
            {
                TourReservation reservation = tourReservationRepository.GetTourReservationById(guest.TourReservationId);
                TourRealisation tourRealisation = tourRealisationRepository.GetTourRealisationById(reservation.TourRealisationId);
                Tour tour = tourService.FindTourForTourRealisation(reservation.TourRealisationId);
                if (tour != null && tour.Id == tourId)
                {
                    guests.Add(guest);
                }
            }
            return guests;
        }

        public TourGuest GetTourGuestByPersonalId(string id, int reservationId)
        {
            foreach (TourGuest guest in _repository.GetAllTourGuests())
            {
                if (guest.PersonalID == id && guest.TourReservationId == reservationId)
                {
                    return guest;
                }
            }
            return null;
        }

        public bool WasTouristOnTour(int tourReservationId)
        {
            foreach (TourGuest guest in _repository.GetAllTourGuests())
            {
                if(guest.TourReservationId == tourReservationId && guest.PersonalID == userRepository.GetById(tourReservationRepository.GetTourReservationById(tourReservationId).User.Id).PersonalId && guest.CheckPointId != -1)
                {
                    return true;
                }
            }

            return false;
        }

        public List<TourGuest> GetAllTourGuests()
        {
            return _repository.GetAllTourGuests();
        }
        public TourGuest Update(TourGuest guest)
        {
            return _repository.UpdateTourGuest(guest);
        }
    }
}
