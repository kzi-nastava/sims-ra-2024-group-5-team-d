using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface ITourRepository
    {
        public List<Tour> GetAllTours(User user);
        public List<Tour> GetAllTours();
        public Tour SaveTour(Tour tour);
        public void DeleteTour(Tour tour);
        public Tour UpdateTour(Tour tour);
        public List<Tour> GetByUserTours(User user);
        public Tour GetTourById(int id);
        public int NextIdForTour();
    }
}
