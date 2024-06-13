using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Appl.UseCases
{
    public class OwnerActionReccommenderService
    {
        private LocationService locationService;
        private AccommodationService accommodationService;
        private AccommodationReservationService accommodationReservationService;
        public OwnerActionReccommenderService()
        {
            locationService= new LocationService(Injector.CreateInstance<ILocationRepository>());
            UserService userService = new UserService(Injector.CreateInstance<IUserRepository>());
            accommodationService = new AccommodationService(Injector.CreateInstance<IAccommodationRepository>(), userService);
            accommodationReservationService = new AccommodationReservationService(Injector.CreateInstance<IAccommodationReservationRepository>());
        }
        public List<KeyValuePair<Location, double>> GetMostPopularLocations()
        {
            List<Location> allLocations = locationService.GetAll();
            List<KeyValuePair<Location, double>> mostPopularLocation = new List<KeyValuePair<Location, double>>();
            allLocations.ForEach(location => {
                double busyness = CalculateBusynessForLocation(location);
                mostPopularLocation.Add(new KeyValuePair<Location, double>(location, busyness));
            });
            mostPopularLocation.Sort((l1, l2) => l2.Value.CompareTo(l1.Value));
            return mostPopularLocation;
        }
        private double CalculateBusynessForLocation(Location location)
        {
            List<Accommodation> accommodationsOnsameLocation = accommodationService.GetAllAccommodationOnSameLocation(location);
            List<AccommodationReservation> reservations = new List<AccommodationReservation>();
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
    }
}
