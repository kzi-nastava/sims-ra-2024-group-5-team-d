using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels.TourViewModels.TouristViewModels
{
    public class CreateRequestViewModel
    {
        public int Id { get; set; }
        public User Tourist { get; set; }
        public CreateRequestViewModel(User tourist)
        {
            Tourist = tourist;
        }
    }
}
