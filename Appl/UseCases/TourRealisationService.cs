using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Appl.UseCases
{
    public class TourRealisationService
    {
        private ITourRealisationRepository _repository;
        public TourRealisationService() 
        {
            _repository = Injector.CreateInstance<ITourRealisationRepository>();
        }
        //potvrditi ovo
        public void DeleteTourRealisation(TourRealisation tourRealisation) 
        {
            _repository.DeleteTourRealisation(tourRealisation);
        }
        public void DeleteTourRealisationById(int id)
        {
            _repository.DeleteTourRealisationById(id);
        }
        //pitati sutra
        public List<TourRealisation> GetTourRealisationsForToday(int tourId)
        {
            List<TourRealisation> tourRealisationsToday = new List<TourRealisation>();

            foreach (TourRealisation tourRealisation in _repository.GetTourRealisationsByTourId(tourId))
            {
                if (tourRealisation.StartTime.Day == DateTime.Now.Day)
                {
                    tourRealisationsToday.Add(tourRealisation);
                }
            }

            return tourRealisationsToday;
        }
    }
}
