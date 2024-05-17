using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    public class RegistrationViewModel :INotifyPropertyChanged
    {
        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }
        private int locationId;
        public int LocationId
        {
            get => locationId;
            set
            {
                locationId = value;
                OnPropertyChanged();
            }
        }
        private int type;
        public int Type
        {
            get => type;
            set
            {
                type = value;
                OnPropertyChanged();
            }
        }
        private int cancellationDeadline;
        public int CancellationDeadline
        {
            get => cancellationDeadline;
            set
            {
                cancellationDeadline = value;
                OnPropertyChanged();
            }
        }
        private int maxCapacity;
        public int MaxCapacity
        {
            get => maxCapacity;
            set
            {
                maxCapacity = value;
                OnPropertyChanged();
            }
        }
        private int minDaysToStay;
        public int MinDaysToStay
        {
            get => minDaysToStay;
            set
            {
                minDaysToStay = value;
                OnPropertyChanged();
            }
        }
        private bool isUploading = false;
        public bool IsUploading
        {
            get => isUploading;
            set
            {
                isUploading = value;
                OnPropertyChanged();
            }
        }
        private bool setAsDefaultEnabled=false;
        public bool SetAsDefaultEnabled
        {
            get => setAsDefaultEnabled;
            set
            {
                setAsDefaultEnabled = value;
                OnPropertyChanged();
            }
        }
        public void Reset()
        {
            Name = "";
            LocationId = 0;
            Type = 0;
            CancellationDeadline = 0;
            MaxCapacity = 0;
            MinDaysToStay = 0;
            IsUploading = false;
            SetAsDefaultEnabled = false;
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
