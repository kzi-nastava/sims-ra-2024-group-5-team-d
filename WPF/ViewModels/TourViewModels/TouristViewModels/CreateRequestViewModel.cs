using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.Commands;
using BookingApp.WPF.Views.TouristView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ToastNotifications;

namespace BookingApp.WPF.ViewModels.TourViewModels.TouristViewModels
{
    public class CreateRequestViewModel : INotifyPropertyChanged
    {
        public ICommand ComplexRequestsTabCommand { get; private set; }
        public int Id { get; set; }

        private int capacity;
        public int Capacity
        {
            get => capacity;
            set
            {
                if (value != capacity)
                {
                    capacity = value;
                    OnPropertyChanged();
                }
            }
        }
        public ICommand IncreaseCountCommand { get; private set; }
        public ICommand DecreaseCountCommand { get; private set; }
        public ICommand CreateRequestCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        private DateTime rangeFrom;
        public DateTime RangeFrom
        {
            get => rangeFrom;
            set
            {
                if (value != rangeFrom)
                {                    
                    rangeFrom = value;
                    RangeTo = RangeFrom;
                    OnPropertyChanged();
                }
            }
        }
        private DateTime rangeTo;
        public DateTime RangeTo
        {
            get => rangeTo;
            set
            {
                if (value != rangeTo)
                {
                    rangeTo = value;
                    OnPropertyChanged();
                }
            }
        }
        public DateTime MinAvailDate { get; set; }
        public int Location { get; set; }
        public int Language { get; set; }
        public string Description { get; set; }
        private STATE Status { get; set; }
        private User Tourist { get; set; }

        public bool IsReadOnly { get; set; }
        public ObservableCollection<TourGuestViewModel> Tourists { get; set; }

        private TourGuestService tourGuestService { get; set; }
        private NotifierService notifier;
        private LocationService locationService { get; set; }
        private TourRequestService tourRequestService { get; set; }
        private TourReservationService tourReservationService { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CreateRequestViewModel(User tourist)
        {
            Tourist = tourist;
            Capacity = 1;
            IncreaseCountCommand = new RelayCommand(IncreaseCount);
            DecreaseCountCommand = new RelayCommand(DecreaseCount);
            CreateRequestCommand = new RelayCommand(CreateRequest);
            CancelCommand = new RelayCommand(Cancel);

            ComplexRequestsTabCommand = new RelayCommand(ShowComplexRequests);
            tourGuestService = new TourGuestService();
            notifier = new NotifierService();
            locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            Tourists = new ObservableCollection<TourGuestViewModel>();
            CreateTouristsFormular();
            Language = 0;
            Location = 0;
            MinAvailDate = (DateTime.UtcNow).AddDays(3);
            RangeFrom = (DateTime.UtcNow).AddDays(3);
            RangeTo = (DateTime.UtcNow).AddDays(3);
            tourRequestService = new TourRequestService();
            tourReservationService = new TourReservationService();
        }

        public void ShowComplexRequests()
        {
            TouristHomeWindow.contentControl.Content = new ComplexRequestsUserControl(Tourist);
        }

        public void IncreaseCount()
        {
            Capacity++;
            Tourists.Add(new TourGuestViewModel(tourGuestService.NextIdForGuest() + (Capacity - 1), -1, $"Tourist {Capacity}"));
            Tourists[Capacity - 1].IsReadOnly = false;
        }

        public void DecreaseCount()
        {
            if(Capacity > 1)
            {
                Capacity--;
                Tourists.RemoveAt(Capacity);
            }
        }

        public void CreateRequest()
        {
            TourReservation tourReservation = tourReservationService.Save(new TourReservation(tourReservationService.NextId(),-1,Tourist));
            foreach (TourGuestViewModel tG in Tourists)
            {
                tourGuestService.Save(new TourGuest(tG.Id, tG.FullName, tG.Years, tourReservation.Id, -1, tG.PersonalID));
            }
            tourRequestService.Save(new TourRequest(tourGuestService.NextIdForGuest(), Tourist.Id, STATE.PENDING, locationService.GetById(Location), Description, (LANGUAGE)Language, RangeFrom, RangeTo, tourReservation.Id,Tourists.Count));
            notifier.ShowSuccess("Reqest created successfully");
            TouristHomeWindow.contentControl.Content = new YourRequestsUserControl(Tourist);
        }

        public void Cancel()
        {
            TouristHomeWindow.contentControl.Content = new YourRequestsUserControl(Tourist);
        }

        public void CreateTouristsFormular()
        {
            for (int i = 0; i < Capacity; i++)
            {
                Tourists.Add(new TourGuestViewModel(tourGuestService.NextIdForGuest(), -1, $"Tourist {i + 1}"));
            }
            FillUserInfo();
        }
        public void FillUserInfo()
        {
            int age = DateTime.Today.Year - Tourist.BirthDate.Year;

            if (Tourist.BirthDate > DateOnly.FromDateTime(DateTime.Today.AddYears(-age)))
            {
                age--;
            }

            Tourists[0].Years = age;
            Tourists[0].PersonalID = Tourist.PersonalId;
            Tourists[0].FullName = Tourist.FullName;
            Tourists[0].IsReadOnly = true;
        }
    }
}
