using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Domain.Serializer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repositories
{
    public class SuperUserRepository:ISuperUserRepository
    {
        private const string FilePath = "../../../Resources/Data/superUser.csv";

        private readonly Serializer<SuperUser> _serializer;

        private List<SuperUser> superUsers;
        private readonly LocationRepository _locationRepository;

        public SuperUserRepository()
        {
            _serializer = new Serializer<SuperUser>();
            _locationRepository = new LocationRepository();
            superUsers = _serializer.FromCSV(FilePath);
        }

        public List<SuperUser> GetAll()
        {
            superUsers = _serializer.FromCSV(FilePath);
            return superUsers;
        }
        public SuperUser Save(SuperUser superUser)
        {
            superUser.Id = NextId();
            superUsers = _serializer.FromCSV(FilePath);
            superUsers.Add(superUser);
            _serializer.ToCSV(FilePath, superUsers);
            return superUser;
        }

        public int NextId()
        {
            superUsers = _serializer.FromCSV(FilePath);
            if (superUsers.Count < 1)
            {
                return 1;
            }
            return superUsers.Max(c => c.Id) + 1;
        }

        public void Delete(SuperUser user)
        {
            superUsers = _serializer.FromCSV(FilePath);
            SuperUser founded = superUsers.Find(u => u.Id == user.Id);
            superUsers.Remove(founded);
            _serializer.ToCSV(FilePath, superUsers);
        }

        public SuperUser Update(SuperUser superUser)
        {
            superUsers = _serializer.FromCSV(FilePath);
            SuperUser current = superUsers.Find(u => u.Id == superUser.Id);
            int index = superUsers.IndexOf(current);
            superUsers.Remove(current);
            superUsers.Insert(index, superUser);       // keep ascending order of ids in file 
            _serializer.ToCSV(FilePath, superUsers);
            return superUser;
        }
        public SuperUser GetById(int id)
        {
            superUsers = _serializer.FromCSV(FilePath);
            return superUsers.Find(superUser => superUser.Id == id);
        }
        public List<SuperUser> GetByUser(User user)
        {
            superUsers = _serializer.FromCSV(FilePath);
            return superUsers.FindAll(superUser => superUser.UserId == user.Id);
        }
    }
}
