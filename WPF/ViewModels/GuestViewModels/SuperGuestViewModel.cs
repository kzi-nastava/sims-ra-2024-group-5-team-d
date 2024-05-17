using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels.GuestViewModels
{
    public class SuperGuestViewModel
    {
        public string Star1 { get; set; }
        public string Star2 { get; set; }
        public string Star3 { get; set; }
        public string Star4 { get; set; }
        public string Star5 { get; set; }
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime ValidFrom { get; set; }
        public int BonusPoints { get; set; }
        public string AvatarPath { get; set; }
        public int NumberOfReservationThisYear { get; set; }
        public string GuestName { get; set; }
        public SuperGuestViewModel()
        {

        }
        public SuperGuestViewModel(int id, int userId, DateTime validFrom, int bonusPoints, string avatarPath, int numberOfReservation, string guestName)
        {
            Id = id;
            UserId = userId;
            ValidFrom = validFrom;
            BonusPoints = bonusPoints;
            AvatarPath = avatarPath;
            NumberOfReservationThisYear = numberOfReservation;
            GuestName = guestName;

            List<string> starPaths = new List<string>();

            for (int i = 0; i < bonusPoints; i++)
            {
                starPaths.Add("../../../Resources/Images/GuestImages/fullStar.png");
            }

            for (int i = starPaths.Count; i < 5; i++)
            {
                starPaths.Add("../../../Resources/Images/GuestImages/star.png");
            }

            Star1 = starPaths[0]; 
            Star2 = starPaths[1];
            Star3 = starPaths[2];
            Star4 = starPaths[3];
            Star5 = starPaths[4];

        }
        public SuperGuestViewModel( int userId, DateTime validFrom, int bonusPoints, string avatarPath, int numberOfReservation, string guestName)
        {
            UserId = userId;
            ValidFrom = validFrom;
            BonusPoints = bonusPoints;
            AvatarPath = avatarPath;
            NumberOfReservationThisYear = numberOfReservation;
            GuestName = guestName;
            List<string> starPaths = new List<string>();

            for (int i = 0; i < bonusPoints; i++)
            {
                starPaths.Add("../../../Resources/Images/GuestImages/fullStar.png");
            }

            for (int i = starPaths.Count; i < 5; i++)
            {
                starPaths.Add("../../../Resources/Images/GuestImages/star.png");
            }

            Star1 = starPaths.Count > 0 ? starPaths[0] : "";
            Star2 = starPaths.Count > 1 ? starPaths[1] : "";
            Star3 = starPaths.Count > 2 ? starPaths[2] : "";
            Star4 = starPaths.Count > 3 ? starPaths[3] : "";
            Star5 = starPaths.Count > 4 ? starPaths[4] : "";

        }
        public SuperGuestViewModel(int userId, string avatarPath,string guestName)
        {
            UserId = userId;
            AvatarPath = avatarPath;
            GuestName = guestName;

        }
    }
}
