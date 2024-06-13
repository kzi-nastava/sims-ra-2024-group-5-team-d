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
        private TourReservationService tourReservationService;
        private TourService tourService;
        private UserService userService;
        private TourRealisationService tourRealisationService;

        public TourGuestService()
        {
            _repository = Injector.CreateInstance<ITourGuestRepository>();
            tourReservationService = new TourReservationService();
            userService = new UserService();
            tourService = new TourService();
            tourRealisationService = new TourRealisationService();
        }
        public List<TourGuest>? GetTourGuestsOnTourRealisation(int tourRealisation)
        {
            List<TourGuest> guests = new List<TourGuest>();
            foreach (TourGuest guest in _repository.GetAllTourGuests())
            {
                TourReservation reservation = tourReservationService.GetById(guest.TourReservationId);
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
                TourReservation reservation = tourReservationService.GetById(guest.TourReservationId);
                //TourRealisation tourRealisation = tourRealisationRepository.GetTourRealisationById(reservation.TourRealisationId);
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
                if (guest.PersonalID == id && guest.TourReservationId != -1 && guest.TourReservationId == reservationId)
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
                if(guest.TourReservationId == tourReservationId && guest.PersonalID == userService.GetById(tourReservationService.GetById(tourReservationId).User.Id).PersonalId && guest.CheckPointId != -1)
                {
                    return true;
                }
            }

            return false;
        }

        public TourGuest GetById(int id)
        {
            foreach (TourGuest t in GetAllTourGuests())
            {
                if (t.Id == id)
                {
                    return t;
                }
            }
            return null;
        }

        public List<TourGuest> GetAllTourGuests()
        {
            return _repository.GetAllTourGuests();
        }
        public TourGuest Update(TourGuest guest)
        {
            return _repository.UpdateTourGuest(guest);
        }

        public bool IsUser(TourGuest guest)
        {
            bool isUser = false;
            userService.GetAll().ForEach(user =>
            {
                if (user.PersonalId == guest.PersonalID)
                    isUser = true;
            });
            return isUser;
        }

        public bool IsFifthTimeOnATour(TourGuest guest)
        {
            int tourPresenceCounter = 0;

            if(IsUser(guest))
            {
                tourPresenceCounter = GetAllTourGuests().Where(tourGuest => tourGuest.PersonalID == guest.PersonalID && tourGuest.WasPresent() && tourRealisationService.GetById(tourReservationService.GetById(tourGuest.TourReservationId).TourRealisationId).StartTime.Year == DateTime.UtcNow.Year).ToList().Count();
            }          

            return tourPresenceCounter == 5;
        }


    }
}
