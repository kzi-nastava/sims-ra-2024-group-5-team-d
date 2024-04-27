using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface ICommentReportRepository
    {
        public List<CommentReport> GetAll();
        public CommentReport GetById(int id);
        public List<CommentReport> GetByCommentId(int commentId);
        public List<CommentReport> GetByReporterId(int reporterId);
        public CommentReport Save(CommentReport commentReport);
    }
}
