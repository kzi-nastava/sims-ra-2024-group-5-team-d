using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using BookingApp.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.WPF.Views.OwnerView
{
    /// <summary>
    /// Interaction logic for OwnerReviewUserControl.xaml
    /// </summary>
    public partial class OwnerReviewUserControl : UserControl
    {
        public static ObservableCollection<UnratedGuestViewModel>UnratedGuests { get; set; }
        private UnratedGuestService unratedGuestService;
        private User LoggedInUser;
        private IUserRepository userRepository;
        public OwnerReviewUserControl(User user)
        {
            userRepository = Injector.CreateInstance<IUserRepository>();
            LoggedInUser = user;
            unratedGuestService = new UnratedGuestService();
            UnratedGuests = new ObservableCollection<UnratedGuestViewModel>();
            DataContext = this;
            unratedGuestService.GetUnratedGuests(LoggedInUser)
                                .ForEach(unratedGuest => UnratedGuests.Add(new UnratedGuestViewModel(userRepository.GetById(unratedGuest.UserId).FullName)));
            InitializeComponent();
        }
        private void RateGuest(object sender, RoutedEventArgs e)
        {
            UnratedGuestsUserControl unratedGuestsWindow = new UnratedGuestsUserControl(LoggedInUser);
            OwnerMainWindow.contentControl.Content = unratedGuestsWindow;

        }

        private void OwnerRates(object sender, RoutedEventArgs e)
        {
            OwnerRatingsUserControl ownerRatingsUserControl = new OwnerRatingsUserControl(LoggedInUser);
            OwnerMainWindow.contentControl.Content = ownerRatingsUserControl;
        }
    }
}
