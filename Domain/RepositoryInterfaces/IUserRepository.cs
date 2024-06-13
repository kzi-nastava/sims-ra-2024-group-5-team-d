using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IUserRepository
    {
        public User GetByUsername(string username);
        public User GetById(int id);
        public User Save(User user);
        public string GetFullNameById(int id);
        public List<User> GetAll();
        public User Update(User user);
    }
}
