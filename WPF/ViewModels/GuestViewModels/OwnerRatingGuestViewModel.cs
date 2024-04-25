using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels
{
    public class OwnerRatingGuestViewModel
    {
        public string AvatarPath { get; set; }
        public DateTime ReservedFrom { get; set; }
        public DateTime ReservedTo { get; set; }
        public string Comment { get; set; }
        public string AccommodationName { get; set; }
        public string Location { get; set; }
        public int NumberOfPeople { get; set; }
        public string OwnerName { get; set; }
        public int CleanlinessRating { get; set; }
        public int RuleComplianceRating { get; set; }
        public OwnerRatingGuestViewModel() { }
        public OwnerRatingGuestViewModel(string imagePath, DateTime reservedFrom, DateTime reservedTo, string comment, string accommodationName,string location, int numberOfPeople, string ownerName, int cleanliness, int ruleComplianceRating)
        {
            AvatarPath = imagePath;
            ReservedFrom = reservedFrom;
            ReservedTo = reservedTo;
            Comment = comment;
            AccommodationName = accommodationName;
            Location = location;
            NumberOfPeople = numberOfPeople;
            OwnerName = ownerName;
            CleanlinessRating = cleanliness;
            RuleComplianceRating = ruleComplianceRating;

        }

    }
}
