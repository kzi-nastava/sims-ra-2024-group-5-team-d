using BookingApp.Domain.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Models
{
    public class ForumComment:ISerializable
    {
        public int Id { get; set; }
        public int ForumId { get; set; }
        public int CreatorId { get; set; }
        public string Comment { get; set; }
        public DateTime DateCreated { get; set; }
        public ForumComment() { 
        
        }
        public ForumComment(int id, int forumId, int creatorId, string comment, DateTime dateCreated)
        {
            Id = id;
            ForumId = forumId;
            CreatorId = creatorId;
            Comment = comment;
            DateCreated = dateCreated;
        }
        public ForumComment(int forumId, int creatorId, string comment, DateTime dateCreated)
        {
            ForumId = forumId;
            CreatorId = creatorId;
            Comment = comment;
            DateCreated = dateCreated;
        }

        public string[] ToCSV()
        {
            return new string[] { Id.ToString(), ForumId.ToString(), CreatorId.ToString(), Comment, DateCreated.ToString("d/M/yyyy h:mm:ss tt") };
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            ForumId = int.Parse(values[1]);
            CreatorId = int.Parse(values[2]);
            Comment = values[3];
            DateCreated = DateTime.ParseExact(values[4], "d/M/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
        }
    }
}
