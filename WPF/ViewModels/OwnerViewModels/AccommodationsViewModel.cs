using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.WPF.Commands;
using BookingApp.WPF.Views.OwnerView;
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
    public class AccommodationsViewModel
    {
        public ICommand RenovateCommand { get; set; }
        public ICommand ShowStatsCommand { get; set; }
        public static ObservableCollection<AccommodationViewModel> Accommodations { get; set; }
        private readonly AccommodationService accommodationService;
        private AccommodationRatingService accommodationRatingService;
        public AccommodationViewModel SelectedAccommodation { get; set; }
        private User loggedInUser;
        public AccommodationsViewModel(User user)
        {
            loggedInUser = user;
            accommodationService = new AccommodationService();
            accommodationRatingService = new AccommodationRatingService();
            Accommodations = new ObservableCollection<AccommodationViewModel>();
            List<Accommodation> ownerAccommodations = accommodationService.GetByUser(user);
            ownerAccommodations.Sort((x, y) => y.IsSuperOwner.CompareTo(x.IsSuperOwner));
            ownerAccommodations.ForEach(accommodation => Accommodations.Add(new AccommodationViewModel(accommodation.Id, accommodation.Name, accommodation.Location, accommodation.Type, accommodation.ImagesPath, accommodation.IsSuperOwner, accommodation.AverageRating, accommodationRatingService.GetNumberOfRatingsForAccommodation(accommodation))));
            RenovateCommand = new RelayParameterCommand(Renovate);
            ShowStatsCommand = new RelayParameterCommand(ShowStats);
        }
        private void Renovate(object obj)
        {
            AccommodationViewModel accommodation = (AccommodationViewModel)obj;
            if (accommodation != null)
            {
                OwnerMainWindow.contentControl.Content = new RenovateAccommodationUserControl(loggedInUser,accommodation.Id);
            }
        }
        private void ShowStats(object obj)
        {
            AccommodationViewModel accommodation= (AccommodationViewModel)obj;
            if(accommodation!=null)
            OwnerMainWindow.contentControl.Content = new AccommodationStatsUserControl(accommodation.Id,loggedInUser); 
        }
    }
}
