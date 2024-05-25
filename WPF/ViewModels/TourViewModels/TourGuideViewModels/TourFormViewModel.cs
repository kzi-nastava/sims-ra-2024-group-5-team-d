using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels.TourViewModels.TourGuideViewModels
{
    public class TourFormViewModel : INotifyPropertyChanged
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; NotifyPropertyChanged(nameof(Id)); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; NotifyPropertyChanged(nameof(Name)); }
        }

        private int _locationId;
        public int LocationId
        {
            get { return _locationId; }
            set { _locationId = value; NotifyPropertyChanged(nameof(LocationId)); }
        }

        private double _duration;
        public double Duration
        {
            get { return _duration; }
            set { _duration = value; NotifyPropertyChanged(nameof(Duration)); }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { _description = value; NotifyPropertyChanged(nameof(Description)); }
        }

        private int _languageId;
        public int LanguageId
        {
            get { return _languageId; }
            set { _languageId = value; NotifyPropertyChanged(nameof(LanguageId)); }
        }

        private int _capacity;
        public int Capacity
        {
            get { return _capacity; }
            set { _capacity = value; NotifyPropertyChanged(nameof(Capacity)); }
        }

        private DateTime _startTime;
        public DateTime StartTime
        {
            get { return _startTime; }
            set { _startTime = value; NotifyPropertyChanged(nameof(StartTime)); }
        }

        private User _user;
        public User User
        {
            get { return _user; }
            set { _user = value; NotifyPropertyChanged(nameof(User)); }
        }

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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
