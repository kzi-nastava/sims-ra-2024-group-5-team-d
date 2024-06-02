using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Appl.UseCases
{
    public class UserService
    {
        private IUserRepository userRepository;
        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;

        }
        public UserService()
        {
            this.userRepository =Injector.CreateInstance<IUserRepository>();

        }
        public void DemoteUser(User user)
        {
            User found = GetById(user.Id);
            if (found.IsSuperUser == true)
            {
                found.IsSuperUser = false;
                Update(found);
            }
        }
        public void PromoteUser(User user)
        {
            User found = GetById(user.Id);
            if (found.IsSuperUser == false)
            {
                found.IsSuperUser = true;
                Update(found);
            }
        }
        public bool IsSuperOwner(User user)
        {
            User found=GetById(user.Id);
            return found.IsSuper()&& found.IsOwner();
        }
        public List<User> GetUsersWithSameMacAdress(string macAdress)
        {
            return userRepository.GetAll().Where(user=>user.MacAddress==macAdress).ToList();
        }
        public User GetById(int id)
        {
            return userRepository.GetById(id);
        }
        public User GetByUsername(string username)
        {
            return userRepository.GetByUsername(username);
        }
        public string GetFullNameById(int id)
        {
            return userRepository.GetFullNameById(id);
        }
        public User Save(User user)
        {
            return userRepository.Save(user);
        }
        public int GetAge(DateOnly dateTime)
        {
            int age = DateTime.Today.Year - dateTime.Year;
            return age;
        }
        public User Update(User user)
        {
            return userRepository.Update(user);
        }
        public List<User> GetAll()
        {
            return userRepository.GetAll();
        }

        public void UpdateCredentials(User loggedInUser, string username, string password)
        {
            loggedInUser.Username = username;
            loggedInUser.Password = password;
            Update(loggedInUser);
        }

        public User GetByPersonalID(string personalId)
        {
            User user = new User();


            userRepository.GetAll().ForEach(u =>
            {
                if (u.PersonalId == personalId)
                    user = u;
            });

            return user;
        }
    }
}
