using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface ICheckPointRepository
    {
        public List<CheckPoint> GetAll();
        public CheckPoint Save(CheckPoint checkPoint);
        public void Delete(CheckPoint checkPoint);
        public CheckPoint Update(CheckPoint checkPoint);
        public CheckPoint GetCheckPointById(int? id);
    }
}
