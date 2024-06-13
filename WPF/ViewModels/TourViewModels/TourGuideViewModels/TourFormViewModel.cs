using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Validation;
using System.Diagnostics;
using System.Collections.ObjectModel;
using ceTe.DynamicPDF.PageElements.Charting.Series;

namespace BookingApp.WPF.ViewModels.TourViewModels.TourGuideViewModels
{
    public class TourFormViewModel : ValidationBase,INotifyPropertyChanged
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set 
            {   
                if (_id != value)
                {

                    _id = value;
                    NotifyPropertyChanged(nameof(Id));
                }
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set 
            {   
                if (_name != value)
                {
                    _name = value;
                    NotifyPropertyChanged(nameof(Name));
                    
                }
            }
        }

        private int _locationId;
        public int LocationId
        {
            get { return _locationId; }
            set 
            {
                if (value != _locationId)
                {
                    _locationId = value;
                    NotifyPropertyChanged(nameof(LocationId));
                    
                }
            }
        }

        private double _duration;
        public double Duration
        {
            get { return _duration; }
            set 
            { 
                if (_duration != value)
                {
                    _duration = value; 
                    NotifyPropertyChanged(nameof(Duration));
                    
                }
            }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set 
            { 
                if (_description != value)
                {
                    _description = value; 
                    NotifyPropertyChanged(nameof(Description));
                    
                }
            }
        }

        private int _languageId;
        public int LanguageId
        {
            get { return _languageId; }
            set 
            { 
                if(_languageId != value)
                {
                    _languageId = value; 
                    NotifyPropertyChanged(nameof(LanguageId));
                    
                }
            }
        }

        private int _capacity;
        public int Capacity
        {
            get { return _capacity; }
            set 
            { 
                if(_capacity != value)
                {

                    _capacity = value; 
                    NotifyPropertyChanged(nameof(Capacity));
                    
                }
            }
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
        protected override void ValidateSelf()
        {
            Debug.WriteLine(StartTime);
            Debug.WriteLine(DateTime.Now);
            if (string.IsNullOrWhiteSpace(this.Name))
            {
                this.ValidationErrors["Name"] = "Name cannot be empty.";
            }

            if (this.LocationId <= 0)
            {
                this.ValidationErrors[nameof(LocationId)] = "Please select a location.";
            }

            if (this.Duration <= 0)
            {
                this.ValidationErrors[nameof(Duration)] = "Duration must be greater than zero.";
            }

            if (string.IsNullOrWhiteSpace(this.Description))
            {
                this.ValidationErrors["Description"] = "Description cannot be empty.";
            }

            if (this.LanguageId <= 0)
            {
                this.ValidationErrors[nameof(LanguageId)] = "Please select a language.";
            }

            if (this.Capacity <= 0)
            {
                this.ValidationErrors[nameof(Capacity)] = "Capacity must be greater than zero.";
            }
            if (this.StartTime <= DateTime.Now)
            {
                this.ValidationErrors["StartTime"] = "Please select a future date.";
            }

        }

    }
}
