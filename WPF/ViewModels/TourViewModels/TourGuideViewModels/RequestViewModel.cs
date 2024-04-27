using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels.TourViewModels.TourGuideViewModels
{
    public class RequestViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public Location Location { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int Capacity { get; set; }
        public LANGUAGE Language { get; set; }
        public bool IsAcceptable { get; set; }

        public RequestViewModel() { }
        public RequestViewModel(int id,string description, Location location, DateTime dateFrom, DateTime dateTo, int capacity, LANGUAGE language, bool isAcceptable)
        {
            Id = id;
            Description = description;
            Location = location;
            DateFrom = dateFrom;
            DateTo = dateTo;
            Capacity = capacity;
            Language = language;
            IsAcceptable = isAcceptable;
        }
    }
}
