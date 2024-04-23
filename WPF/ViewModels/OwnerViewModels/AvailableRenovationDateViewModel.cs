using System;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    public class AvailableRenovationDateViewModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public AvailableRenovationDateViewModel(DateTime fromDate,DateTime toDate)
        {
            FromDate = fromDate;
            ToDate = toDate;
        }
    }
}