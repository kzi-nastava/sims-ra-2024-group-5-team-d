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
    public class CommentReportService
    {
        private ICommentReportRepository commentReportRepository;
        public CommentReportService(ICommentReportRepository commentReportRepository) { 
        this.commentReportRepository = commentReportRepository;
        }
        public bool IsAlreadyReported(int commentId, int reporterId)
        {
            return commentReportRepository.GetByCommentId(commentId).Any(report => report.ReporterId == reporterId);
        }
        public void ReportComment(int commentId, int reporterId)
        {
           Save(new CommentReport(commentId, reporterId));
        }
        public List<CommentReport> GetAll()
        {
            return commentReportRepository.GetAll();
        }
        public CommentReport GetById(int Id)
        {
            return commentReportRepository.GetById(Id);
        }
        public List<CommentReport> GetByCommentId(int commentId)
        {
            return commentReportRepository.GetByCommentId(commentId);
        }
        public List<CommentReport> GetByReporterId(int reporterId)
        {
            return commentReportRepository.GetByReporterId(reporterId);
        }
        public CommentReport Save(CommentReport commentReport)
        {
            return commentReportRepository.Save(commentReport);
        }

        public int GetNumberOfReportsForComment(int commentId)
        {
            return GetAll().Where(report => report.CommentId == commentId).Count();
        }
    }
}
