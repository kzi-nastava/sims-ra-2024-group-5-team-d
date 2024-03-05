using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class LocationRepository
    {
        private const string FilePath = "../../../Resources/Data/locations.csv";

        private readonly Serializer<Location> _serializer;

        private List<Location> _locations;

        public LocationRepository()
        {
            _serializer = new Serializer<Location>();
            _locations= _serializer.FromCSV(FilePath);
        }
        public List<Location> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public Location GetById(int Id)
        {
            _locations = _serializer.FromCSV(FilePath);
            return _locations.Find(l =>l.Id == Id);
        }
    }
}
