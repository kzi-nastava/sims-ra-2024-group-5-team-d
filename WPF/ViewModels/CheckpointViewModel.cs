using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels
{
    public class CheckPointViewModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TourId { get; set; }
        //public bool IsChecked { get; set; }

        private bool isChecked;
        public bool IsChecked
        {
            get => isChecked;
            set
            {
                if (value != isChecked)
                {
                    isChecked = value;
                    OnPropertyChanged();
                }
            }
        }
        public CheckPointViewModel() { }
        public CheckPointViewModel(int id, string name, int tourId, bool isChecked)
        {
            Id = id;
            Name = name;
            TourId = tourId;
            IsChecked = isChecked;
        }
        public CheckPointViewModel(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
