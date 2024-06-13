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

namespace BookingApp.WPF.ViewModels.TourViewModels.TouristViewModels
{
    public class CreateSimpleRequestViewModel : INotifyPropertyChanged
    {
        public ICommand SimpleRequestsTabCommand { get; private set; }
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
        private User Tourist { get; set; }

        public ObservableCollection<TourGuestViewModel> Tourists { get; set; }

        private TourGuestService tourGuestService { get; set; }
        private NotifierService notifier;
        private UserService userService { get; set; }
        private LocationService locationService { get; set; }

        public int Index { get; set; }
        public ObservableCollection<SimpleRequestViewModel> SimpleRequests { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CreateSimpleRequestViewModel(int index, SimpleRequestViewModel request, ObservableCollection<SimpleRequestViewModel> requests)
        {
            userService = new UserService();
            IncreaseCountCommand = new RelayCommand(IncreaseCount);
            DecreaseCountCommand = new RelayCommand(DecreaseCount);
            CreateRequestCommand = new RelayCommand(CreateRequest);
            CancelCommand = new RelayCommand(Cancel);

            SimpleRequestsTabCommand = new RelayCommand(ShowSimpleRequests);
            tourGuestService = new TourGuestService();
            notifier = new NotifierService();
            locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            Tourists = new ObservableCollection<TourGuestViewModel>();
            Index = index;
            Tourist = userService.GetById(request.TouristId);
            SimpleRequests = requests;
            Capacity = request.Capacity;
            RangeFrom = request.RangeFrom;
            RangeTo = request.RangeTo;
            Location = request.Location.Id;
            Language = (int)request.Language;
            Description = request.Description;
            Tourists = request.Tourists;
        }

        public CreateSimpleRequestViewModel(User tourist, ObservableCollection<SimpleRequestViewModel> requests)
        {
            Index = -1;
            Tourist = tourist;
            SimpleRequests = requests;
            Capacity = 1;
            IncreaseCountCommand = new RelayCommand(IncreaseCount);
            DecreaseCountCommand = new RelayCommand(DecreaseCount);
            CreateRequestCommand = new RelayCommand(CreateRequest);
            CancelCommand = new RelayCommand(Cancel);

            SimpleRequestsTabCommand = new RelayCommand(ShowSimpleRequests);
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
        }

        public void ShowSimpleRequests()
        {
            TouristHomeWindow.contentControl.Content = new YourRequestsUserControl(Tourist);
        }

        public void IncreaseCount()
        {
            Capacity++;
            Tourists.Add(new TourGuestViewModel(tourGuestService.NextIdForGuest() + (Capacity - 1), -1, $"Tourist {Capacity}"));
            Tourists[Capacity - 1].IsReadOnly = false;
        }

        public void DecreaseCount()
        {
            if (Capacity > 1)
            {
                Capacity--;
                Tourists.RemoveAt(Capacity);
            }
        }

        public void CreateRequest()
        {
            SimpleRequestViewModel request = new SimpleRequestViewModel(new TourRequest(-1, Tourist.Id, STATE.PENDING, locationService.GetById(Location), Description, (LANGUAGE)Language, RangeFrom, RangeTo, -1, Tourists.Count));
            request.Tourists = Tourists;
            if(Index == -1)
            {
                SimpleRequests.Add(request);
                notifier.ShowSuccess("Simple request added.");
            }
            else
            {
                SimpleRequests[Index] = request;
                notifier.ShowSuccess("Simple request updated.");
            }
            TouristHomeWindow.contentControl.Content = new CreateComplexRequestUserControl(Tourist, SimpleRequests);
        }

        public void Cancel()
        {
            TouristHomeWindow.contentControl.Content = new CreateComplexRequestUserControl(Tourist,SimpleRequests);
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
