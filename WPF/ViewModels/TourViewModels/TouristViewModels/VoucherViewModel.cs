using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels
{
    public class VoucherViewModel
    {
        public int VoucherId { get; set; }
        public string VoucherTitle { get; set; }
        public DateTime ExpireDate { get; set; }

        public VoucherViewModel(Voucher v)
        {
            switch (v.Type)
            {
                case VOUCHERTYPE.CANCELEDTOUR:
                    VoucherTitle = "Cancelled Tour Voucher";
                    break;
                case VOUCHERTYPE.GUIDEQUIT:
                    VoucherTitle = "Guide Quit Voucher";
                    break;
                case VOUCHERTYPE.FIVETOURSGIFT:
                    VoucherTitle = "Five Tours Gift Voucher";
                    break;
                default:
                    VoucherTitle = "Unknown Voucher Type";
                    break;
            }

            ExpireDate = v.ExpireDate;
            VoucherId = v.Id;
        }


    }
}
