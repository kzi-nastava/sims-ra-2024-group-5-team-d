using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    public class AvailableRenovationDateViewModel:INotifyPropertyChanged
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        private bool isSelected=false;
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public AvailableRenovationDateViewModel(DateTime fromDate,DateTime toDate)
        {
            FromDate = fromDate;
            ToDate = toDate;
            IsSelected = false;
        }
    }
}