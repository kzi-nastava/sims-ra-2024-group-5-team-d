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

        public ObservableCollection<SimpleRequestViewModel> SimpleRequests { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ComplexRequestViewModel(ComplexTourRequest req)
        {
            Debug.WriteLine("HEJ SLOVENI");
            SimpleRequests = new ObservableCollection<SimpleRequestViewModel>();
            IsMoreClicked = false;
            Id = req.Id;
            TouristId = req.TouristId;
            Status = req.Status;
            req.Requests.ForEach(simpleReq =>
            {
                Debug.WriteLine("JOS SE ZIVii" + simpleReq.Id);
                SimpleRequests.Add(new SimpleRequestViewModel(simpleReq));
                if(simpleReq.Status == STATE.ACCEPTED)
                    NumberOfAcceptedRequests++;
            });
            NumberOfRequests = SimpleRequests.Count();
            Debug.WriteLine("ides za KANADU" + SimpleRequests.Count());

            ShowMoreCommand = new RelayCommand(ShowMore);
        }

        public void ShowMore()
        {
            IsMoreClicked = !IsMoreClicked;
        }
    }
}
