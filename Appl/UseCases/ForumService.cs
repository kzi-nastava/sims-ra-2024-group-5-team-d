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
        private ForumCommentService forumCommentService;
        private UserService userService;
        private AccommodationReservationService accommodationReservationService;
        private AccommodationService accommodationService;
        public ForumService() 
        {
            accommodationService = new AccommodationService();
            accommodationReservationService = new AccommodationReservationService();
            userService = new UserService();
            forumCommentService = new ForumCommentService();
            locationService = new LocationService();
            forumRepository = Injector.CreateInstance<IForumRepository>();
        }
        public List<Forum> GetAll()
        {
            List<Forum> forums = forumRepository.GetAll();
            forums.ForEach(forum => forum.Location = locationService.GetById(forum.Location.Id));
            return forums;
        }
        public bool IsSuperForum(Forum forum)
        {
            List<ForumComment> forumComments = forumCommentService.GetByForumId(forum.Id);
            int ownerComments = GetNumberOfOwnerComments(forumComments,forum.Location);
            int userComments = GetNumberOfUserComments(forumComments,forum.Location);
            return ownerComments >= 10 && userComments >= 20;
        }

        private int GetNumberOfUserComments(List<ForumComment>forumComments,Location location)
        {
            return forumComments.Where(comment =>accommodationReservationService.HasReservationOnLocation(userService.GetById(comment.CreatorId), location)).Count();
        }
        private int GetNumberOfOwnerComments(List<ForumComment> forumComments, Location location)
        {
            return forumComments.Where(comment =>accommodationService.HasAccommodationOnLocation(userService.GetById(comment.CreatorId), location)).Count();
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
