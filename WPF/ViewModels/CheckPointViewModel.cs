using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels
{
    public class CheckPointViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TourId { get; set; }
        public bool IsChecked { get; set; }

        public CheckPointViewModel() { }
        public CheckPointViewModel(int id, string name, int tourId, bool isChecked)
        {
            Id = id;
            Name = name;
            TourId = tourId;
            IsChecked = isChecked;
        }
    }
}
