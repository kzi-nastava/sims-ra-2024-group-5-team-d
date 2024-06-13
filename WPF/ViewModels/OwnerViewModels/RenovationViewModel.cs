using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    public class RenovationViewModel
    {
        public string Star1 { get; set; }
        public string Star2 { get; set; }
        public string Star3 { get; set; }
        public string Star4 { get; set; }
        public string Star5 { get; set; }
        public bool IsCancelable { get; set; }
        public int RenovationId { get; set; }
        public string Name { get; set; }
        public TYPE Type { get; set; }
        public Location Location { get; set; }
        public DateTime RenovationFrom { get; set; }
        public DateTime RenovationTo { get; set; }
        public string ImagesPath { get; set; }
        public bool? IsSuperOwner { get; set; }
        public RenovationViewModel()
        {
        }
        public RenovationViewModel(int renovationId,string name, TYPE type, Location location, DateTime renovationFrom, DateTime renovationTo, string imagesPath,double averageRating,bool isCancelable,bool? isSuperOwner)
        {
            IsCancelable = isCancelable;
            RenovationId = renovationId;
            Name = name;
            Type = type;
            Location = location;
            RenovationFrom = renovationFrom;
            RenovationTo = renovationTo;
            ImagesPath = imagesPath;
            IsSuperOwner = isSuperOwner;
            List<string> starPaths = new List<string>();
            while (averageRating >= 1)
            {
                starPaths.Add("../../../Resources/Images/OwnerImages/StarFull.png");
                averageRating--;
            }
            if (averageRating > 0.25)
                starPaths.Add("../../../Resources/Images/OwnerImages/StarHalfFull.png");
            while (starPaths.Count < 5)
                starPaths.Add("../../../Resources/Images/OwnerImages/idemo.png");
            Star1 = starPaths[0];
            Star2 = starPaths[1];
            Star3 = starPaths[2];
            Star4 = starPaths[3];
            Star5 = starPaths[4];
        }
    }
}
