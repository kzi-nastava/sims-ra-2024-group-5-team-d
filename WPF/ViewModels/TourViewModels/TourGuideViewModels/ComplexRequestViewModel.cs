using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.Commands;
using BookingApp.WPF.Views.TouristGuide;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels.TourViewModels.TourGuideViewModels
{
    public class ComplexRequestViewModel
    {
        public User Tourist { get; set; }
        public ICommand AcceptCommand { get; set; }
        public ObservableCollection<RequestViewModel> SimpleRequests { get; set; }
        public UserService userService { get; set; }
        public LocationService locationService { get; set; }
        public User LoggedInUser {  get; set; }
        public bool IsPartAccepted {  get; set; }
        public TourReservationService tourReservationService {  get; set; }
        public TourRealisationService tourRealisationService { get; set; }
        
        public ComplexRequestViewModel(ComplexTourRequest complex,User user) 
        {
            IsPartAccepted = false;
            LoggedInUser = user;
            userService = new UserService();
            locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            tourRealisationService = new TourRealisationService();
            tourReservationService = new TourReservationService();
            SimpleRequests = new ObservableCollection<RequestViewModel>();
            Tourist = userService.GetById( complex.TouristId);
            AcceptCommand = new RelayParameterCommand(Accept);
            foreach (var s in complex.Requests)
            {
                if(tourReservationService.GetById(s.TourReservationId).TourRealisationId != -1)
                {
                    if(tourRealisationService.GetById(tourReservationService.GetById(s.TourReservationId).TourRealisationId).User.Id == LoggedInUser.Id)
                    {
                        IsPartAccepted = true;
                    }
                }
                RequestViewModel req = new RequestViewModel(s.Id, s.Description, locationService.GetById(s.Location.Id), s.RangeFrom, s.RangeTo, s.Capacity, s.Language, userService.GetById(s.TouristId), s.IsAcceptableInsideComplex());
                req.GetRangeOfDates(user);
                SimpleRequests.Add(req);
            }
            
        }
        public void Accept(object parameter)
        {
            if(parameter is RequestViewModel request)
            {
                Debug.WriteLine(request.SelectedTime);
                if (request.SelectedTime != TimeOnly.MinValue)
                    SideBar.contentControlW.Content = new CreateNewTourForm(LoggedInUser, request);

            }

        }
    }
}
