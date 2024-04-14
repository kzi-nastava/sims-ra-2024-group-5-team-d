using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Domain.Serializer;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Appl.UseCases
{
    public class CheckPointService
    {
        public ICheckPointRepository _repository { get; set; }
        public TourService _tourService { get; set; }
        public CheckPointService() 
        {
            _repository = Injector.CreateInstance<ICheckPointRepository>();
            _tourService = new TourService();
        }
        public CheckPoint GetById(int id)
        {
            return _repository.GetCheckPointById(id);
        }
        public List<CheckPoint> GetAll()
        {
            return _repository.GetAll();
        }
        public List<CheckPoint> SuggestCheckPoints(int locationId)
        {
            var checkPointsInCity = _repository.GetAll()
                                    .Where(cp => _tourService.GetById(cp.TourId).Location.Id == locationId)
                                    .DistinctBy(cp => cp.Name)
                                    .ToList();

            checkPointsInCity.ForEach(cp => cp.IsChecked = false);

            return checkPointsInCity;
        }
        public CheckPoint Save(CheckPoint cp)
        {
            return _repository.Save(cp);
        }
        public CheckPoint Update(CheckPoint cp)
        {
            return _repository.Update(cp);
        }
        public List<CheckPoint> GetAllCheckPointsByTourId(int tourId)
        {
            List<CheckPoint> checkPointsForTour = new List<CheckPoint>();
            foreach (CheckPoint checkPoint in _repository.GetAll())
            {
                if (checkPoint.TourId == tourId)
                {
                    checkPointsForTour.Add(checkPoint);
                }
            }
            return checkPointsForTour;
        }

    }
}
