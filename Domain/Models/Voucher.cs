using BookingApp.Domain.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Models
{
    public enum VOUCHERTYPE { CANCELEDTOUR, GUIDEQUIT , FIVETOURSGIFT};
    public class Voucher : ISerializable
    {
        public int Id { get; set; }
        public DateTime ExpireDate { get; set; }
        public VOUCHERTYPE Type { get; set; }

        public Voucher(int id, DateTime expireDate, VOUCHERTYPE type)
        {
            Id = id;
            ExpireDate = expireDate;
            Type = type;
        }
        public Voucher() { }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            ExpireDate = Convert.ToDateTime(values[1]);
            Type = (VOUCHERTYPE)Convert.ToInt32(values[2]);
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString() , ExpireDate.ToString(), Type.ToString()};
            return csvValues;
        }
    }
}
