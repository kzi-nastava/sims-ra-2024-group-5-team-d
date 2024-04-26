using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IForumCommentRepository
    {
        List<ForumComment> GetAll();
        List<ForumComment> GetByForumId(int forumId);
        ForumComment GetById(int id);
        ForumComment Update(ForumComment forumComment);
        ForumComment Save(ForumComment forumComment);
        public void Delete(ForumComment forumComment);
        public List<ForumComment> GetByUser(User user);
    }
}
