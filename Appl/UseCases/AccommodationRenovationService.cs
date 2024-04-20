using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Appl.UseCases
{
    public class AccommodationRenovationService
    {
        private IAccommodationRenovationRepository accommodationRenovationRepository;
        public AccommodationRenovationService()
        {
            accommodationRenovationRepository =Injector.CreateInstance<IAccommodationRenovationRepository>();
        }
        public AccommodationRenovation Save(AccommodationRenovation accommodationRenovation)
        {
            return accommodationRenovationRepository.Save(accommodationRenovation);
        }
        public AccommodationRenovation GetAccommodationRenovationById(int id)
        {
            return accommodationRenovationRepository.GetById(id);
        }
        public List<AccommodationRenovation> GetAllAccommodationRenovations()
        {
            return accommodationRenovationRepository.GetAll();
        }
        public AccommodationRenovation Update(AccommodationRenovation accommodationRenovation)
        {
            return accommodationRenovationRepository.Update(accommodationRenovation);
        }
        public void Delete(AccommodationRenovation accommodationRenovation)
        {
            accommodationRenovationRepository.Delete(accommodationRenovation);
        }
    }
}
