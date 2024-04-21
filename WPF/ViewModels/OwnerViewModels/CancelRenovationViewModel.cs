using BookingApp.Appl.UseCases;
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
        public RenovationViewModel Renovation { get; set; }
        private AccommodationRenovationService accommodationRenovationService;
        public CancelRenovationViewModel(RenovationViewModel renovation)
        {
            accommodationRenovationService = new AccommodationRenovationService();
            Renovation = renovation;
            CancelRenovationCommand = new RelayCommand(CancelRenovation);
        }
        public void CancelRenovation()
        {
            accommodationRenovationService.DeleteById(Renovation.RenovationId);
            RenovationsViewModel.Renovations.Remove(Renovation);
        }
    }
}
