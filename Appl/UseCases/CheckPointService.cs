using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
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
        public CheckPointService() 
        {
            _repository = Injector.CreateInstance<ICheckPointRepository>();
        }
        public CheckPoint GetById(int id)
        {
            return _repository.GetCheckPointById(id);
        }
        public List<CheckPoint> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
