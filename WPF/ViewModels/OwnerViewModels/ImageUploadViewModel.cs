using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    public class ImageUploadViewModel:INotifyPropertyChanged
    {
        public string ImagePath { get; set; }
        private bool isSelected = false;

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                OnPropertyChanged();
            }
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        public ImageUploadViewModel(string ImagePath)
        {
            this.ImagePath = ImagePath;
            IsSelected = false;
        }
    }
}
