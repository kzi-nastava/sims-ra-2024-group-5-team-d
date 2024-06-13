using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IGuestRequestRepository
    {
        public List<GuestRequest> GetAll();
        public GuestRequest Save(GuestRequest request);
        public void Delete(GuestRequest request);
        public GuestRequest Update(GuestRequest request);
        public GuestRequest GetById(int id);

    }
}
