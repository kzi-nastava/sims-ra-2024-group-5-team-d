using BookingApp.Domain.Models;
using BookingApp.WPF.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels.TourViewModels.TouristViewModels
{
    public class SimpleRequestViewModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public Location Location { get; set; }
        public LANGUAGE Language { get; set; }
        public STATE Status { get; set; }
        public DateTime RangeFrom { get; set; }
        public DateTime RangeTo { get; set; }
        public string Description { get; set; }
        public int TouristId { get; set; }
        public bool IsAccepted { get; set; }
        public bool IsPending { get; set; }
        public bool IsInvalid { get; set; }
        public ICommand Cancel { get; private set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SimpleRequestViewModel(TourRequest tourRequest)
        {
            Id = tourRequest.Id;
            Location = tourRequest.Location;
            Language = tourRequest.Language;
            Status = tourRequest.Status;
            RangeFrom = tourRequest.RangeFrom;
            RangeTo = tourRequest.RangeTo;
            Description = tourRequest.Description;
            TouristId = tourRequest.TouristId;
            Cancel = new RelayCommand(CancelRequest);
        }

        public void CancelRequest()
        {

        }
    }
}
