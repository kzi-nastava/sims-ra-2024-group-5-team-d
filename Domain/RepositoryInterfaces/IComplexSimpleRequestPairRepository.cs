using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IComplexSimpleRequestPairRepository
    {
        public List<ComplexSimpleRequestPair> GetAll();
        public void Save(ComplexSimpleRequestPair request);
        public void Delete(ComplexSimpleRequestPair request);
    }
}
