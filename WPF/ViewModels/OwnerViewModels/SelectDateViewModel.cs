using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    public class SelectDateViewModel :INotifyPropertyChanged
    {
        public string AccommodationName { get; set; }
        private int daysForRenovation;
        public int DaysForRenovation
        {
            get { return daysForRenovation; }
            set
            {
                daysForRenovation = value;
                OnPropertyChanged();
            }
        }
        private bool showRenovationDates=false;
        public bool ShowRenovationDates
        {
            get { return showRenovationDates; }
            set
            {
                showRenovationDates = value;
                OnPropertyChanged();
            }
        }
        private DateTime renovateFrom = DateTime.UtcNow;
        public DateTime RenovateFrom
        {
            get { return renovateFrom; }
            set
            {
                renovateFrom = value;
                OnPropertyChanged();
            }
        }
        private DateTime renovateTo = DateTime.UtcNow;
        public DateTime RenovateTo
        {
            get { return renovateTo; }
            set
            {
                renovateTo = value;
                OnPropertyChanged();
            }
        }
        private bool showReserve = false;
        public bool ShowReserve
        {
            get { return showReserve; }
            set
            {
                showReserve = value;
                OnPropertyChanged();
            }
        }

        public SelectDateViewModel(string accommodationName)
        {
            AccommodationName = accommodationName;
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}