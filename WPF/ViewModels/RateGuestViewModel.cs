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

namespace BookingApp.WPF.ViewModels
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

        private IAccommodationReservationRepository accommodationReservationRepository;
        private IGuestRatingRepository guestRatingRepository;
        private UnratedGuestViewModel unratedGuest;
        public RateGuestViewModel()
        {
        }
        public RateGuestViewModel(UnratedGuestViewModel unratedGuest)
        {
            accommodationReservationRepository=Injector.CreateInstance<IAccommodationReservationRepository>();
            guestRatingRepository=Injector.CreateInstance<IGuestRatingRepository>();
            CleanlinessRatingCommand= new RelayParameterCommand(GetCleanlinessRating);
            RuleComplianceRatingCommand = new RelayParameterCommand(GetRuleComplianceRating);
            RateGuestCommand = new RelayCommand(RateGuest);


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
            guestRatingRepository.Save(new GuestRating(accommodationReservationRepository.GetById(Id).AccommodationId, accommodationReservationRepository.GetById(Id).UserId,Id,Cleanliness,RuleCompliance,Comment));
            UnratedGuestsUserControl.UnratedGuests.Remove(unratedGuest);
        }
    }
}
