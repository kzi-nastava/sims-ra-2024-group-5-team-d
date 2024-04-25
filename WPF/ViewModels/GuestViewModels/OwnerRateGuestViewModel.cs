using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels.GuestViewModels
{

    public class OwnerRateGuestViewModel
    {
        public OwnerRatingGuestViewModel OwnerRatingGuestViewModel{ get; set; }
        public OwnerRateGuestViewModel(OwnerRatingGuestViewModel ownerRatingGuestViewModel)
        {
            OwnerRatingGuestViewModel=ownerRatingGuestViewModel;
        }
    }
}
