using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels.TourViewModels.TourGuideViewModels
{
    public class TourFormViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int LocationId { get; set; }
        public double Duration { get; set; }
        public string Description { get; set; }
        public int LanguageId { get; set; }
        public int Capacity { get; set; }
        public DateTime StartTime { get; set; }
        public User User { get; set; }

        public TourFormViewModel() { }
        public TourFormViewModel(int id, string name, int locationId, double duration, string description, int languageId, int capacity, DateTime dateTime, string imagesPath)
        {
            Id = id;
            Name = name;
            LocationId = locationId;
            Duration = duration;
            Description = description;
            LanguageId = languageId;
            Capacity = capacity;
            StartTime = dateTime;
        }

    }
}
