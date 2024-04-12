using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels
{
    public class AgeStatisticsViewModel
    {
        public int Under18Count { get; set; }
        public int Age18To50Count { get; set; }
        public int Over50Count { get; set; }
        public AgeStatisticsViewModel() { }
        public AgeStatisticsViewModel(int under18Count, int age18To50Count,int over50Count) 
        {
            Under18Count = under18Count;
            Age18To50Count = age18To50Count;
            Over50Count = over50Count;
        }
    }
}
