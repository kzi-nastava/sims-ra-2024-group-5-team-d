using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels
{
    public class CheckpointViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public CheckpointViewModel() { }

        public CheckpointViewModel(int id, string name)
        {
            Id = id;
            Name = name;
        }

    }
}
