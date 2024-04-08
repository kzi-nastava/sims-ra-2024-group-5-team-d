using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels
{
    public class RateGuestViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public int Cleanliness { get; set; }
        public int RuleCompliance { get; set; }
        public RateGuestViewModel()
        {
        }
        public RateGuestViewModel(int id, string name)
        {
            Cleanliness = 1;
            RuleCompliance = 1;
            Id = id;
            Name = name;
        }
    }
}
