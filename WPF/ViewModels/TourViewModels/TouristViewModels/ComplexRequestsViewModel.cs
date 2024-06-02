using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.WPF.Commands;
using BookingApp.WPF.Views.TouristView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels.TourViewModels.TouristViewModels
{
    public class ComplexRequestsViewModel
    {
        public ICommand SimpleRequestsTabCommand { get; set; }

        public ICommand CreateNewRequestCommand { get; set; }
        public User Tourist { get; set; }
        public ObservableCollection<ComplexRequestViewModel> ComplexRequests { get; set; }

        private ComplexTourRequestService complexTourRequestService { get; set; }

        public ComplexRequestsViewModel(User user)
        {
            SimpleRequestsTabCommand = new RelayCommand(SwitchToSimpleRequests);
            CreateNewRequestCommand = new RelayCommand(CreateNewRequest);
            ComplexRequests = new ObservableCollection<ComplexRequestViewModel>();
            complexTourRequestService = new ComplexTourRequestService();
            Tourist = user;
            complexTourRequestService.Validate();
            complexTourRequestService.GetAll(user).ForEach(req =>
            {
                ComplexRequests.Add(new ComplexRequestViewModel(req));
            });
        }

        public void CreateNewRequest()
        {
            TouristHomeWindow.contentControl.Content = new CreateComplexRequestUserControl(Tourist, new ObservableCollection<SimpleRequestViewModel>());
        }

        public void SwitchToSimpleRequests()
        {
            TouristHomeWindow.contentControl.Content = new YourRequestsUserControl(Tourist);
        }


    }
}
