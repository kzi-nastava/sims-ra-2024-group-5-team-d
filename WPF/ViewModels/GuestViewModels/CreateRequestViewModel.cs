using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.WPF.Commands;
using BookingApp.WPF.Views.GuestWindows;
using Jamesnet.Wpf.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;

namespace BookingApp.WPF.ViewModels
{
    public class CreateRequestViewModel: INotifyPropertyChanged
    {
        public ICommand SendRequestCommand { get; private set; }
        private DateTime newReservedFrom = DateTime.UtcNow;
        public DateTime NewReservedFrom
        {
            get { return newReservedFrom; }
            set
            {
                newReservedFrom = value;
                OnPropertyChanged();
            }
        }
        private DateTime newReservedTo = DateTime.UtcNow;
        public DateTime NewReservedTo
        {
            get { return newReservedTo; }
            set
            {
                newReservedTo = value;
                OnPropertyChanged();
            }
        }
        int reservationId { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private NotificationsService notificationsService;
        private AccommodationReservationService reservationService;
        private AccommodationService accommodationService;
        private GuestRequestService guestRequestService;
        NotifierService notifierService;
        public User LoggedInUser {  get; set; }
        MoveReservationAccommodation moveWindow {  get; set; }
        public CreateRequestViewModel(User user, int reservationId, MoveReservationAccommodation moveReservationAccommodation)
        {
            InitializeServices();
            SendRequestCommand = new RelayCommand(SendRequest);
            this.reservationId = reservationId;
            LoggedInUser = user;
            moveWindow = moveReservationAccommodation;
            
        }

        private void InitializeServices()
        {
            guestRequestService = new GuestRequestService();
            notificationsService = new NotificationsService();
            accommodationService = new AccommodationService();
            reservationService = new AccommodationReservationService();
            notifierService = new NotifierService();
        }

        public void SendRequest()
        {
            GuestRequest guestRequest = new GuestRequest(reservationId, NewReservedFrom, NewReservedTo, "" , STATUS.INPROCESS);
            guestRequestService.Save(guestRequest);
            notificationsService.CreateNotification(accommodationService.GetById(reservationService.GetById(reservationId).AccommodationId).Owner.Id,guestRequest.Id,Domain.Models.Type.REQUEST);
            moveWindow.Close();
            GuestWindow.contentControl.Content = new SearchAccommodationUserControl(LoggedInUser);
            notifierService.ShowSuccess("Successfully sent a request!");
        }
    }
}
