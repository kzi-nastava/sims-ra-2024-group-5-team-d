using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Domain.Serializer;
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


        private readonly Serializer<Tour> _serializerTours;

        private List<Tour> _tours;

        private readonly LocationRepository _locationRepository;

        public TourRepository()
        {
            _serializerTours = new Serializer<Tour>();
            _locationRepository = new LocationRepository();
            _tours = _serializerTours.FromCSV(FilePathTours);
        }

        public List<Tour> GetAllTours()
        {
            _tours = _serializerTours.FromCSV(FilePathTours);
            _tours.ForEach(tour => tour.Location = _locationRepository.GetById(tour.Location.Id));
            return _tours;
        }
        public List<Tour> GetAllTours(User user)
        {
            List<Tour> tours = _serializerTours.FromCSV(FilePathTours);
            foreach (Tour tour in tours)
            {
                if (tour.User.Id == user.Id)
                {
                    tour.Location = _locationRepository.GetById(tour.Location.Id);
                }
            }
            return tours;
        }

        public Tour SaveTour(Tour tour)
        {
            tour.Id = NextIdForTour();
            _tours = _serializerTours.FromCSV(FilePathTours);
            _tours.Add(tour);
            _serializerTours.ToCSV(FilePathTours, _tours);
            return tour;
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

        // KOMBINUJE 2 SERVICE CRUD??
        public void DeleteTour(Tour tour)
        {
            _tours = _serializerTours.FromCSV(FilePathTours);
            Tour founded = _tours.Find(c => c.Id == tour.Id);
            _tours.Remove(founded);
            _serializerTours.ToCSV(FilePathTours, _tours);
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

        public List<Tour> GetByUserTours(User user)
        {
            _tours = _serializerTours.FromCSV(FilePathTours);
            _tours.ForEach(tour => tour.Location = _locationRepository.GetById(tour.Location.Id));
            return _tours.FindAll(c => c.User.Id == user.Id);
        }

        public Location getLocationByLocationId(int locationId)
        {
            return _locationRepository.GetById(locationId);
        }



        // SERVICE KOJI KOMBINUJE 2 REPOSITORIJUMA


        // SPECIJALNI SEVICE 

        public Tour GetTourById(int id)
        {
            _tours = _serializerTours.FromCSV(FilePathTours);
            foreach (Tour t in _tours)
            {
                if (t.Id == id)
                {
                    t.Location = _locationRepository.GetById(t.Location.Id);
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
