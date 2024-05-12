using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Appl.UseCases
{
    public class ForumCommentService
    {
        private readonly IForumCommentRepository forumCommentRepository;
        public ForumCommentService() 
        { 
            forumCommentRepository = Injector.CreateInstance<IForumCommentRepository>();
        }
        public ForumCommentService(IForumCommentRepository forumCommentRepository)
        {
            this.forumCommentRepository = forumCommentRepository;
        }
        public List<ForumComment> GetAll()
        {
            return forumCommentRepository.GetAll();
        }
        public int GetNumberOfCommentsForForum(int forumId)
        {
            return forumCommentRepository.GetByForumId(forumId).Count;
        }
        public List<ForumComment> GetByForumId(int forumId)
        {
            return forumCommentRepository.GetByForumId(forumId);
        }
        public ForumComment GetById(int id)
        {
            return forumCommentRepository.GetById(id);
        }
        public ForumComment Update(ForumComment forumComment)
        {
            return forumCommentRepository.Update(forumComment);
        }
        public ForumComment Save(ForumComment forumComment)
        {
            return forumCommentRepository.Save(forumComment);
        }
        public void Delete(ForumComment forumComment)
        {
            forumCommentRepository.Delete(forumComment);
        }
        public List<ForumComment> GetByUser(User user)
        {
            return forumCommentRepository.GetByUser(user);
        }
    }
}
