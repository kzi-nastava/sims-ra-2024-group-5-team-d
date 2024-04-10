using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Appl.UseCases
{
    public class TourService
    {
        private ITourRepository _repository;
        private TourRealisationService tourRealisationService;
        private ITourRealisationRepository tourRealisationRepository; 

        public TourService()
        {
            tourRealisationRepository = Injector.CreateInstance<ITourRealisationRepository>();
            _repository = Injector.CreateInstance<ITourRepository>();
            tourRealisationService = new TourRealisationService();
        }
        //PROVERITI I OVO
        public void DeleteTour(Tour tour)
        {
            // Obrisi sve realizacije ture prvo
            foreach (TourRealisation tourRealisation in tourRealisationRepository.GetTourRealisationsByTourId(tour.Id))
            {
                tourRealisationRepository.DeleteTourRealisation(tourRealisation);
            }
            // Obrisi turu sada
            _repository.DeleteTour(tour);
        }
        //pitati gde ova
        public List<Tour> GetToursForToday()
        {
            List<Tour> toursToday = new List<Tour>();
            foreach (Tour t in _repository.GetAllTours())
            {
                foreach (TourRealisation tR in tourRealisationRepository.GetTourRealisationsByTourId(t.Id))
                {
                    if (tR.StartTime.Day == DateTime.Now.Day && !toursToday.Contains(t))
                    {
                        toursToday.Add(t);
                    }
                }
            }
            return toursToday;
        }

        public List<Tour> GetFinishedTours()
        {
            List<Tour> finishedTours = new List<Tour>();
            foreach (Tour t in _repository.GetAllTours())
            {
                foreach (TourRealisation tR in tourRealisationRepository.GetTourRealisationsByTourId(t.Id))
                {
                    if (tR.StartTime.DayOfYear < DateTime.Now.DayOfYear && !finishedTours.Contains(t))
                    {
                        finishedTours.Add(t);
                    }
                }
            }
            return finishedTours;
        }

    }
}
