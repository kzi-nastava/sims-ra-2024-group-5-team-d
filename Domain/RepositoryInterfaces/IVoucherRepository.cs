using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IVoucherRepository
    {
        public List<Voucher> GetAll();
        public int NextId();
        public Voucher Update(Voucher voucher);
        public Voucher GetById(int id);
        public void Delete(Voucher voucher);
        public Voucher Save(Voucher voucher);   
    }
}
