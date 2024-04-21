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
    public class AccommodationRenovationRepository:IAccommodationRenovationRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodationRenovations.csv";

        private readonly Serializer<AccommodationRenovation> _serializer;
        private List<AccommodationRenovation> _accommodationRenovations;

        public AccommodationRenovationRepository()
        {
            _serializer = new Serializer<AccommodationRenovation>();
            _accommodationRenovations = _serializer.FromCSV(FilePath);
        }
        public List<AccommodationRenovation> GetAll()
        {
            _accommodationRenovations = _serializer.FromCSV(FilePath);
            return _accommodationRenovations;
        }
        public AccommodationRenovation GetById(int id)
        {
            _accommodationRenovations = _serializer.FromCSV(FilePath);
            return _accommodationRenovations.Find(accommodationRenovation => accommodationRenovation.Id == id);
        }
        public AccommodationRenovation Update(AccommodationRenovation accommodationRenovation)
        {
            _accommodationRenovations = _serializer.FromCSV(FilePath);
            AccommodationRenovation current = _accommodationRenovations.Find(a => a.Id == accommodationRenovation.Id);
            int index = _accommodationRenovations.IndexOf(current);
            _accommodationRenovations.Remove(current);
            _accommodationRenovations.Insert(index, accommodationRenovation);
            _serializer.ToCSV(FilePath, _accommodationRenovations);
            return accommodationRenovation;
        }

        public AccommodationRenovation Save(AccommodationRenovation accommodationRenovation)
        {
            accommodationRenovation.Id = NextId();
            _accommodationRenovations = _serializer.FromCSV(FilePath);
            _accommodationRenovations.Add(accommodationRenovation);
            _serializer.ToCSV(FilePath, _accommodationRenovations);
            return accommodationRenovation;
        }

        public int NextId()
        {
            _accommodationRenovations = _serializer.FromCSV(FilePath);
            if (_accommodationRenovations.Count < 1)
            {
                return 1;
            }
            return _accommodationRenovations.Max(c => c.Id) + 1;
        }
        public void DeleteById(int accommodationRenovationId) {

            _accommodationRenovations = _serializer.FromCSV(FilePath);
            AccommodationRenovation founded = _accommodationRenovations.Find(g => g.Id == accommodationRenovationId);
            _accommodationRenovations.Remove(founded);
            _serializer.ToCSV(FilePath, _accommodationRenovations);
        }
    

        public void Delete(AccommodationRenovation accommodationRenovation)
        {
            _accommodationRenovations = _serializer.FromCSV(FilePath);
            AccommodationRenovation founded = _accommodationRenovations.Find(g => g.Id == accommodationRenovation.Id);
            _accommodationRenovations.Remove(founded);
            _serializer.ToCSV(FilePath, _accommodationRenovations);
        }

    }
}
