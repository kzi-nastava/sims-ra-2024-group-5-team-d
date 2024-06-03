using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    public class ShortcutViewModel
    {
        public string Name { get; set; }
        public string Command { get; set; }
        public ShortcutViewModel(string name, string command)
        {
            Name = name;
            Command = command;
        }
    }
}
