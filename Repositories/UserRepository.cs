using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Domain.Serializer;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        private const string FilePath = "../../../Resources/Data/users.csv";

        private readonly Serializer<User> _serializer;

        private List<User> _users;

        public UserRepository()
        {
            _serializer = new Serializer<User>();
            _users = _serializer.FromCSV(FilePath);
        }

        public List<User> GetAll()
        {
            _users = _serializer.FromCSV(FilePath);
            return _users;
        }
        public User GetByUsername(string username)
        {
            _users = _serializer.FromCSV(FilePath);
            return _users.FirstOrDefault(u => u.Username == username);
        }
        public string GetFullNameById(int id)
        {
            _users = _serializer.FromCSV(FilePath);
            return _users.FirstOrDefault(u => u.Id == id).FullName;
        }
        public User GetById(int id)
        {
            _users = _serializer.FromCSV(FilePath);
            return _users.FirstOrDefault(u => u.Id == id);
        }

        private int NextId()
        {
            _users = _serializer.FromCSV(FilePath);
            if (_users.Count < 1)
            {
                return 1;
            }
            return _users.Max(c => c.Id) + 1;
        }
        public User Save(User user)
        {
            user.Id = NextId();
            _users = _serializer.FromCSV(FilePath);
            _users.Add(user);
            _serializer.ToCSV(FilePath, _users);
            return user;
        }
        public User Update(User user)
        {
            _users = _serializer.FromCSV(FilePath);
            var index = _users.FindIndex(u => u.Id == user.Id);
            _users[index] = user;
            _serializer.ToCSV(FilePath, _users);
            return user;
        }
    }
}
