using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using BookingApp.WPF.Commands;
using BookingApp.WPF.Views.OwnerView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    public class RateGuestViewModel
    {
        public ICommand RateGuestCommand { get; set; }
        public ICommand RuleComplianceRatingCommand { get; set; }
        public ICommand CleanlinessRatingCommand { get; set; }

        private AccommodationReservationService accommodationReservationService;
        private GuestRatingService guestRatingService;
        private NotificationsService notificationsService;

        public int ReservationId { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public int Cleanliness { get; set; }
        public int RuleCompliance { get; set; }
        private UnratedGuestViewModel unratedGuest;
        public RateGuestViewModel()
        {
        }
        public RateGuestViewModel(UnratedGuestViewModel unratedGuest)
        {
            InitializeServices();
            CleanlinessRatingCommand = new RelayParameterCommand(GetCleanlinessRating);
            RuleComplianceRatingCommand = new RelayParameterCommand(GetRuleComplianceRating);
            RateGuestCommand = new RelayCommand(RateGuest);
            this.unratedGuest = unratedGuest;
            Cleanliness = 1;
            RuleCompliance = 1;

            ReservationId = unratedGuest.ReservationId;
            Name = unratedGuest.FullName;
        }

        private void InitializeServices()
        {
            notificationsService = new NotificationsService(Injector.CreateInstance<INotificationRepository>());
            accommodationReservationService = new AccommodationReservationService(Injector.CreateInstance<IAccommodationReservationRepository>());
            guestRatingService = new GuestRatingService(Injector.CreateInstance<IGuestRatingRepository>());
        }

        private void GetCleanlinessRating(object parameter)
        {
            Cleanliness = int.Parse(parameter.ToString());
        }
        private void GetRuleComplianceRating(object parameter)
        {
            RuleCompliance = int.Parse(parameter.ToString());
        }
        private void RateGuest()
        {
            guestRatingService.Save(new GuestRating(accommodationReservationService.GetById(ReservationId).AccommodationId, accommodationReservationService.GetById(ReservationId).UserId, ReservationId, Cleanliness, RuleCompliance, Comment));
            UnratedGuestsViewModel.UnratedGuests.Remove(unratedGuest);
            notificationsService.RemoveNotification(ReservationId);
        }
    }
}
