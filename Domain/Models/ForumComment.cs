using BookingApp.Domain.Serializer;
using System;
using System.Collections.Generic;
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
        public int NumberOfReports { get; set; }
        public DateTime DateCreated { get; set; }
        public ForumComment() { 
        
        }
        public ForumComment(int id, int forumId, int creatorId, string comment, int numberOfReports, DateTime dateCreated)
        {
            Id = id;
            ForumId = forumId;
            CreatorId = creatorId;
            Comment = comment;
            NumberOfReports = numberOfReports;
            DateCreated = dateCreated;
        }
        public ForumComment(int forumId, int creatorId, string comment, int numberOfReports, DateTime dateCreated)
        {
            ForumId = forumId;
            CreatorId = creatorId;
            Comment = comment;
            NumberOfReports = numberOfReports;
            DateCreated = dateCreated;
        }

        public string[] ToCSV()
        {
            return new string[] { Id.ToString(), ForumId.ToString(), CreatorId.ToString(), Comment, NumberOfReports.ToString(), DateCreated.ToString() };
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            ForumId = int.Parse(values[1]);
            CreatorId = int.Parse(values[2]);
            Comment = values[3];
            NumberOfReports = int.Parse(values[4]);
            DateCreated = DateTime.Parse(values[5]);
        }
    }
}
