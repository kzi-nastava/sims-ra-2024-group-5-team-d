using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
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

        private AccommodationRenovationService accommodationRenovationService;
        private AccommodationService accommodationService;

        public RenovationViewModel SelectedRenovation { get; set; }
        private User loggedInUser;
        public static ObservableCollection<RenovationViewModel> Renovations { get; set; }

        public RenovationsViewModel(User user) {
            InitializeServices();
            loggedInUser = user;
            Renovations = new ObservableCollection<RenovationViewModel>();
            CancelRenovationCommand=new RelayParameterCommand(CancelRenovation);
            List<AccommodationRenovation> renovations=accommodationRenovationService.GetRenovationsForOwner(loggedInUser);
            foreach (AccommodationRenovation renovation in renovations)
            {
                Accommodation accommodation=accommodationService.GetById(renovation.AccommodationId);
                Renovations.Add(new RenovationViewModel(renovation.Id,accommodation.Name,accommodation.Type,accommodation.Location,renovation.RenovateFrom,renovation.RenovateTo,accommodation.ImagesPath,accommodation.AverageRating,renovation.IsCancelable(), accommodation.Owner.IsSuperUser));
            }
        }

        private void InitializeServices()
        {
            UserService userService = new UserService(Injector.CreateInstance<IUserRepository>());
            accommodationService = new AccommodationService(Injector.CreateInstance<IAccommodationRepository>(), userService);
            accommodationRenovationService = new AccommodationRenovationService(Injector.CreateInstance<IAccommodationRenovationRepository>(),accommodationService);        
        }

        public void CancelRenovation(object parameter)
        {
            if (parameter != null)
            {
                RenovationViewModel renovation = (RenovationViewModel)parameter;
                CancelRenovationWindow cancelRenovation = new CancelRenovationWindow(renovation);
                cancelRenovation.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                cancelRenovation.ShowDialog();
            }
        }
    }
}
