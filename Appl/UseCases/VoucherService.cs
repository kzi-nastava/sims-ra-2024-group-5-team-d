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
        public VoucherService()
        {
            voucherRepository = Injector.CreateInstance<IVoucherRepository>();
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
    }
}
