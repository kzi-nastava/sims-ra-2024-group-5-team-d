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
        public TourReservation TourReservation { get; set; }
        public ITourReservationRepository tourReservationRepository { get; set; }
        public ICheckPointRepository checkPointRepository { get; set; }
        public ITourRealisationRepository tourRealisationRepository { get; set; }
        public ITourGuestRepository tourGuestRepository { get; set; }
        public CheckPointService checkPointService;
        public IVoucherRepository voucherRepository;

        NotifierService notifier;
        public BookingSectionViewModel(TourRealisationViewModel tourRealisation, TourViewModel tour, User user, int numberOfSeats) 
        {
            notifier = new NotifierService();
            TourRealisation = tourRealisation;
            Tour = tour;
            User = user;
            TourDate = DateOnly.FromDateTime(tourRealisation.DateTime);
            CancellationDue = tourRealisation.DateTime.AddDays(-2);
            checkPointRepository = Injector.CreateInstance<ICheckPointRepository>();
            checkPointService = new CheckPointService();
            FirstCheckpointName = checkPointService.GetAllCheckPointsByTourId(Tour.Id).FirstOrDefault().Name;
            NumberOfTourists = numberOfSeats;
            Tourists = new ObservableCollection<TourGuestViewModel>();
            TourGuestService = new TourGuestService();
            tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
            TourReservation = new TourReservation(tourReservationRepository.NextIdForReservation(), tourRealisation.Id, user);
            CreateTouristsFormular();
            BookCommand = new RelayCommand(SaveReservation);
            CancelCommand = new RelayCommand(CancelReservation);
            tourRealisationRepository = Injector.CreateInstance<ITourRealisationRepository>();
            tourGuestRepository = Injector.CreateInstance<ITourGuestRepository>();
            Vouchers = new ObservableCollection<VoucherViewModel>();
            voucherRepository = Injector.CreateInstance<IVoucherRepository>();
            foreach (Voucher v in voucherRepository.GetAll())
            {
                if (v.User.Id == user.Id && v.ExpireDate > DateTime.Now)
                {
                    Vouchers.Add(new VoucherViewModel(v));
                }                    
            }
            UseVoucherCommand = new RelayCommand(UseVoucheClick);
            UseCommand = new RelayCommand(UseClick);
            IsUseVoucherClicked = false;
            VoucherNotInUse = true;
            IsVoucherInUse = false;
        }

        public void UseClick()
        {
            if(SelectedVoucher != null)
            {
                VoucherNotInUse = false;
                IsUseVoucherClicked = false;
                IsVoucherInUse = true;
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
                voucherRepository.Delete(toBeDeleted);
            }
            tourReservationRepository.SaveReservation(TourReservation);
            TourRealisation tR = tourRealisationRepository.GetTourRealisationById(TourRealisation.Id);
            tR.AvailableSeats -= NumberOfTourists;
            tourRealisationRepository.UpdateTourRealisation(tR);
            foreach(TourGuestViewModel tG in Tourists)
            {
                tourGuestRepository.SaveGuest(new TourGuest(tG.Id, tG.FullName, tG.Years, TourReservation.Id,-1, tG.PersonalID));
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
