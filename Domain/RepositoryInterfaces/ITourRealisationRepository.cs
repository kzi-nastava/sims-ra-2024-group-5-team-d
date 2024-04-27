using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface ITourRealisationRepository
    {
        public List<TourRealisation> GetAllTourRealisations(User user);
        public List<TourRealisation> GetAllTourRealisations();
        public TourRealisation SaveTourRealisation(TourRealisation tourRealisation);
        public void DeleteTourRealisation(TourRealisation tourRealisation);
        public TourRealisation UpdateTourRealisation(TourRealisation tourRealisation);
        public List<TourRealisation> GetByUserTourRealisations(User user);
        public TourRealisation GetTourRealisationById(int tourRealsiationId);
        public List<TourRealisation> GetTourRealisationsByTourId(int tourId);
        public void DeleteTourRealisationById(int id);
    }
}
