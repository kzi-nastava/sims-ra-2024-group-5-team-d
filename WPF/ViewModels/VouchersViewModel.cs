using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.Commands;
using BookingApp.WPF.Views.TouristView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels
{
    public class VouchersViewModel
    {

        public ObservableCollection<VoucherViewModel> Vouchers { get; set; }

        public User User { get; set; }
        private IVoucherRepository repository { get; set; }

        public ICommand PastToursTabCommand { get; private set; }

        public ICommand LiveTourTabCommand { get; private set; }


        public VouchersViewModel(User user)
        {
            User = user;
            Vouchers = new ObservableCollection<VoucherViewModel>();
            repository = Injector.CreateInstance<IVoucherRepository>();
            foreach(Voucher v in repository.GetAll())
            {
                if(v.User.Id == user.Id)
                    Vouchers.Add(new VoucherViewModel(v));
            }
            PastToursTabCommand = new RelayCommand(SwitchToPastTours);
            LiveTourTabCommand = new RelayCommand(SwitchToLiveTour);
        }

        public void SwitchToLiveTour()
        {
            TouristHomeWindow.contentControl.Content = new YourToursUserControl(User);
        }

        public void SwitchToPastTours()
        {
            TouristHomeWindow.contentControl.Content = new PastToursUserControl(User);
        }
    }
}
