using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using BookingApp.WPF.Commands;
using BookingApp.WPF.Views.TouristGuide;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels.TourViewModels.TourGuideViewModels
{
    public class RequestsViewModel : INotifyPropertyChanged
    {
        public User LoggedInUser { get; set; }
        public ICommand AcceptCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand StatisticsCommand { get; set; }
        public ICommand ToursTodayTabCommand { get; set; }
        public ICommand RequestTabCommand { get; set; }
        public ICommand ComplexRequestTabCommand { get; set; }
        public ICommand FinishedToursTabCommand { get; set; }
        public ICommand AllToursTabCommand { get; set; }
        public ObservableCollection<RequestViewModel> TourRequests { get ; set; }
        public RequestViewModel SelectedTourRequest { get; set; }
        public TourRequestService tourRequestService { get; set; }
        public LocationService locationService { get; set; }
        public UserService userService {  get; set; }

        public RequestsViewModel(User user) 
        { 
            LoggedInUser = user;
            tourRequestService = new TourRequestService();
            tourRequestService.Validate();
            userService = new UserService();
            ToursTodayTabCommand = new RelayCommand(ToursTodayTab);
            FinishedToursTabCommand = new RelayCommand(FinishedToursTab);
            RequestTabCommand = new RelayCommand(RequestsTab);
            AllToursTabCommand = new RelayCommand(AllToursTab);
            ComplexRequestTabCommand = new RelayCommand(ComplexRequestsTab);
            SearchCommand = new RelayCommand(Search);
            StatisticsCommand = new RelayCommand(Statistics);
            AcceptCommand = new RelayParameterCommand(Accept);
            TourRequests = new ObservableCollection<RequestViewModel>();
            locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            Search();
        }

        private int pickedLocationId = 10;

        public int PickedLocationId
        {
            get => pickedLocationId;
            set
            {
                if (value != pickedLocationId)
                {
                    pickedLocationId = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime pickedDateFrom = DateTime.Now;

        public DateTime PickedDateFrom
        {
            get => pickedDateFrom;
            set
            {
                if (value != pickedDateFrom)
                {
                    pickedDateFrom = value;
                    OnPropertyChanged();
                    

                }
            }
        }
        private DateTime pickedDateTo = DateTime.Now;

        public DateTime PickedDateTo
        {
            get => pickedDateTo;
            set
            {
                if (value != pickedDateTo)
                {
                    pickedDateTo = value;
                    OnPropertyChanged();
                    

                }
            }
        }


        private int pickedLanguage = 3;

        public int PickedLanguage
        {
            get => pickedLanguage;
            set
            {
                if (value != pickedLanguage)
                {
                    pickedLanguage = value;
                    OnPropertyChanged();
                }
            }
        }

        private int pickedMaxCapacity = 1;

        public int PickedMaxCapacity
        {
            get => pickedMaxCapacity;
            set
            {
                if (value != pickedMaxCapacity)
                {
                    pickedMaxCapacity = value;
                    OnPropertyChanged(nameof(PickedMaxCapacity));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Search()
        {
            TourRequests.Clear();

            foreach (TourRequest tourRequest in tourRequestService.GetAll())
            {
                bool languageMatch = pickedLanguage == 3 || tourRequest.Language == (LANGUAGE)pickedLanguage;
                bool durationMatch = (pickedDateFrom.DayOfYear == DateTime.Now.DayOfYear && pickedDateTo.DayOfYear == DateTime.Now.DayOfYear) || (tourRequest.RangeFrom >= pickedDateFrom && tourRequest.RangeTo <= pickedDateTo); //PROVERI AKO NE IZABERE DATUM DA BUDE TRUE SVAKAKO
                bool locationMatch = pickedLocationId == 10 || tourRequest.Location.Id == pickedLocationId;
                bool capacityMatch = tourRequest.Capacity >= Convert.ToInt32(pickedMaxCapacity);
                if (languageMatch && durationMatch && locationMatch && capacityMatch && tourRequest.Status == STATE.PENDING)
                {
                    TourRequests.Add(new RequestViewModel(tourRequest.Id,tourRequest.Description, locationService.GetById(tourRequest.Location.Id), tourRequest.RangeFrom, tourRequest.RangeTo, tourRequest.Capacity,tourRequest.Language,userService.GetById( tourRequest.TouristId),tourRequest.IsAcceptable()));
                }
            }
        }
        public void Accept(object parameter)
        {   
            if(parameter is RequestViewModel request)
                SideBar.contentControlW.Content = new CreateNewTourForm(LoggedInUser,request);
        }

        private void AllToursTab()
        {
            SideBar.contentControlW.Content = new AllToursWindow(LoggedInUser);
        }
        private void ToursTodayTab()
        {
            SideBar.contentControlW.Content = new ToursTodayWindow(LoggedInUser);
        }
        private void FinishedToursTab()
        {
            SideBar.contentControlW.Content = new FinishedToursWindow(LoggedInUser);
        }
        private void RequestsTab()
        {
            SideBar.contentControlW.Content = new RequestsWindow(LoggedInUser);
        }
        private void ComplexRequestsTab()
        {
            SideBar.contentControlW.Content = new ComplexRequestsWindow(LoggedInUser);
        }
        private void Statistics()
        {
            SideBar.contentControlW.Content = new RequestsStatistics(LoggedInUser);
        }
    }
}
