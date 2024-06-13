using BookingApp.Domain.Models;
using BookingApp.Validation;
using HarfBuzzSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    public class RegistrationViewModel : ValidationBase,INotifyPropertyChanged
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

        protected override void ValidateSelf()
        {
            if (string.IsNullOrWhiteSpace(this.Name))
            {
                this.ValidationErrors["Name"] = "Name cannot be empty.";
            }

            if (CancellationDeadline <= 0)
            {
                this.ValidationErrors[nameof(CancellationDeadline)] = "Deadline must be greater than zero.";
            }

            if (MaxCapacity < 1)
            {
                this.ValidationErrors[nameof(MaxCapacity)] = "Capacity must be greater than zero.";
            }

            if (MinDaysToStay <= 0)
            {
                this.ValidationErrors[nameof(MinDaysToStay)] = "Minimum days to stay must be greater than zero";
            }

            if (LocationId < 0)
            {
                this.ValidationErrors[nameof(LocationId)] = "Please select a location.";
            }

            if (Type < 0)
            {
                this.ValidationErrors[nameof(Type)] = "Please select a type.";
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
