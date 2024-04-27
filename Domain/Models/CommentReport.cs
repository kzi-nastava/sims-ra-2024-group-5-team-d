using BookingApp.Domain.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Models
{
    public class CommentReport : ISerializable
    {
        public int Id { get; set; }
        public int CommentId { get; set; }
        public int ReporterId { get; set; }
        public CommentReport()
        {

        }
        public CommentReport(int id, int commentId, int reporterId)
        {
            Id = id;
            CommentId = commentId;
            ReporterId = reporterId;
        }
        public CommentReport(int commentId, int reporterId)
        {
            CommentId = commentId;
            ReporterId = reporterId;
        }
        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            CommentId = int.Parse(values[1]);
            ReporterId = int.Parse(values[2]);
        }

        public string[] ToCSV()
        {
            return new string[] { Id.ToString(), CommentId.ToString(), ReporterId.ToString() };
        }
    }
}
