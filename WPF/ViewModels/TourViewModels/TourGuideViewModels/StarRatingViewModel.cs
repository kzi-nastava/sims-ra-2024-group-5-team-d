using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels.TourViewModels.TourGuideViewModels
{
    public class StarRatingViewModel
    {
        public bool IsStar1 { get; set; } = false;
        public bool IsStar2 { get; set;} = false;
        public bool IsStar3 { get; set; } = false;
        public bool IsStar4 { get; set; } = false;
        public bool IsStar5 { get; set; } = false;

        public StarRatingViewModel(int rate) {
              SetStarRating(rate);
        }
        public StarRatingViewModel(bool isStar1, bool isStar2, bool isStar3, bool isStar4, bool isStar5)
        {
            IsStar1 = isStar1;
            IsStar2 = isStar2;
            IsStar3 = isStar3;
            IsStar4 = isStar4;
            IsStar5 = isStar5;
        }
        public void SetStarRating(int rate)
        {
            var stars = new List<Action<bool>> {
                (value) => IsStar1 = value,
                (value) => IsStar2 = value,
                (value) => IsStar3 = value,
                (value) => IsStar4 = value,
                (value) => IsStar5 = value
            };

            // Set the appropriate stars to true based on the rate
            for (int i = 0; i < stars.Count; i++)
            {
                stars[i](i < rate);
            }
        }
    }
}
