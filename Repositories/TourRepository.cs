using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookingApp.Repositories
{
    public class TourRepository : ITourRepository
    {
        private const string FilePathTours = "../../../Resources/Data/tours.csv";
        private const string FilePathTourRealisations = "../../../Resources/Data/tourRealisations.csv";


        private readonly Serializer<Tour> _serializerTours;
        private readonly Serializer<TourRealisation> _serializerTourRealisations;

        private List<Tour> _tours;
        private List<TourRealisation> _tourRealisations;

        private readonly LocationRepository _locationRepository;

        public TourRepository()
        {
            _serializerTours = new Serializer<Tour>();
            _serializerTourRealisations = new Serializer<TourRealisation>();
            _locationRepository = new LocationRepository();
            _tours = _serializerTours.FromCSV(FilePathTours);
            // _tourRealisations = _serializerTourRealisations.FromCSV(FilePathTourRealisations);
        }

        public List<Tour> GetAllTours()
        {
            _tours = _serializerTours.FromCSV(FilePathTours);
            _tours.ForEach(tour => tour.Location = _locationRepository.GetById(tour.Location.Id));
            return _tours;
        }
        public List<TourRealisation> GetAllTourRealisations()
        {
            return _serializerTourRealisations.FromCSV(FilePathTourRealisations);
        }

        public Tour SaveTour(Tour tour)
        {
            tour.Id = NextIdForTour();
            _tours = _serializerTours.FromCSV(FilePathTours);
            _tours.Add(tour);
            _serializerTours.ToCSV(FilePathTours, _tours);
            return tour;
        }
        public TourRealisation SaveTourRealisation(TourRealisation tourRealisation)
        {
            tourRealisation.Id = NextIdForTourRealisation();

            _tourRealisations = _serializerTourRealisations.FromCSV(FilePathTourRealisations);
            _tourRealisations.Add(tourRealisation);
            _serializerTourRealisations.ToCSV(FilePathTourRealisations, _tourRealisations);
            return tourRealisation;
        }

        public int NextIdForTour()
        {
            _tours = _serializerTours.FromCSV(FilePathTours);
            if (_tours.Count < 1)
            {
                return 1;
            }
            return _tours.Max(c => c.Id) + 1;
        }
        public int NextIdForTourRealisation()
        {
            _tourRealisations = _serializerTourRealisations.FromCSV(FilePathTourRealisations);
            if (_tourRealisations.Count < 1)
            {
                return 1;
            }
            return _tourRealisations.Max(c => c.Id) + 1;
        }

        public void DeleteTour(Tour tour)
        {
            _tours = _serializerTours.FromCSV(FilePathTours);
            Tour founded = _tours.Find(c => c.Id == tour.Id);
            _tourRealisations = _serializerTourRealisations.FromCSV(FilePathTourRealisations);
            foreach (TourRealisation tourRealisation in _tourRealisations.FindAll(tourRealisation => tourRealisation.TourId == tour.Id))
            {
                _tourRealisations.Remove(tourRealisation);
            }
            _tours.Remove(founded);
            _serializerTours.ToCSV(FilePathTours, _tours);
            _serializerTourRealisations.ToCSV(FilePathTourRealisations, _tourRealisations);
        }

        public void DeleteTourRealisation(TourRealisation tourRealisation)
        {
            _tourRealisations = _serializerTourRealisations.FromCSV(FilePathTourRealisations);
            TourRealisation founded = _tourRealisations.Find(c => c.Id == tourRealisation.Id);
            _tourRealisations.Remove(founded);
            _serializerTourRealisations.ToCSV(FilePathTourRealisations, _tourRealisations);
        }

        public Tour UpdateTour(Tour tour)
        {
            _tours = _serializerTours.FromCSV(FilePathTours);
            Tour current = _tours.Find(c => c.Id == tour.Id);
            int index = _tours.IndexOf(current);
            _tours.Remove(current);
            _tours.Insert(index, tour);       // keep ascending order of ids in file 
            _serializerTours.ToCSV(FilePathTours, _tours);
            return tour;
        }
        public TourRealisation UpdateTourRealisation(TourRealisation tourRealisation)
        {
            _tourRealisations = _serializerTourRealisations.FromCSV(FilePathTourRealisations);
            TourRealisation current = _tourRealisations.Find(c => c.Id == tourRealisation.Id);
            int index = _tourRealisations.IndexOf(current);
            _tourRealisations.Remove(current);
            _tourRealisations.Insert(index, tourRealisation);       // keep ascending order of ids in file 
            _serializerTourRealisations.ToCSV(FilePathTourRealisations, _tourRealisations);
            return tourRealisation;
        }
        public List<Tour> GetByUserTours(User user)
        {
            _tours = _serializerTours.FromCSV(FilePathTours);
            _tours.ForEach(tour => tour.Location = _locationRepository.GetById(tour.Location.Id));
            return _tours.FindAll(c => c.User.Id == user.Id);
        }
        public List<TourRealisation> GetByUserTourRealisations(User user)
        {
            _tourRealisations = _serializerTourRealisations.FromCSV(FilePathTourRealisations);
            return _tourRealisations.FindAll(c => c.User.Id == user.Id);
        }
        public Location getLocationByLocationId(int locationId)
        {
            return _locationRepository.GetById(locationId);
        }

        public TourRealisation GetTourRealisationById(int tourRealsiationId)
        {
            _tourRealisations = _serializerTourRealisations.FromCSV(FilePathTourRealisations);
            foreach (TourRealisation tR in _tourRealisations)
            {
                if (tR.Id == tourRealsiationId)
                {
                    return tR;
                }
            }
            return null;
        }
        public List<TourRealisation> GetTourRealisationsByTourId(int tourId)
        {
            _tourRealisations = _serializerTourRealisations.FromCSV(FilePathTourRealisations);
            List<TourRealisation> tourRealisations = new List<TourRealisation>();
            foreach (TourRealisation tourRealisation in _tourRealisations)
            {
                if (tourRealisation.TourId == tourId)
                {
                    Debug.WriteLine(tourId);
                    tourRealisations.Add(tourRealisation);
                }
            }
            return tourRealisations;
        }

        public List<Tour> GetToursForToday()
        {
            List<Tour> toursToday = new List<Tour>();
            foreach (Tour t in _tours)
            {
                foreach (TourRealisation tR in GetTourRealisationsByTourId(t.Id))
                {
                    if (tR.StartTime.Day == DateTime.Now.Day && !toursToday.Contains(t))
                    {
                        toursToday.Add(t);
                    }
                }

            }
            return toursToday;
        }
        public List<TourRealisation> GetTourRealisationsForToday(int tourId)
        {
            List<TourRealisation> tourRealisationsToday = new List<TourRealisation>();

            foreach (TourRealisation tourRealisation in GetTourRealisationsByTourId(tourId))
            {
                if (tourRealisation.StartTime.Day == DateTime.Now.Day)
                {
                    tourRealisationsToday.Add(tourRealisation);
                }
            }

            return tourRealisationsToday;
        }
        public Tour GetTourById(int id)
        {
            _tours = _serializerTours.FromCSV(FilePathTours);
            foreach (Tour t in _tours)
            {
                if (t.Id == id)
                {
                    return t;
                }
            }
            return null;
        }

        public Tour GetTourByName(string name)
        {
            _tours = _serializerTours.FromCSV(FilePathTours);
            foreach (Tour t in _tours)
            {
                if (t.Name == name)
                {
                    return t;
                }
            }
            return null;
        }

    }
}
