using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.WPF.Commands;
using BookingApp.WPF.Views.TouristView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels.TourViewModels.TouristViewModels
{
    public class SimpleRequestsViewModel
    {
        public ICommand CreateNewRequestCommand { get; private set; }
        public ICommand ShowStatisticsCommand { get; private set; }
        private User Tourist { get; set; }
        public ObservableCollection<SimpleRequestViewModel> Requests { get; private set; }
        private TourRequestService tourRequestService { get; set; }
        public SimpleRequestsViewModel(User tourist)
        {
            tourRequestService = new TourRequestService();
            Tourist = tourist;
            CreateNewRequestCommand = new RelayCommand(CreateNewRequest);
            ShowStatisticsCommand = new RelayCommand(ShowStatistics);
            Requests = new ObservableCollection<SimpleRequestViewModel>();
            tourRequestService.GetRequestsForTourist(tourist).ForEach(request => Requests.Add(new SimpleRequestViewModel(request)));

        }

        public void CreateNewRequest()
        {
            TouristHomeWindow.contentControl.Content = new CreateRequestUserControl(Tourist);
        }

        public void ShowStatistics() 
        {

        }
    }
}
