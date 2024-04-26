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
    public class ForumService
    {
        private IForumRepository forumRepository;
        private LocationService locationService;
        public ForumService() 
        {
            locationService = new LocationService();
            forumRepository = Injector.CreateInstance<IForumRepository>();
        }
        public List<Forum> GetAll()
        {
            List<Forum> forums = forumRepository.GetAll();
            forums.ForEach(forum => forum.Location = locationService.GetById(forum.Location.Id));
            return forums;
        }
        public Forum GetById(int Id)
        {
            return forumRepository.GetById(Id);
        }
        public Forum Update(Forum forum)
        {
            return forumRepository.Update(forum);
        }
        public void Delete(Forum forum)
        {
            forumRepository.Delete(forum);
        }
        public Forum Save(Forum forum)
        {
            return forumRepository.Save(forum);
        }
    }
}
