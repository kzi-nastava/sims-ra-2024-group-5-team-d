using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.WPF.Commands;
using BookingApp.WPF.Views.OwnerView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    public class RenovationsViewModel
    {
        public ICommand CancelRenovationCommand { get; set; }
        public RenovationViewModel SelectedRenovation { get; set; }
        private User loggedInUser;
        public static ObservableCollection<RenovationViewModel> Renovations { get; set; }
        private AccommodationRenovationService accommodationRenovationService;
        private AccommodationService accommodationService;
        public RenovationsViewModel(User user) {
        loggedInUser = user;
            Renovations = new ObservableCollection<RenovationViewModel>();
            CancelRenovationCommand=new RelayCommand(CancelRenovation);
            accommodationRenovationService = new AccommodationRenovationService();
            accommodationService = new AccommodationService();
            List<AccommodationRenovation> renovations=accommodationRenovationService.GetRenovationsForOwner(loggedInUser);
            foreach (AccommodationRenovation renovation in renovations)
            {
                Accommodation accommodation=accommodationService.GetById(renovation.AccommodationId);
                Renovations.Add(new RenovationViewModel(renovation.Id,accommodation.Name,accommodation.Type,accommodation.Location,renovation.RenovateFrom,renovation.RenovateTo,accommodation.ImagesPath,accommodation.AverageRating,renovation.IsCancelable()));
            }
        }
        public void CancelRenovation()
        {
            if(SelectedRenovation!=null)
            {
                CancelRenovationWindow cancelRenovation = new CancelRenovationWindow(SelectedRenovation);
                cancelRenovation.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                cancelRenovation.ShowDialog();
            }
        }
    }
}
