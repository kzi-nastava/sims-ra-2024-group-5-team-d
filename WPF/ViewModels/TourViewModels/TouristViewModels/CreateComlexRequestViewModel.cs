using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.Commands;
using BookingApp.WPF.Views.TouristView;
using HarfBuzzSharp;
using iText.Layout.Font;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ToastNotifications;

namespace BookingApp.WPF.ViewModels.TourViewModels.TouristViewModels
{
    public class CreateComlexRequestViewModel
    {
        public ICommand SimpleRequestsTabCommand { get; private set; }
        public ICommand CreateComplexRequestCommand { get; private set; }
        public ICommand CancelComplexRequestCommand { get; private set; }

        public ICommand AddMoreCommand { get; private set; }
        public ICommand RemoveRequestCommand { get; private set; }
        public ICommand EditRequestCommand { get; private set; }
        public User Tourist { get; set; }
        private TourGuestService tourGuestService { get; set; }
        private NotifierService notifier;
        private LocationService locationService { get; set; }
        private TourRequestService tourRequestService { get; set; }
        private TourReservationService tourReservationService { get; set; }
        private ComplexTourRequestService complexTourRequestService { get; set; }
        private ComplexSimpleRequestPairService complexSimpleRequestPairService { get; set; }
        public ObservableCollection<SimpleRequestViewModel> SimpleRequests { get; private set; }


        public CreateComlexRequestViewModel(User tourist, ObservableCollection<SimpleRequestViewModel> requests)
        {
            tourGuestService = new TourGuestService();
            notifier = new NotifierService();
            locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            tourReservationService = new TourReservationService();
            tourRequestService = new TourRequestService();
            complexTourRequestService = new ComplexTourRequestService();
            complexSimpleRequestPairService = new ComplexSimpleRequestPairService();

            Tourist = tourist;
            SimpleRequestsTabCommand = new RelayCommand(ShowSimpleRequests);
            EditRequestCommand = new RelayParameterCommand(EditExecute);
            RemoveRequestCommand = new RelayParameterCommand(DeleteExecute);
            CancelComplexRequestCommand = new RelayCommand(CancelComplexRequest);
            CreateComplexRequestCommand = new RelayCommand(CreateComplexRequest);
            SimpleRequests = requests;
            if(SimpleRequests.All(req => req.IsNotEmpty) && SimpleRequests.Count < 6)
                SimpleRequests.Add(new SimpleRequestViewModel(Tourist.Id));
            AddMoreCommand = new RelayCommand(AddMore);
        }

        public void CreateComplexRequest()
        {
            if(SimpleRequests.Any(req => req.IsEmpty))
                SimpleRequests.RemoveAt(SimpleRequests.Count - 1);

            if(SimpleRequests.Count > 0)
            {
                foreach(SimpleRequestViewModel req in SimpleRequests)
                {
                    TourReservation tourReservation = tourReservationService.Save(new TourReservation(tourReservationService.NextId(), -1, Tourist));
                    foreach (TourGuestViewModel tG in req.Tourists)
                    {
                        tourGuestService.Save(new TourGuest(tG.Id, tG.FullName, tG.Years, tourReservation.Id, -1, tG.PersonalID));

                    }
                    TourRequest simpleRequest = tourRequestService.Save(new TourRequest(tourGuestService.NextIdForGuest(), Tourist.Id, STATE.PENDING, locationService.GetById(req.Location.Id), req.Description, req.Language, req.RangeFrom, req.RangeTo, tourReservation.Id, req.Tourists.Count));
                    complexSimpleRequestPairService.Save(new ComplexSimpleRequestPair(complexTourRequestService.NextId(), simpleRequest.Id));
                }
                complexTourRequestService.Save(new ComplexTourRequest(complexTourRequestService.NextId(), STATE.PENDING, Tourist.Id));
                notifier.ShowSuccess("Complex reqest created successfully");
                TouristHomeWindow.contentControl.Content = new ComplexRequestsUserControl(Tourist);
            }
            else
            {
                TouristHomeWindow.contentControl.Content = new ComplexRequestsUserControl(Tourist);
                notifier.ShowError("Request not created since it contains 0 simple requests");
            }
        }
        public void CancelComplexRequest()
        {
            TouristHomeWindow.contentControl.Content = new ComplexRequestsUserControl(Tourist);
        }


        private void EditExecute(object parameter)
        {
            if (parameter is SimpleRequestViewModel request)
            {
                int index = SimpleRequests.IndexOf(request);
                TouristHomeWindow.contentControl.Content = new CreateSimpleRequestUserControl(index, request, SimpleRequests);
            }
        }

        private void DeleteExecute(object parameter)
        {
            if (parameter is SimpleRequestViewModel request)
            {
                SimpleRequests.Remove(request);
                if (SimpleRequests.Count == 5 && SimpleRequests.All(req => req.IsNotEmpty))
                    SimpleRequests.Add(new SimpleRequestViewModel(Tourist.Id));
            }
        }

        public void AddMore()
        {
            SimpleRequests.RemoveAt(SimpleRequests.Count - 1);
            TouristHomeWindow.contentControl.Content = new CreateSimpleRequestUserControl(Tourist, SimpleRequests);
        }

        public void ShowSimpleRequests()
        {
            TouristHomeWindow.contentControl.Content = new YourRequestsUserControl(Tourist);
        }
    }
}
