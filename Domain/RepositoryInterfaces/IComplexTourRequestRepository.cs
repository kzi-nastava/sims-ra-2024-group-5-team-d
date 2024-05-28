using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IComplexTourRequestRepository
    {
        public List<ComplexTourRequest> GetAll(User user);
        public List<ComplexTourRequest> GetAll();
        public ComplexTourRequest Save(ComplexTourRequest request);
        public void Delete(ComplexTourRequest request);
        public ComplexTourRequest Update(ComplexTourRequest request);
        public ComplexTourRequest GetById(int id);
        public int NextId();
    }
}
