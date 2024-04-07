using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels
{
    public class UnratedGuestViewModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public UnratedGuestViewModel()
        {

        }
        public UnratedGuestViewModel(string name)
        {
            Name = name;
        }
    }
}
