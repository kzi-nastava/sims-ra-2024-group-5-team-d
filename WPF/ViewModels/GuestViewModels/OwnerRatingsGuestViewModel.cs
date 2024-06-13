using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.WPF.ViewModels.OwnerViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels
{
    public class OwnerRatingsGuestViewModel
    {
        public ObservableCollection<OwnerRatingGuestViewModel> OwnerRatingsGuest { get; set; }
        public GuestRatingService guestRatingService;
        public AccommodationService accommodationService;
        public AccommodationReservationService accommodationReservationService;
        public OwnerRatingsGuestViewModel(User user) 
        {
            InitializeServices();
            OwnerRatingsGuest = new ObservableCollection<OwnerRatingGuestViewModel>();
            guestRatingService.GetAllRatingsForGuest(user).ForEach(rating=>
            OwnerRatingsGuest.Add(new OwnerRatingGuestViewModel(accommodationService.GetById(rating.AccommodationId).Owner.AvatarPath, accommodationReservationService.GetById(rating.ReservationId).ReservedFrom, accommodationReservationService.GetById(rating.ReservationId).ReservedTo,rating.Comment , accommodationService.GetById(rating.AccommodationId).Name, accommodationService.GetById(rating.AccommodationId).Location.ToString(), accommodationService.GetById(rating.ReservationId).Capacity, accommodationService.GetById(rating.AccommodationId).Owner.FullName,rating.CleanlinessRating, rating.RuleComplianceRating)));

        }

        private void InitializeServices()
        {
            guestRatingService = new GuestRatingService();
            accommodationService = new AccommodationService();
            accommodationReservationService = new AccommodationReservationService();
        }
    }
}
