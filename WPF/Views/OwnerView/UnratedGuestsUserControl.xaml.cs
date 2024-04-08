using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    /// Interaction logic for UnratedGuestsUserControl.xaml
    /// </summary>
    public partial class UnratedGuestsUserControl : UserControl
    {
        public static ObservableCollection<UnratedGuestViewModel> UnratedGuests { get; set; }
        public UnratedGuestViewModel SelectedGuest { get; set; }
        private IUserRepository userRepository;
        private IAccommodationRepository accommodationRepository;
        private User loggedInUser;
        private UnratedGuestService unratedGuestService;
        public UnratedGuestsUserControl(User user)
        {
            accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
            userRepository = Injector.CreateInstance<IUserRepository>();
            unratedGuestService = new UnratedGuestService();
            loggedInUser = user;
            UnratedGuests = new ObservableCollection<UnratedGuestViewModel>();
            DataContext = this;
            unratedGuestService.GetUnratedGuests(loggedInUser)
                                .ForEach(unratedGuest => UnratedGuests.Add
                                (new UnratedGuestViewModel(unratedGuest.Id,userRepository.GetById(unratedGuest.UserId).Username, unratedGuest.ReservedFrom, unratedGuest.ReservedTo, accommodationRepository.GetById(unratedGuest.AccommodationId).Location, accommodationRepository.GetAccommodationNameById(unratedGuest.AccommodationId)))
                                );
            InitializeComponent();
        }


         private void RateGuest(object sender, RoutedEventArgs e)
         {
            if(SelectedGuest!=null)
            {
                RateGuestFormWindow rateFormWindow = new RateGuestFormWindow(SelectedGuest);
                Window parentWindow = Window.GetWindow(this);
                rateFormWindow.Owner = parentWindow;
                rateFormWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                rateFormWindow.ShowDialog();
            }    


         }
    }
}
