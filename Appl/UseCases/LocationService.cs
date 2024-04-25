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
        public List<Location> GetMostPopularLocations()
        { 
            List<Location>allLocations=locationRepository.GetAll();
            List<KeyValuePair<Location, double>> mostPopularLocation = new List<KeyValuePair<Location, double>>();
            allLocations.ForEach(location => {
              double busyness=CalculateBusynessForLocation(location);
                mostPopularLocation.Add(new KeyValuePair<Location, double>(location, busyness));
            });
            mostPopularLocation.Sort((l1, l2) => l1.Value.CompareTo(l2.Value));
            mostPopularLocation.ForEach(location => Debug.WriteLine(location.Key + " " + location.Value));
            return mostPopularLocation.Select(location => location.Key).ToList();
        }
        private double CalculateBusynessForLocation(Location location)
        {
            int GlobalnumberOfReservations = 0;
            double Globaloccupancy = 0;
            int numberOfAccommodations = 0;
            accommodationService.GetAllAccommodationOnSameLocation(location).ForEach(accommodation => {
                    List<AccommodationReservation> allReservationsForAccommodation = accommodationReservationService.GetAllReservationsForAccommodation(accommodation.Id).Where(reservation => reservation.IsFinished()).ToList();
                    if (allReservationsForAccommodation.Count != 0)
                    {
                        numberOfAccommodations+= 1;
                        int numberOfReservations = allReservationsForAccommodation.Count();
                        double occupancy = 0;
                        allReservationsForAccommodation.ForEach(reservation => {
                            occupancy+=(double)reservation.NumberOfPeople / accommodation.Capacity;
                        });                      
                        occupancy = occupancy / numberOfReservations;
                        GlobalnumberOfReservations += numberOfReservations;
                        Globaloccupancy += occupancy;
                    }
                });
            Globaloccupancy = Globaloccupancy / numberOfAccommodations;
            return GlobalnumberOfReservations*Globaloccupancy;
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
