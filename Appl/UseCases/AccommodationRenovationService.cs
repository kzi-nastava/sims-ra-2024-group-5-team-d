using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
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
        private AccommodationService accommodationService;
        public AccommodationRenovationService()
        {
            accommodationService = new AccommodationService();
            accommodationRenovationRepository =Injector.CreateInstance<IAccommodationRenovationRepository>();
        }
        public AccommodationRenovationService(IAccommodationRenovationRepository accommodationRenovationRepository)
        {
            this.accommodationRenovationRepository = accommodationRenovationRepository;
        }
        public AccommodationRenovationService(IAccommodationRenovationRepository accommodationRenovationRepository, AccommodationService accommodationService)
        {
            this.accommodationRenovationRepository = accommodationRenovationRepository;
            this.accommodationService = accommodationService;
        }
        public AccommodationRenovation Save(AccommodationRenovation accommodationRenovation)
        {
            return accommodationRenovationRepository.Save(accommodationRenovation);
        }
        public AccommodationRenovation GetAccommodationRenovationById(int id)
        {
            return accommodationRenovationRepository.GetById(id);
        }
        public List<AccommodationRenovation> GetAll()
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
        public void DeleteById(int accommodationRenovationId)
        {
            accommodationRenovationRepository.DeleteById(accommodationRenovationId);
        }
        public List<AccommodationRenovation> GetRenovationsForOwner(User owner)
        {
            return GetAll().Where(accommodationRenovation=> IsReservationForOwnerAccommodation(accommodationRenovation,owner)).ToList();
        }
        public bool IsReservationForOwnerAccommodation(AccommodationRenovation renovation, User owner)
        {
            return accommodationService.IsUserOwnerOfAccommodation(owner, renovation.AccommodationId);
        }

        public List<AccommodationRenovation> GetByAccommodation(Accommodation accommodation)
        {
            return accommodationRenovationRepository.GetAll().Where(renovation => renovation.AccommodationId == accommodation.Id).ToList();
        }
    }
}
