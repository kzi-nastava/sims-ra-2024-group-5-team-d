using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Appl.UseCases
{
    public class RateTourService
    {
        private TourRealisationService tourRealisationService;
        private TourReservationService tourReservationService;

        public RateTourService() 
        {
            tourRealisationService = new TourRealisationService();
            tourReservationService = new TourReservationService();
        }

        public List<TourRealisation> GetAllPastTourRealisationsForTourist(User tourist)
        {
            List<TourReservation> tourReservations = tourReservationService.GetTourReservationsForTourist(tourist);
            List<TourRealisation> finishedTourRealisation = new List<TourRealisation>();
            tourReservations.ForEach(tourReservation => {
                TourRealisation tourRealisation = tourRealisationService.GetTourRealisationById(tourReservation.TourRealisationId);
                if (tourRealisation.IsFinished)
                    finishedTourRealisation.Add(tourRealisation);
            });
            return finishedTourRealisation;
        }
    }
}
