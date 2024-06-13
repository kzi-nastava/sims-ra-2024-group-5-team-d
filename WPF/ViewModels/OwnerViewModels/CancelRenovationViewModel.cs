using BookingApp.Appl.UseCases;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    public class CancelRenovationViewModel
    {
        public ICommand CancelRenovationCommand { get; set; }

        private AccommodationRenovationService accommodationRenovationService;

        public RenovationViewModel Renovation { get; set; }
        public CancelRenovationViewModel(RenovationViewModel renovation)
        {
            InitializeServices();
            Renovation = renovation;
            CancelRenovationCommand = new RelayCommand(CancelRenovation);
        }

        private void InitializeServices()
        {
            accommodationRenovationService = new AccommodationRenovationService(Injector.CreateInstance<IAccommodationRenovationRepository>());
        }

        public void CancelRenovation()
        {
                accommodationRenovationService.DeleteById(Renovation.RenovationId);
                RenovationsViewModel.Renovations.Remove(Renovation);
        }
    }
}
