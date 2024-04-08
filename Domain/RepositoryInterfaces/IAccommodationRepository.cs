using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IAccommodationRepository
    {
        public List<Accommodation> GetAll();
        public Accommodation Save(Accommodation accommodation);
        public void Delete(Accommodation accommodation);
        public Accommodation Update(Accommodation accommodation);
        public List<Accommodation> GetByUser(User user);
        public string GetAccommodationNameById(int accommodationId);
        public Accommodation GetById(int accommodationId);
        public int NextId();
    }
}
