using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Appl.UseCases
{
    public class VoucherService
    {
        private IVoucherRepository voucherRepository;
        private TourGuestService tourGuestService;
        private UserService userService;
        public VoucherService()
        {
            voucherRepository = Injector.CreateInstance<IVoucherRepository>();
            tourGuestService = new TourGuestService();
            userService = new UserService();
        }
        public List<Voucher> GetAAll()
        {
            return voucherRepository.GetAll();
        }
        public Voucher Save(Voucher voucher)
        {
            return voucherRepository.Save(voucher);
        }
        public Voucher GetById(int id)
        {
            return voucherRepository.GetById(id);
        }
        public void Delete(Voucher voucher)
        {
            voucherRepository.Delete(voucher);
            return;
        }
        public int NextId()
        {
            return voucherRepository.NextId();
        }

        public Voucher Update(Voucher voucher)
        {
            return voucherRepository.Update(voucher);
        }

        public void FiveTourAYearVoucherWin(TourGuest guest)
        {
            if(tourGuestService.IsFifthTimeOnATour(guest))
            {
                User tourGuide = new User();
                tourGuide.Id = -1;
                Save(new Voucher(NextId(), DateTime.UtcNow.AddMonths(6), VOUCHERTYPE.FIVETOURSGIFT, userService.GetByPersonalID(guest.PersonalID), tourGuide));
            }
        }
    }
}
