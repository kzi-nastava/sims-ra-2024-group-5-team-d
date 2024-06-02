using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Appl.UseCases
{
    public class SuperForumService
    {
        private ForumCommentService forumCommentService;
        private AccommodationReservationService accommodationReservationService;
        private AccommodationService accommodationService;
        private UserService userService;
        public SuperForumService()
        {
            userService = new UserService(Injector.CreateInstance<IUserRepository>());
            accommodationService = new AccommodationService(Injector.CreateInstance<IAccommodationRepository>(),userService);
            accommodationReservationService = new AccommodationReservationService(Injector.CreateInstance<IAccommodationReservationRepository>(),accommodationService);
            forumCommentService = new ForumCommentService(Injector.CreateInstance<IForumCommentRepository>());
        }
        public bool IsSuperForum(Forum forum)
        {
            List<ForumComment> forumComments = forumCommentService.GetByForumId(forum.Id);
            int ownerComments = GetNumberOfOwnerComments(forumComments, forum.Location);
            int userComments = GetNumberOfGuestComments(forumComments, forum.Location);
            return ownerComments >= 10 && userComments >= 20;
        }
        private int GetNumberOfGuestComments(List<ForumComment> forumComments, Location location)
        {
            return forumComments.Where(comment => accommodationReservationService.HasReservationOnLocation(userService.GetById(comment.CreatorId), location)).Count();
        }
        private int GetNumberOfOwnerComments(List<ForumComment> forumComments, Location location)
        {
            return forumComments.Where(comment => accommodationService.HasAccommodationOnLocation(userService.GetById(comment.CreatorId), location)).Count();
        }
    }
}
