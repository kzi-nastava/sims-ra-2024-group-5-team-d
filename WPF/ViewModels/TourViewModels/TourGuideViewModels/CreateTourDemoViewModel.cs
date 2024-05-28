using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels.TourViewModels.TourGuideViewModels
{
    public class CreateTourDemoViewModel : INotifyPropertyChanged
    {
        private string _name;
        private string _location;
        private string _duration;
        private string _capacity;
        private string _description;

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        public string Location
        {
            get => _location;
            set { _location = value; OnPropertyChanged(); }
        }

        public string Duration
        {
            get => _duration;
            set { _duration = value; OnPropertyChanged(); }
        }

        public string Capacity
        {
            get => _capacity;
            set { _capacity = value; OnPropertyChanged(); }
        }

        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(); }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
