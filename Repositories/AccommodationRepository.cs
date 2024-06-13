using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Domain.Serializer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace BookingApp.Repositories
{
    public class AccommodationRepository : IAccommodationRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodations.csv";

        private readonly Serializer<Accommodation> _serializer;

        private List<Accommodation> _accommodations;
        private readonly LocationRepository _locationRepository;

        public AccommodationRepository()
        {
            _serializer = new Serializer<Accommodation>();
            _locationRepository = new LocationRepository();
            _accommodations = _serializer.FromCSV(FilePath);
        }

        public List<Accommodation> GetAll()
        {
            _accommodations = _serializer.FromCSV(FilePath);
            _accommodations.ForEach(accommodation => accommodation.Location = _locationRepository.GetById(accommodation.Location.Id));
            return _accommodations;
        }
        public Accommodation Save(Accommodation accommodation)
        {
            accommodation.Id = NextId();
            _accommodations = _serializer.FromCSV(FilePath);
            _accommodations.Add(accommodation);
            _serializer.ToCSV(FilePath, _accommodations);
            return accommodation;
        }

        public int NextId()
        {
            _accommodations = _serializer.FromCSV(FilePath);
            if (_accommodations.Count < 1)
            {
                return 1;
            }
            return _accommodations.Max(c => c.Id) + 1;
        }

        public void Delete(Accommodation accommodation)
        {
            _accommodations = _serializer.FromCSV(FilePath);
            Accommodation founded = _accommodations.Find(a => a.Id == accommodation.Id);
            _accommodations.Remove(founded);
            Directory.Delete(founded.ImagesPath, true);
            _serializer.ToCSV(FilePath, _accommodations);
        }

        public Accommodation Update(Accommodation accommodation)
        {
            _accommodations = _serializer.FromCSV(FilePath);
            Accommodation current = _accommodations.Find(a => a.Id == accommodation.Id);
            int index = _accommodations.IndexOf(current);
            _accommodations.Remove(current);
            _accommodations.Insert(index, accommodation);       // keep ascending order of ids in file 
            _serializer.ToCSV(FilePath, _accommodations);
            return accommodation;
        }
        public Accommodation GetById(int id)
        {
            _accommodations = _serializer.FromCSV(FilePath);
            _accommodations.ForEach(accommodation => accommodation.Location = _locationRepository.GetById(accommodation.Location.Id));
            return _accommodations.Find(accommodation => accommodation.Id == id);
        }
        public List<Accommodation> GetByUser(User user)
        {
            _accommodations = _serializer.FromCSV(FilePath);
            _accommodations.ForEach(accommodation => accommodation.Location = _locationRepository.GetById(accommodation.Location.Id));
            return _accommodations.FindAll(accommodation => accommodation.Owner.Id == user.Id);
        }
        public string GetAccommodationNameById(int accommodationId)
        {
            return _accommodations.Find(accommodation => accommodation.Id == accommodationId).Name;
        }


    }
}
