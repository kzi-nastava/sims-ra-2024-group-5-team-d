using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Appl.UseCases
{
    public class SuperUserService
    {
        private ISuperUserRepository superUserRepository;
        public SuperUserService()
        {
            superUserRepository = Injector.CreateInstance<ISuperUserRepository>();
        }
        public List<SuperUser> GetAll()
        {
            return superUserRepository.GetAll();
        }
        public SuperUser Save(SuperUser superUser)
        {
            return superUserRepository.Save(superUser);
        }
        public int NextId()
        {
            return superUserRepository.NextId();
        }
        public void Delete(SuperUser user)
        {
            superUserRepository.Delete(user);
        }
        public SuperUser Update(SuperUser superUser)
        {
            return superUserRepository.Update(superUser);
        }
        public SuperUser GetById(int id)
        {
            return superUserRepository.GetById(id);
        }
        public List<SuperUser> GetByUser(User user)
        {
            return superUserRepository.GetByUser(user);
        }
    }
}
