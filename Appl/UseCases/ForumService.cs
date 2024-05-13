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
            locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            forumRepository = Injector.CreateInstance<IForumRepository>();
        }

        public ForumService(IForumRepository forumRepository,LocationService locationService)
        {
            this.locationService = locationService;
            this.forumRepository = forumRepository;
        }
        public List<Forum> GetAll()
        {
            List<Forum> forums = forumRepository.GetAll();
            forums.ForEach(forum => forum.Location = locationService.GetById(forum.Location.Id));
            return forums;
        }
        public List<Forum> GetAllByUser(User user)
        {
            List<Forum> forums = GetAll().Where(forum=> forum.IdUser==user.Id).ToList();
            return forums;
        }

        public Forum GetById(int Id)
        {
            Forum forum = forumRepository.GetById(Id);
            forum.Location = locationService.GetById(forum.Location.Id);
            return forum;
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
        public bool IsUserCreateForum(User user, Forum forum)
        {
            return forum.IdUser == user.Id;
                
        }
    }
}
