using BookingApp.Domain.Models;
using BookingApp.WPF.Commands;
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
    public class ComplexRequestViewModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int TouristId { get; set; }
        public STATE Status { get; set; }
        public ICommand ShowMoreCommand { get; set; }
        public int NumberOfRequests { get; set; }
        public int NumberOfAcceptedRequests { get; set; }
        public bool IsAccepted { get; set; }
        public bool IsPending { get; set; }
        public bool IsInvalid { get; set; }

        private bool isMoreClicked;
        public bool IsMoreClicked
        {
            get => isMoreClicked;
            set
            {
                if (value != isMoreClicked)
                {
                    isMoreClicked = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool isMoreNotClicked;
        public bool IsMoreNotClicked
        {
            get => isMoreNotClicked;
            set
            {
                if (value != isMoreNotClicked)
                {
                    isMoreNotClicked = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<SimpleRequestViewModel> SimpleRequests { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ComplexRequestViewModel(ComplexTourRequest req)
        {
            SimpleRequests = new ObservableCollection<SimpleRequestViewModel>();
            IsMoreClicked = false;
            IsMoreNotClicked = true;
            Id = req.Id;
            TouristId = req.TouristId;
            Status = req.Status;
            req.Requests.ForEach(simpleReq =>
            {
                SimpleRequests.Add(new SimpleRequestViewModel(simpleReq));
                if(simpleReq.Status == STATE.ACCEPTED)
                    NumberOfAcceptedRequests++;
            });
            NumberOfRequests = SimpleRequests.Count();
            if(req.Status == STATE.ACCEPTED)
            {
                IsAccepted = true;
                IsPending = false;
                IsInvalid = false;
            }
            else if(req.Status == STATE.PENDING)
            {
                IsPending = true;
                IsInvalid = false;
                IsAccepted = false;
            }
            else
            {
                IsAccepted = false;
                IsPending = false;
                IsInvalid = true;
            }

            ShowMoreCommand = new RelayCommand(ShowMore);
        }

        public void ShowMore()
        {
            IsMoreClicked = !IsMoreClicked;
            IsMoreNotClicked = !IsMoreNotClicked;
        }
    }
}
