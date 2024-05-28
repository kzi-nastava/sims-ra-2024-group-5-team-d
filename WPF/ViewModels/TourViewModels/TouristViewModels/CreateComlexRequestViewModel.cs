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
    public class CreateComlexRequestViewModel
    {
        public ICommand SimpleRequestsTabCommand { get; private set; }

        public User Tourist { get; set; }

        public ObservableCollection<SimpleRequestViewModel> SimpleRequests { get; private set; }


        public CreateComlexRequestViewModel(User tourist) 
        {
            Tourist = tourist;
            SimpleRequestsTabCommand = new RelayCommand(ShowSimpleRequests);
            SimpleRequests = new ObservableCollection<SimpleRequestViewModel>();
            SimpleRequests.Add(new SimpleRequestViewModel(Tourist.Id));
        }

        

        public void ShowSimpleRequests()
        {
            TouristHomeWindow.contentControl.Content = new YourRequestsUserControl(Tourist);
        }
    }
}
