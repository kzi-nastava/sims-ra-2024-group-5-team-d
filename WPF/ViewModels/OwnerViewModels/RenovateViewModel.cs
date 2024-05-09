using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.WPF.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    public class RenovateViewModel
    {
        private User loggedInUser;
        public ICommand ReserveRenovationCommand { get; set; }

        public ICommand ShowAvailableDatesCommand { get; set; }
        public SelectDateViewModel SelectDate { get; set; }
        private AccommodationService accommodationService;
        public ObservableCollection<AvailableRenovationDateViewModel> AvailableDates { get; set; }
        private AccommodationRenovationService accommodationRenovationService;
        public AvailableRenovationDateViewModel SelectedAvailableDate { get; set; }
        private int accommodationId;
        private NotifierService notifierService;
        private AvailableDatesForReservationService availableDatesForReservationService;
        public RenovateViewModel(User user,int accommodationId)
        {
            notifierService = new NotifierService();
            availableDatesForReservationService = new AvailableDatesForReservationService();
            this.accommodationId = accommodationId;
            accommodationRenovationService = new AccommodationRenovationService();
            AvailableDates = new ObservableCollection<AvailableRenovationDateViewModel>();
            accommodationService = new AccommodationService();
            SelectDate = new SelectDateViewModel(accommodationService.GetAccommodationNameById(accommodationId));
            loggedInUser = user;
            ShowAvailableDatesCommand = new RelayCommand(ShowAvailableDates);
            ReserveRenovationCommand = new RelayCommand(ReserveRenovation);
        }
        private void ShowAvailableDates()
        {
            AvailableDates.Clear();
            List<KeyValuePair<DateTime, DateTime>> availableDatesForRenovations = availableDatesForReservationService.CheckAvailableDatesInGivenRange(SelectDate.RenovateFrom, SelectDate.RenovateTo, SelectDate.DaysForRenovation, accommodationService.GetById(accommodationId));
            availableDatesForRenovations.ForEach(date => AvailableDates.Add(new AvailableRenovationDateViewModel(date.Key, date.Value)));
            SelectDate.ShowRenovationDates = true;  

        }
        private void ReserveRenovation()
        {
            if (SelectedAvailableDate != null)
            {
                notifierService.ShowSuccess("Renovation reserved successfully");
                SelectDate.ShowRenovationDates = false;
                accommodationRenovationService.Save(new AccommodationRenovation(accommodationId, SelectedAvailableDate.FromDate, SelectedAvailableDate.ToDate));
            }
        }
    }
}
