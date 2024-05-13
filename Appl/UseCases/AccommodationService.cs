using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Appl.UseCases
{
    public class AccommodationService
    {
        private IAccommodationRepository accommodationRepository;
        private UserService userService;
        public AccommodationService() 
        {
            userService = new UserService();
            accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
        }
        public AccommodationService(IAccommodationRepository accommodationRepository,UserService userService)
        {
            this.userService = userService;
            this.accommodationRepository = accommodationRepository;
        }
        public AccommodationService(IAccommodationRepository accommodationRepository)
        {
            this.accommodationRepository = accommodationRepository;
        }
        public List<Accommodation>GetAllAccommodationOnSameLocation(Location location)
        {
            return accommodationRepository.GetAll().Where(accommodation=>accommodation.Location.Id==location.Id).ToList();
        }
        public bool HasAccommodationOnLocation(User owner,Location location)
        {
            List<Accommodation> accomodations = accommodationRepository.GetByUser(owner);
            return accomodations.Any(accommodation => accommodation.Location.Id == location.Id);
        }
        public List<Accommodation> GetAll()
        {
            List<Accommodation>accommodations=accommodationRepository.GetAll();
            accommodations.ForEach(accommodation => accommodation.Owner = userService.GetById(accommodation.Owner.Id));
            return accommodations;
        }
        public Accommodation Save(Accommodation accommodation)
        {
            return accommodationRepository.Save(accommodation);
        }
        public void Delete(Accommodation accommodation)
        {
            accommodationRepository.Delete(accommodation);
        }
        public Accommodation GetBestRatedAccommodationForOwner(User owner)
        {
            return GetByUser(owner).OrderByDescending(accommodation => accommodation.AverageRating).First();
        }
        public Accommodation Update(Accommodation accommodation)
        {
            return accommodationRepository.Update(accommodation);
        }
        public Accommodation GetById(int id)
        {
            Accommodation accommodation = accommodationRepository.GetById(id);
            accommodation.Owner=userService.GetById(accommodation.Owner.Id);
            return accommodation;
        }
        public List<Accommodation> GetByUser(User user)
        {
            List<Accommodation> accommodations = accommodationRepository.GetByUser(user);
            accommodations.ForEach(accommodation => accommodation.Owner = userService.GetById(accommodation.Owner.Id));
            return accommodations;
        }
        public string GetAccommodationNameById(int accommodationId)
        {
            return accommodationRepository.GetById(accommodationId).Name;
        }
        public bool IsUserOwnerOfAccommodation(User user, int accommodationId)
        {
            return accommodationRepository.GetByUser(user).Any(accommodation => accommodation.Id == accommodationId);
        }
        public int NextId()
        {
            return accommodationRepository.NextId();
        }
    }
}
