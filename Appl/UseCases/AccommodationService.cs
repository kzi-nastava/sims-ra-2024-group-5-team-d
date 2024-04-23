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
        public AccommodationService() 
        {
            accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
        }
        public bool IsSuperOwner(User user)
        {
            return accommodationRepository.GetByUser(user).Any(accommodation => accommodation.IsSuperOwner);
        }
        public void UpgradeToSuperOwner(User user)
        {
                accommodationRepository.GetByUser(user).ForEach(accommodation =>{ accommodation.IsSuperOwner = true;accommodationRepository.Update(accommodation); });
        }
        public void DowngradeFromSuperOwner(User user)
        {
            accommodationRepository.GetByUser(user).ForEach(accommodation => { accommodation.IsSuperOwner = false; accommodationRepository.Update(accommodation); });
        }
        public void UpdateOwnerStatus(double averageOwnerRating,User owner) {
            if (averageOwnerRating >= 4.5 && !IsSuperOwner(owner))
                UpgradeToSuperOwner(owner);
            else if (averageOwnerRating < 4.5 && IsSuperOwner(owner))
                DowngradeFromSuperOwner(owner);
        }
        public List<Accommodation> GetAll()
        {
            return accommodationRepository.GetAll();
        }
        public Accommodation Save(Accommodation accommodation)
        {
            return accommodationRepository.Save(accommodation);
        }
        public void Delete(Accommodation accommodation)
        {
            accommodationRepository.Delete(accommodation);
        }
        public Accommodation Update(Accommodation accommodation)
        {
            return accommodationRepository.Update(accommodation);
        }
        public Accommodation GetById(int id)
        {
            return accommodationRepository.GetById(id);
        }
        public List<Accommodation> GetByUser(User user)
        {
            return accommodationRepository.GetByUser(user);
        }
        public string GetAccommodationNameById(int accommodationId)
        {
            return accommodationRepository.GetById(accommodationId).Name;
        }
        public bool IsUserOwnerOfAccommodation(User user, int accommodationId)
        {
            return accommodationRepository.GetByUser(user).Any(accommodation => accommodation.Id == accommodationId);
        }
    }
}
