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
        public void DeleteTour(Tour tour)
        {
            foreach (TourRealisation tourRealisation in tourRealisationRepository.GetTourRealisationsByTourId(tour.Id))
            {
                tourRealisationRepository.DeleteTourRealisation(tourRealisation);
            }
            _repository.DeleteTour(tour);
        }
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

        public bool HasAvailableSeatsInAnyRealisation(int tourId)
        {
            return tourRealisationRepository.GetTourRealisationsByTourId(tourId).Any(tR => tR.AvailableSeats > 0);
        }
        public Tour GetBestTourOfAllTime()
        {
            double mostVisited = 0.00;
            int mostVisitedTourId = -1;

            foreach (Tour t in _repository.GetAllTours())
            {
                int totalAttendees = 0;
                int counter = 0;

                foreach (TourRealisation tR in tourRealisationRepository.GetTourRealisationsByTourId(t.Id))
                {
                    totalAttendees += t.MaxCapacity - tR.AvailableSeats;
                    counter++; 
                }

                if (counter > 0)
                {
                    if (totalAttendees > mostVisited)
                    {
                        mostVisited = totalAttendees;
                        mostVisitedTourId = t.Id;
                    }
                }
            }

            return _repository.GetTourById(mostVisitedTourId);
        }
        public Tour GetBestTourInAYear(int year)
        {
            double mostVisited = 0.00;
            int mostVisitedTourId = -1;

            foreach (Tour t in _repository.GetAllTours())
            {
                int totalAttendees = 0; 
                int counter = 0; 

                foreach (TourRealisation tR in tourRealisationRepository.GetTourRealisationsByTourId(t.Id).Where(tR => tR.StartTime.Year == year))
                {
                    totalAttendees += t.MaxCapacity - tR.AvailableSeats; 
                    counter++; 
                }

                if (counter > 0)
                {
                    if (totalAttendees > mostVisited)
                    {
                        mostVisited = totalAttendees;
                        mostVisitedTourId = t.Id;
                    }
                }
            }

            return _repository.GetTourById(mostVisitedTourId);
        }



        public List<Tour> GetAllTours()
        {
            return _repository.GetAllTours();
        }
        public Tour FindTourForTourRealisation(int tourRealisationTourId)
        {
            return _repository.GetAllTours().Find(t => t.Id == tourRealisationTourId);
        }
        public Tour GetById(int id)
        {
            return _repository.GetTourById(id);
        }

    }
}
