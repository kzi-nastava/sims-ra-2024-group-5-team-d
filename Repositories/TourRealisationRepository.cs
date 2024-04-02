using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repositories
{
    public class TourRealisationRepository : ITourRealisationRepository
    {
        private const string FilePathTourRealisations = "../../../Resources/Data/tourRealisations.csv";
        private readonly Serializer<TourRealisation> _serializerTourRealisations;

        private List<TourRealisation> _tourRealisations;

        public TourRealisationRepository()
        {
            _serializerTourRealisations = new Serializer<TourRealisation>();
            _tourRealisations = _serializerTourRealisations.FromCSV(FilePathTourRealisations);
        }

        public List<TourRealisation> GetAllTourRealisations()
        {
            return _serializerTourRealisations.FromCSV(FilePathTourRealisations);
        }

        public TourRealisation SaveTourRealisation(TourRealisation tourRealisation)
        {
            tourRealisation.Id = NextIdForTourRealisation();

            _tourRealisations = _serializerTourRealisations.FromCSV(FilePathTourRealisations);
            _tourRealisations.Add(tourRealisation);
            _serializerTourRealisations.ToCSV(FilePathTourRealisations, _tourRealisations);
            return tourRealisation;
        }

        private int NextIdForTourRealisation()
        {
            _tourRealisations = _serializerTourRealisations.FromCSV(FilePathTourRealisations);
            if (_tourRealisations.Count < 1)
            {
                return 1;
            }
            return _tourRealisations.Max(c => c.Id) + 1;
        }
        public void DeleteTourRealisation(TourRealisation tourRealisation)
        {
            _tourRealisations = _serializerTourRealisations.FromCSV(FilePathTourRealisations);
            TourRealisation founded = _tourRealisations.Find(c => c.Id == tourRealisation.Id);
            _tourRealisations.Remove(founded);
            _serializerTourRealisations.ToCSV(FilePathTourRealisations, _tourRealisations);
        }
        public TourRealisation UpdateTourRealisation(TourRealisation tourRealisation)
        {
            _tourRealisations = _serializerTourRealisations.FromCSV(FilePathTourRealisations);
            TourRealisation current = _tourRealisations.Find(c => c.Id == tourRealisation.Id);
            int index = _tourRealisations.IndexOf(current);
            _tourRealisations.Remove(current);
            _tourRealisations.Insert(index, tourRealisation);       // keep ascending order of ids in file 
            _serializerTourRealisations.ToCSV(FilePathTourRealisations, _tourRealisations);
            return tourRealisation;
        }
        public List<TourRealisation> GetByUserTourRealisations(User user)
        {
            _tourRealisations = _serializerTourRealisations.FromCSV(FilePathTourRealisations);
            return _tourRealisations.FindAll(c => c.User.Id == user.Id);
        }
        public TourRealisation GetTourRealisationById(int tourRealsiationId)
        {
            _tourRealisations = _serializerTourRealisations.FromCSV(FilePathTourRealisations);
            foreach (TourRealisation tR in _tourRealisations)
            {
                if (tR.Id == tourRealsiationId)
                {
                    return tR;
                }
            }
            return null;
        }
        public List<TourRealisation> GetTourRealisationsByTourId(int tourId)
        {
            _tourRealisations = _serializerTourRealisations.FromCSV(FilePathTourRealisations);
            List<TourRealisation> tourRealisations = new List<TourRealisation>();
            foreach (TourRealisation tourRealisation in _tourRealisations)
            {
                if (tourRealisation.TourId == tourId)
                {
                    Debug.WriteLine(tourId);
                    tourRealisations.Add(tourRealisation);
                }
            }
            return tourRealisations;
        }
        public List<TourRealisation> GetTourRealisationsForToday(int tourId)
        {
            List<TourRealisation> tourRealisationsToday = new List<TourRealisation>();

            foreach (TourRealisation tourRealisation in GetTourRealisationsByTourId(tourId))
            {
                if (tourRealisation.StartTime.Day == DateTime.Now.Day)
                {
                    tourRealisationsToday.Add(tourRealisation);
                }
            }

            return tourRealisationsToday;
        }
    }

}
