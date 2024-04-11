using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels
{
    public class TourRealisationViewModel
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int TourId { get; set; }
        public bool NotEnoughSpace { get; set; }
        public User User { get; set; }
        public int AvailableSeats { get; set; }
        public bool IsCancellable { get; set; }
        public TourRealisationViewModel() { }

        public TourRealisationViewModel(int id, DateTime dateTime, int tourId, int availableSeats,bool cancel, User user)
        {
            Id = id;
            DateTime = dateTime;
            TourId = tourId;
            AvailableSeats = availableSeats;
            User = user;
            IsCancellable = cancel;
        }         

        public TourRealisationViewModel(int id, DateTime dateTime, int tourId, int availableSeats, User user)
        {
            Id = id;
            DateTime = dateTime;
            TourId = tourId;
            AvailableSeats = availableSeats;
            User = user;
        }

        public TourRealisationViewModel(int id, DateTime dateTime, int tourId, int availableSeats, double duration, User user, int wantedNumberOfSeats)
        {
            Id = id;
            DateTime = dateTime;
            TourId = tourId;
            AvailableSeats = availableSeats;
            User = user;
            StartTime = TimeOnly.FromDateTime(DateTime);
            EndTime = StartTime.AddHours(duration);
            NotEnoughSpace = AvailableSeats < wantedNumberOfSeats;
        }
    }
}
