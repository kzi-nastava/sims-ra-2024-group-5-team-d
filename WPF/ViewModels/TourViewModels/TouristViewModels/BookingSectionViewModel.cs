using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using BookingApp.WPF.Commands;
using BookingApp.WPF.Views;
using BookingApp.WPF.Views.TouristView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace BookingApp.WPF.ViewModels
{
    public class BookingSectionViewModel : INotifyPropertyChanged
    {
        private bool isVoucherInUse;
        public bool IsVoucherInUse
        {
            get => isVoucherInUse;
            set
            {
                if (value != isVoucherInUse)
                {
                    isVoucherInUse = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand UseCommand { get; set; }
        public ICommand UseVoucherCommand { get; set; }

        private bool isUseVoucherClicked;
        public bool IsUseVoucherClicked
        {
            get => isUseVoucherClicked;
            set
            {
                if (value != isUseVoucherClicked)
                {
                    isUseVoucherClicked = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool voucherNotInUse;
        public bool VoucherNotInUse
        {
            get => voucherNotInUse;
            set
            {
                if (value != voucherNotInUse)
                {
                    voucherNotInUse = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public VoucherViewModel SelectedVoucher { get; set; }
        public ICommand BookCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public int NumberOfTourists { get; set; }
        public ObservableCollection<TourGuestViewModel> Tourists { get; set; }
        public TourRealisationViewModel TourRealisation { get; set; }
        public TourViewModel Tour { get; set; }
        public DateOnly TourDate { get; set; }
        public DateTime CancellationDue { get; set; }
        public string FirstCheckpointName { get; set; }
        public ObservableCollection<VoucherViewModel> Vouchers { get; set; }
        public User User { get; set; }
        public TourGuestService TourGuestService { get; set; }
        public TourReservationService tourReservationService { get; set; }
        public TourReservation TourReservation { get; set; }
        public TourRealisationService tourRealisationService { get; set; }
        public TourGuestService tourGuestService { get; set; }

        public CheckPointService checkPointService;

        public VoucherService voucherService { get; set; }

        NotifierService notifier;
        public BookingSectionViewModel(TourRealisationViewModel tourRealisation, TourViewModel tour, User user, int numberOfSeats) 
        {
            tourReservationService = new TourReservationService();
            tourRealisationService = new TourRealisationService();
            tourGuestService = new TourGuestService();
            voucherService = new VoucherService();
            TourGuestService = new TourGuestService();
            checkPointService = new CheckPointService();
            notifier = new NotifierService();

            TourRealisation = tourRealisation;
            Tour = tour;
            User = user;
            TourDate = DateOnly.FromDateTime(tourRealisation.DateTime);
            CancellationDue = tourRealisation.DateTime.AddDays(-2);
            FirstCheckpointName = checkPointService.GetAllCheckPointsByTourId(Tour.Id).FirstOrDefault().Name;
            NumberOfTourists = numberOfSeats;
            Tourists = new ObservableCollection<TourGuestViewModel>();
            TourReservation = new TourReservation(tourReservationService.NextId(), tourRealisation.Id, user);

            CreateTouristsFormular();

            BookCommand = new RelayCommand(SaveReservation);
            CancelCommand = new RelayCommand(CancelReservation);
            UseVoucherCommand = new RelayCommand(UseVoucheClick);
            UseCommand = new RelayParameterCommand(UseClick);

            Vouchers = new ObservableCollection<VoucherViewModel>();
            foreach (Voucher v in voucherService.GetAAll())
            {
                Debug.WriteLine("AAAAA" + tour.User.Id);
                if (v.User.Id == user.Id && v.ExpireDate > DateTime.Now && ((v.TourGuide.Id == tour.User.Id) || v.IsUniversal()))
                {
                    Debug.WriteLine("MIKA IMA VELIKO DUPE!!!!!!");
                    Vouchers.Add(new VoucherViewModel(v));
                }                    
            }

            IsUseVoucherClicked = false;
            VoucherNotInUse = true;
            IsVoucherInUse = false;
        }

        public void UseClick(object parameter)
        {
            if(parameter is VoucherViewModel voucher)
            {
                VoucherNotInUse = false;
                IsUseVoucherClicked = false;
                IsVoucherInUse = true;
                SelectedVoucher = voucher;
            }

        }

        public void UseVoucheClick()
        {
            if (IsUseVoucherClicked)
                IsUseVoucherClicked = false;
            else
                IsUseVoucherClicked = true;
        }

        public void CreateTouristsFormular()
        {
            for (int i = 0; i < NumberOfTourists; i++)
            {
                Tourists.Add(new TourGuestViewModel(TourGuestService.NextIdForGuest(), TourReservation.Id, $"Tourist {i + 1}"));
            }
            FillUserInfo();
        }
        public void FillUserInfo()
        {
            int age = DateTime.Today.Year - User.BirthDate.Year;

            if (User.BirthDate > DateOnly.FromDateTime(DateTime.Today.AddYears(-age)))
            {
                age--;
            }

            Tourists[0].Years = age;
            Tourists[0].PersonalID = User.PersonalId;
            Tourists[0].FullName = User.FullName;
        }

        public void SaveReservation()
        {
            Voucher toBeDeleted = new Voucher();
            if(SelectedVoucher != null)
            {
                toBeDeleted.Id = SelectedVoucher.VoucherId;
                voucherService.Delete(toBeDeleted);
            }
            tourReservationService.Save(TourReservation);
            TourRealisation tR = tourRealisationService.GetTourRealisationById(TourRealisation.Id);
            tR.AvailableSeats -= NumberOfTourists;

            tourRealisationService.Update(tR);
            foreach(TourGuestViewModel tG in Tourists)
            {
                tourGuestService.Save(new TourGuest(tG.Id, tG.FullName, tG.Years, TourReservation.Id,-1, tG.PersonalID));
            }
            notifier.ShowSuccess("Tour booked successfully");
            TouristHomeWindow.contentControl.Content = new TouristHomeUserControl(User);
        }

        public void CancelReservation()
        {
            TouristHomeWindow.contentControl.Content = new TourDetailsUserControl(Tour, NumberOfTourists, User);
        }
    }
}
