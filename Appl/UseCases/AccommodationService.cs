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
        private AccommodationRatingService accommodationRatingService;
        public AccommodationService() 
        {
            accommodationRatingService = new AccommodationRatingService();
            accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
        }
        public bool IsSuperOwner(User user)
        {
            return accommodationRepository.GetByUser(user).Any(accommodation => accommodation.IsSuperOwner);
        }
        public void UpgradeToSuperOwner(User user)
        {
            if(accommodationRatingService.GetNumberOfRatingsForOwner(user)>=50)
            accommodationRepository.GetByUser(user).ForEach(accommodation =>{ accommodation.IsSuperOwner = true;accommodationRepository.Update(accommodation); });
        }
        public void DowngradeFromSuperOwner(User user)
        {
            accommodationRepository.GetByUser(user).ForEach(accommodation => { accommodation.IsSuperOwner = false; accommodationRepository.Update(accommodation); });
        }
        public void UpdateOwnerStatus(double averageOwnerRating,User owner) {
            if (averageOwnerRating > 4.5 && !IsSuperOwner(owner))
                UpgradeToSuperOwner(owner);
            else if (averageOwnerRating < 4.5 && IsSuperOwner(owner))
                DowngradeFromSuperOwner(owner);
        }
    }
}
