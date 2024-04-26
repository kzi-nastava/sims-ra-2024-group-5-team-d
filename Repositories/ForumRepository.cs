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
    public class ForumRepository : IForumRepository
    {

        private const string FilePath = "../../../Resources/Data/forums.csv";

        private readonly Serializer<Forum> serializer;
        private List<Forum> forums;

        public ForumRepository()
        {
            serializer = new Serializer<Forum>();
            forums = serializer.FromCSV(FilePath);
        }
        public List<Forum> GetAll()
        {
            forums = serializer.FromCSV(FilePath);
            return forums;
        }
        public Forum GetByForumId(int forumId)
        {
            forums = serializer.FromCSV(FilePath);
            return forums.Find(forum => forum.Id == forumId);
        }
        public Forum GetById(int id)
        {
            forums = serializer.FromCSV(FilePath);
            return forums.Find(forum => forum.Id == id);
        }
        public Forum Update(Forum forum)
        {
            forums = serializer.FromCSV(FilePath);
            Forum current = forums.Find(f => f.Id == forum.Id);
            int index = forums.IndexOf(current);
            forums.Remove(current);
            forums.Insert(index, forum);
            serializer.ToCSV(FilePath, forums);
            return forum;
        }

        public Forum Save(Forum forum)
        {
            forum.Id = NextId();
            forums = serializer.FromCSV(FilePath);
            forums.Add(forum);
            serializer.ToCSV(FilePath, forums);
            return forum;
        }

        public int NextId()
        {
            forums = serializer.FromCSV(FilePath);
            if (forums.Count < 1)
            {
                return 1;
            }
            return forums.Max(c => c.Id) + 1;
        }

        public void Delete(Forum forum)
        {
            forums = serializer.FromCSV(FilePath);
            Forum founded = forums.Find(f => f.Id == forum.Id);
            forums.Remove(founded);
            serializer.ToCSV(FilePath, forums);
        }

        public List<Forum> GetByGuest(User guest)
        {
            forums = serializer.FromCSV(FilePath);
            return forums.FindAll(forum => forum.IdUser == guest.Id);
        }


    }
}
