using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Domain.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repositories
{
    public class ComplexSimpleRequestPairRepository : IComplexSimpleRequestPairRepository
    {
        private const string FilePath = "../../../Resources/Data/complexSimpleRequestPairs.csv";

        private readonly Serializer<ComplexSimpleRequestPair> _serializerComplexSimpleRequestPairs;

        private List<ComplexSimpleRequestPair> pairs;

        public ComplexSimpleRequestPairRepository()
        {
            _serializerComplexSimpleRequestPairs = new Serializer<ComplexSimpleRequestPair>();
            pairs = _serializerComplexSimpleRequestPairs.FromCSV(FilePath);
        }

        public void Delete(ComplexSimpleRequestPair request)
        {
            pairs = _serializerComplexSimpleRequestPairs.FromCSV(FilePath);
            ComplexSimpleRequestPair founded = pairs.Find(c => c.ComplexRequestId == request.ComplexRequestId && c.SimpleRequestId == request.SimpleRequestId);
            pairs.Remove(founded);
            _serializerComplexSimpleRequestPairs.ToCSV(FilePath, pairs);
        }

        public List<ComplexSimpleRequestPair> GetAll()
        {
            return _serializerComplexSimpleRequestPairs.FromCSV(FilePath);
        }

        public void Save(ComplexSimpleRequestPair request)
        {
            pairs = _serializerComplexSimpleRequestPairs.FromCSV(FilePath);
            pairs.Add(request);
            _serializerComplexSimpleRequestPairs.ToCSV(FilePath, pairs);
        }
    }
}
