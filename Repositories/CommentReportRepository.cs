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
    public class CommentReportRepository : ICommentReportRepository
    {
        private const string FilePath = "../../../Resources/Data/commentReports.csv";

        private readonly Serializer<CommentReport> _serializer;
        private List<CommentReport> commentReports;

        public CommentReportRepository()
        {
            _serializer = new Serializer<CommentReport>();
            commentReports = _serializer.FromCSV(FilePath);
        }
        public List<CommentReport> GetAll()
        {
            commentReports = _serializer.FromCSV(FilePath);
            return commentReports;
        }
        public CommentReport GetById(int id)
        {
            commentReports = _serializer.FromCSV(FilePath);
            return commentReports.Find(commentReport => commentReport.Id == id);
        }
        public List<CommentReport> GetByCommentId(int commentId)
        {
            commentReports = _serializer.FromCSV(FilePath);
            return commentReports.FindAll(commentReport => commentReport.CommentId == commentId);
        }
        public List<CommentReport> GetByReporterId(int reporterId)
        {
            commentReports = _serializer.FromCSV(FilePath);
            return commentReports.FindAll(commentReport => commentReport.ReporterId == reporterId);
        }
        public CommentReport Save(CommentReport commentReport)
        {
            commentReport.Id = NextId();
            commentReports = _serializer.FromCSV(FilePath);
            commentReports.Add(commentReport);
            _serializer.ToCSV(FilePath, commentReports);
            return commentReport;
        }

        public int NextId()
        {
            commentReports = _serializer.FromCSV(FilePath);
            if (commentReports.Count < 1)
            {
                return 1;
            }
            return commentReports.Max(c => c.Id) + 1;
        }
    }
}
