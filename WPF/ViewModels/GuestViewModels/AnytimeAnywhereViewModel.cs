using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Repositories;
using BookingApp.WPF.Views.GuestWindows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using BookingApp.WPF.Commands;
using Xceed.Wpf.Toolkit.Primitives;
using System.ComponentModel;
using System.Diagnostics;

namespace BookingApp.WPF.ViewModels.GuestViewModels
{
    public class AnytimeAnywhereViewModel: INotifyPropertyChanged
    {
        public ICommand SelectionChangedCommand { get; set; }
        public ICommand ReserveCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public User LoggedInUser;
        public ObservableCollection<KeyValuePair<DateTime, DateTime>> AvailableDates { get; set; }
        public KeyValuePair<DateTime, DateTime> SelectedDate { get; set; }
        private AvailableDatesForReservationService availableDatesForReservationService;
        private AccommodationService accommodationService;
        private int accommodationId;
        private AccommodationReservationService accommodationReservationService;
        private SuperUserService superUserService;

        public int NumberOfPeople { get; set; }

        private bool reserveEnable= false;
        public bool ReserveEnable
        {
            get => reserveEnable;
            set
            {
                if (value != reserveEnable)
                {
                    reserveEnable = value;
                    OnPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public AnytimeAnywhereViewModel(User user, AccommodationViewModel selectedAccommmodation, DateTime fromDate, DateTime toDate, int numberOfPeople, int numberOfDays)
        {
            LoggedInUser = user;
            NumberOfPeople = numberOfPeople;
            accommodationId = selectedAccommmodation.Id;
            accommodationService = new AccommodationService();
            accommodationReservationService = new AccommodationReservationService();
            superUserService = new SuperUserService();
            availableDatesForReservationService = new AvailableDatesForReservationService();
            AvailableDates = new ObservableCollection<KeyValuePair<DateTime, DateTime>>();
            SelectionChangedCommand = new RelayParameterCommand(DataGridSelectionChanged);
            ReserveCommand = new RelayCommand(ReserveAccommodation);
            CancelCommand = new RelayCommand(Cancel);

            availableDatesForReservationService.GetAvailableDatesInGivenRange(fromDate, toDate, numberOfDays, accommodationService.GetById(selectedAccommmodation.Id))
                .ForEach(date =>AvailableDates.Add(date));
            if(AvailableDates.Count()==0)
            {
                availableDatesForReservationService.FindandShowAvailableDatesForExtendendRange(fromDate, toDate, numberOfDays, accommodationService.GetById(selectedAccommmodation.Id))
                    .ForEach(date => AvailableDates.Add(date));

            }

        }
        public void DataGridSelectionChanged(object parameter)
        {
            if (parameter != null)
            {
                EnableReserveButton();
            }
        }

        private void EnableReserveButton()
        {
            bool isAvailableDateSelected = SelectedDate.Key != null && SelectedDate.Value != null;
                ReserveEnable = true;
        }
        private void ReserveAccommodation()
        {
            Debug.WriteLine("nije vidljinbov");
            AccommodationReservation newReservation = new AccommodationReservation(accommodationId, LoggedInUser.Id, SelectedDate.Key, SelectedDate.Value, NumberOfPeople);
            AccommodationReservation savedAccommodation = accommodationReservationService.Save(newReservation);
            if (LoggedInUser.IsSuper())
            {
                superUserService.IsDiscountUsed(LoggedInUser);
            }

            GuestWindow.contentControl.Content = new SearchAccommodationUserControl(LoggedInUser);
        }
        private void Cancel()
        {
            GuestWindow.contentControl.Content = new SearchAccommodationUserControl(LoggedInUser);
        }

    }
}
