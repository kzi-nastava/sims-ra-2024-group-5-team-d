using BookingApp.Domain.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToastNotifications.Position;

namespace BookingApp.Domain.Models
{
    public enum VOUCHERTYPE { CANCELEDTOUR, GUIDEQUIT , FIVETOURSGIFT};
    public class Voucher : ISerializable
    {
        public int Id { get; set; }
        public DateTime ExpireDate { get; set; }
        public VOUCHERTYPE Type { get; set; }
        public User User { get; set; }
        public User TourGuide {get; set;}

        public Voucher(int id, DateTime expireDate, VOUCHERTYPE type, User user, User tourGuide)
        {
            Id = id;
            ExpireDate = expireDate;
            Type = type;
            User = user;
            TourGuide = tourGuide;
        }
        public Voucher() { }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            ExpireDate = DateTime.ParseExact(values[1], "d/M/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
            Type = (VOUCHERTYPE)Enum.Parse(typeof(VOUCHERTYPE), values[2]);
            User = new User() { Id = Convert.ToInt32(values[3]) };
            TourGuide = new User() { Id = Convert.ToInt32(values[4]) };
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString() , ExpireDate.ToString("d/M/yyyy h:mm:ss tt", CultureInfo.InvariantCulture), Type.ToString(), User.Id.ToString(), TourGuide.Id.ToString() };
            return csvValues;
        }

        public void UpgradeForUniversalUse()
        {
            TourGuide.Id = -1;
        }

        public bool IsUniversal()
        {
            return TourGuide.Id == -1;
        }
    }
}
