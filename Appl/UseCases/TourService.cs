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
        private TourRatingService tourRatingService;
        private TourReservationService tourReservationService;

        public TourService()
        {
            _repository = Injector.CreateInstance<ITourRepository>();
            tourRealisationService = new TourRealisationService();
            tourRatingService = new TourRatingService();
            tourReservationService = new TourReservationService();
        }
        public void DeleteTour(Tour tour)
        {
            foreach (TourRealisation tourRealisation in tourRealisationService.GetTourRealisationsByTourId(tour.Id))
            {
                tourRealisationService.DeleteTourRealisation(tourRealisation);
            }
            _repository.DeleteTour(tour);
        }
        public List<Tour> GetToursForToday(User user)
        {
            var toursToday = new HashSet<Tour>();
            var today = DateTime.Today;
            foreach (var tour in _repository.GetAllTours(user))
            {
                if (tourRealisationService.GetTourRealisationsByTourId(tour.Id)
                    .Any(tR => tR.StartTime.Date == today))
                {
                    toursToday.Add(tour);
                }
            }
            return toursToday.ToList();
        }

        public List<Tour> GetFinishedTours(User user)
        {
            var finishedTours = new HashSet<Tour>();
            foreach (var tour in _repository.GetAllTours(user))
            {
                if (tourRealisationService.GetTourRealisationsByTourId(tour.Id)
                    .Any(tR => tR.IsFinished))
                {
                    finishedTours.Add(tour);
                }
            }
            return finishedTours.ToList();
        }

        public bool HasAvailableSeatsInAnyRealisation(int tourId)
        {
            return tourRealisationService.GetTourRealisationsByTourId(tourId).Any(tR => tR.AvailableSeats > 0 && tR.IsFinished == false && tR.IsLive == false);
        }
        public Tour GetBestTourOfAllTime()
        {
            double mostVisited = 0.00;
            int mostVisitedTourId = -1;

            foreach (Tour t in _repository.GetAllTours())
            {
                double totalAttendees = tourRealisationService.GetTourRealisationsByTourId(t.Id)
                    .Sum(tR => t.MaxCapacity - tR.AvailableSeats);

                if (totalAttendees > mostVisited)
                {
                    mostVisited = totalAttendees;
                    mostVisitedTourId = t.Id;
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
                double totalAttendees = tourRealisationService.GetTourRealisationsByTourId(t.Id)
                    .Where(tR => tR.StartTime.Year == year)
                    .Sum(tR => t.MaxCapacity - tR.AvailableSeats);

                if (totalAttendees > mostVisited)
                {
                    mostVisited = totalAttendees;
                    mostVisitedTourId = t.Id;
                }
            }

            return _repository.GetTourById(mostVisitedTourId);
        }

        public List<Tour> GetAllTours(User user)
        {
            return _repository.GetAllTours(user);
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
        public Tour Save(Tour tour)
        {
            return _repository.SaveTour(tour);
        }
        public int NextId()
        {
            return _repository.NextIdForTour();
        }
        public bool AmIAvailable(User user, DateTime start, double duration)
        {
            foreach (var tourRealisation in tourRealisationService.GetAllTourRealisations(user))
            {
                var tour = GetById(tourRealisation.TourId);
                bool IsBefore = start.AddHours(duration) < tourRealisation.StartTime;
                bool IsAfter = start > tourRealisation.StartTime.AddHours(tour.Duration);
                if (!(IsBefore || IsAfter))
                {
                    return false;
                }
            }
            return true;
        }

        public int GetNumberOfVotes(int tourId)
        {
            int numberOfVotes = 0;

            tourRatingService.GetAllTourRatings().ForEach(rating =>
            {
                if(tourId == tourRealisationService.GetById(tourReservationService.GetById(rating.TourReservationId).TourRealisationId).TourId)
                {
                    numberOfVotes++;
                }
            });

            return numberOfVotes;
        }

        public double GetRating(int tourId)
        {
            double sumOfRating = 0.0;

            tourRatingService.GetAllTourRatings().ForEach(rating =>
            {
                if (tourId == tourRealisationService.GetById(tourReservationService.GetById(rating.TourReservationId).TourRealisationId).TourId)
                {
                    sumOfRating += sumOfRating + (rating.TouristLanguage + rating.TouristKnowladge + rating.TourAmusement) / 3.0;
                }
            });
            return sumOfRating/GetNumberOfVotes(tourId);
        }
        public int NextIdForTour()
        {
            return _repository.NextIdForTour();
        }
    }
}
