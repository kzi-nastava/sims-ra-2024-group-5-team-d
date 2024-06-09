using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.WPF.Commands;
using BookingApp.WPF.Views.TouristGuide;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels.TourViewModels.TourGuideViewModels
{
    public class ComplexRequestsViewModel : INotifyPropertyChanged
    {
        public User LoggedInUser { get; set; }
        public ICommand ToursTodayTabCommand { get; set; }
        public ICommand RequestTabCommand { get; set; }
        public ICommand FinishedToursTabCommand { get; set; }
        public ICommand AllToursTabCommand { get; set; }
        
        public ObservableCollection<ComplexRequestViewModel> ComplexRequests {  get; set; }
  

        public ComplexTourRequestService complexTourRequestService { get; set; }
        public TourService tourService {  get; set; }
        public ComplexRequestsViewModel(User user) {
            LoggedInUser = user;
            ToursTodayTabCommand = new RelayCommand(ToursTodayTab);
            FinishedToursTabCommand = new RelayCommand(FinishedToursTab);
            RequestTabCommand = new RelayCommand(RequestsTab);
            AllToursTabCommand = new RelayCommand(AllToursTab);
            ComplexRequests = new ObservableCollection<ComplexRequestViewModel>();
            complexTourRequestService = new ComplexTourRequestService();
            tourService = new TourService();
            
            foreach (var req in complexTourRequestService.GetAll())
            {
                ComplexRequestViewModel complex = new ComplexRequestViewModel(req, LoggedInUser);
                if(complex.IsPartAccepted != true && req.Status == STATE.PENDING)
                {
                    ComplexRequests.Add(complex);
                }
            }
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
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
