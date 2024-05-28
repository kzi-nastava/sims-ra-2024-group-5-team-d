using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Appl.UseCases
{
    public class ComplexSimpleRequestPairService
    {
        private IComplexSimpleRequestPairRepository _repository;

        public ComplexSimpleRequestPairService()
        {
            _repository = Injector.CreateInstance<IComplexSimpleRequestPairRepository>();
        }

        public void Delete(ComplexSimpleRequestPair request)
        {
            _repository.Delete(request);
        }

        public List<ComplexSimpleRequestPair> GetAll()
        {
            return _repository.GetAll();
        }

        public void Save(ComplexSimpleRequestPair request)
        {
            _repository.Save(request);
        }
    }
}
