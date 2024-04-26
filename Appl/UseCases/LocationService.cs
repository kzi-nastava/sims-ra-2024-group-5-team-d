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
    public class LocationService
    {
        private ILocationRepository locationRepository;
        private AccommodationReservationService accommodationReservationService;
        private AccommodationService accommodationService;
        public LocationService()
        {
            accommodationService = new AccommodationService();
            accommodationReservationService = new AccommodationReservationService();
            locationRepository = Injector.CreateInstance<ILocationRepository>();
        }
        public List<KeyValuePair<Location, double>> GetMostPopularLocations()
        { 
            List<Location>allLocations=locationRepository.GetAll();
            List<KeyValuePair<Location, double>> mostPopularLocation = new List<KeyValuePair<Location, double>>();
            allLocations.ForEach(location => {
              double busyness=CalculateBusynessForLocation(location);
                mostPopularLocation.Add(new KeyValuePair<Location, double>(location, busyness));
            });
            mostPopularLocation.Sort((l1, l2) => l2.Value.CompareTo(l1.Value));
            mostPopularLocation.ForEach(location => Debug.WriteLine(location.Key + " " + location.Value));
            return mostPopularLocation;
        }
        private double CalculateBusynessForLocation(Location location)
        {
            List<Accommodation>accommodationsOnsameLocation=accommodationService.GetAllAccommodationOnSameLocation(location);
            List<AccommodationReservation>reservations=new List<AccommodationReservation>();
            accommodationsOnsameLocation.ForEach(accommodation =>
            {
                reservations.AddRange(accommodationReservationService
                    .GetAllReservationsForAccommodation(accommodation.Id)
                    .Where(reservation => reservation.IsFinished())
                    .ToList());
            });
            double occupancy = reservations.Sum(reservation => (double)reservation.NumberOfPeople / accommodationService.GetById(reservation.AccommodationId).Capacity);
            return occupancy;
        }   
        public List<Location> GetAll()
        {
            return locationRepository.GetAll();
        }
        public Location GetById(int Id)
        {
            return locationRepository.GetById(Id);
        }


    }
}
