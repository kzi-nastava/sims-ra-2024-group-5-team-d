using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.Commands;
using BookingApp.WPF.Views.TouristView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private bool isEmpty;
        public bool IsEmpty
        {
            get => isEmpty;
            set
            {
                if (value != isEmpty)
                {
                    isEmpty = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool isNotEmpty;
        public bool IsNotEmpty
        {
            get => isNotEmpty;
            set
            {
                if (value != isNotEmpty)
                {
                    isNotEmpty = value;
                    OnPropertyChanged();
                }
            }
        }
        public Location Location { get; set; }
        public string LocationName { get; set; }
        public LANGUAGE Language { get; set; }
        public STATE Status { get; set; }
        public DateTime RangeFrom { get; set; }
        public DateTime RangeTo { get; set; }
        public DateTime StartTime { get; set; }
        public string Description { get; set; }
        public int TouristId { get; set; }
        public int ReservationId { get; set; }
        public bool IsAccepted { get; set; }
        public bool IsPending { get; set; }
        public bool IsInvalid { get; set; }
        public bool NotAccepted { get; set; }
        public int Capacity { get; set; }
        public ICommand Cancel { get; private set; }
        public ObservableCollection<TourAttendeeViewModel> Attendees { get; set; }
        public ObservableCollection<TourGuestViewModel> Tourists { get; set; }
        private TourReservationService tourReservationService { get; set; }
        private TourRealisationService tourRealisationService { get; set; }
        private TourGuestService tourGuestService { get; set; }
        private LocationService locationService { get; set; }
        

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SimpleRequestViewModel(int touristId)
        {
            Id = -1;
            TouristId = touristId;
            IsEmpty = true;
            IsNotEmpty = false;
            Attendees = new ObservableCollection<TourAttendeeViewModel>();
            Tourists = new ObservableCollection<TourGuestViewModel>();
        }
        public SimpleRequestViewModel(TourRequest tourRequest)
        {
            Id = tourRequest.Id;
            IsEmpty = false;
            IsNotEmpty = true;
            Language = tourRequest.Language;
            Status = tourRequest.Status;
            tourReservationService = new TourReservationService();
            tourRealisationService = new TourRealisationService();
            tourGuestService = new TourGuestService();
            locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            Location = locationService.GetById(tourRequest.Location.Id);
            LocationName = $"{Location.City}, {Location.Country}";
            Description = tourRequest.Description;
            TouristId = tourRequest.TouristId;
            ReservationId = tourRequest.TourReservationId;
            Capacity = tourRequest.Capacity;
            Cancel = new RelayCommand(CancelRequest);
            if(tourRequest.Status == STATE.PENDING)
            {
                IsPending = true;
                IsAccepted = false;
                IsInvalid = false;
                RangeFrom = tourRequest.RangeFrom;
                RangeTo = tourRequest.RangeTo;
                NotAccepted = true;
            }
            else if(tourRequest.Status == STATE.ACCEPTED)
            {
                IsPending = false;
                IsAccepted = true;
                NotAccepted = false;
                IsInvalid = false;
                StartTime = tourRealisationService.GetById(tourReservationService.GetById(tourRequest.TourReservationId).TourRealisationId).StartTime;
            }
            else
            {
                IsPending = false;
                IsAccepted = false;
                IsInvalid = true;
                RangeFrom = tourRequest.RangeFrom;
                RangeTo = tourRequest.RangeTo;
                NotAccepted = true;
            }
            Attendees = new ObservableCollection<TourAttendeeViewModel>();
            Tourists = new ObservableCollection<TourGuestViewModel>();
            tourGuestService.GetAllTourGuests().ForEach(tG => {
                if (tG.TourReservationId == ReservationId)
                    Attendees.Add(new TourAttendeeViewModel(tG.Id, tG.FullName)); });

        }

        public void CancelRequest()
        {

        }
    }
}
