using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Domain.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repositories
{
    public class ForumCommentRepository:IForumCommentRepository
    {
        private const string FilePath = "../../../Resources/Data/forumComments.csv";

        private readonly Serializer<ForumComment> _serializer;
        private List<ForumComment> forumComments;

        public ForumCommentRepository()
        {
            _serializer = new Serializer<ForumComment>();
            forumComments = _serializer.FromCSV(FilePath);
        }
        public List<ForumComment> GetAll()
        {
            forumComments = _serializer.FromCSV(FilePath);
            return forumComments;
        }
        public List<ForumComment> GetByForumId(int forumId)
        {
            forumComments = _serializer.FromCSV(FilePath);
            return forumComments.FindAll(forumComment => forumComment.ForumId == forumId);
        }
        public ForumComment GetById(int id)
        {
            forumComments = _serializer.FromCSV(FilePath);
            return forumComments.Find(forumComment => forumComment.Id == id);
        }
        public ForumComment Update(ForumComment forumComment)
        {
            forumComments = _serializer.FromCSV(FilePath);
            ForumComment current = forumComments.Find(fC => fC.Id == forumComment.Id);
            int index = forumComments.IndexOf(current);
            forumComments.Remove(current);
            forumComments.Insert(index, forumComment);
            _serializer.ToCSV(FilePath, forumComments);
            return forumComment;
        }

        public ForumComment Save(ForumComment forumComment)
        {
            forumComment.Id = NextId();
            forumComments = _serializer.FromCSV(FilePath);
            forumComments.Add(forumComment);
            _serializer.ToCSV(FilePath, forumComments);
            return forumComment;
        }

        public int NextId()
        {
            forumComments = _serializer.FromCSV(FilePath);
            if (forumComments.Count < 1)
            {
                return 1;
            }
            return forumComments.Max(c => c.Id) + 1;
        }

        public void Delete(ForumComment forumComment)
        {
            forumComments = _serializer.FromCSV(FilePath);
            ForumComment founded = forumComments.Find(fC => fC.Id == forumComment.Id);
            forumComments.Remove(founded);
            _serializer.ToCSV(FilePath, forumComments);
        }

        public List<ForumComment> GetByUser(User user)
        {
            forumComments = _serializer.FromCSV(FilePath);
            return forumComments.FindAll(forumComment => forumComment.CreatorId == user.Id);
        }
    }
}
