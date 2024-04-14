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
        public int Id { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public int Cleanliness { get; set; }
        public int RuleCompliance { get; set; }

        private AccommodationReservationService accommodationReservationService;
        private GuestRatingService guestRatingService;
        private UnratedGuestViewModel unratedGuest;
        public RateGuestViewModel()
        {
        }
        public RateGuestViewModel(UnratedGuestViewModel unratedGuest)
        {
            accommodationReservationService = new AccommodationReservationService();
            guestRatingService = new GuestRatingService();
            CleanlinessRatingCommand = new RelayParameterCommand(GetCleanlinessRating);
            RuleComplianceRatingCommand = new RelayParameterCommand(GetRuleComplianceRating);
            RateGuestCommand = new RelayCommand(RateGuest);
            this.unratedGuest = unratedGuest;
            Cleanliness = 1;
            RuleCompliance = 1;

            Id = unratedGuest.Id;
            Name = unratedGuest.FullName;
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
            guestRatingService.Save(new GuestRating(accommodationReservationService.GetById(Id).AccommodationId, accommodationReservationService.GetById(Id).UserId, Id, Cleanliness, RuleCompliance, Comment));
            UnratedGuestsViewModel.UnratedGuests.Remove(unratedGuest);
        }
    }
}
