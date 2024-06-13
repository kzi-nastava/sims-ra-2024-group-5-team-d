using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IAccommodationRenovationRepository
    {
        List<AccommodationRenovation> GetAll();
        AccommodationRenovation GetById(int id);
        AccommodationRenovation Update(AccommodationRenovation accommodationRenovation);
        AccommodationRenovation Save(AccommodationRenovation accommodationRenovation);
        void Delete(AccommodationRenovation accommodationRenovation);
        void DeleteById(int accommodationRenovationId);
    }
}
