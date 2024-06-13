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

        public LocationService(ILocationRepository locationRepository)
        { 
        this.locationRepository = locationRepository;
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
