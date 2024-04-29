using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface ISuperUserRepository
    {
        public List<SuperUser> GetAll();
        public SuperUser Save(SuperUser superUser);
        public int NextId();
        public void Delete(SuperUser user);
        public SuperUser Update(SuperUser superUser);
        public SuperUser GetById(int id);
        public List<SuperUser> GetByUser(User user);
    }
}
