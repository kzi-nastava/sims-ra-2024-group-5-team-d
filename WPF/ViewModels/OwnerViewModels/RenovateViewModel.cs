using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xceed.Wpf.Toolkit.Primitives;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    public class RenovateViewModel
    {
        private User loggedInUser;
        public ICommand ReserveRenovationCommand { get; set; }
        public ICommand SelectionChangedCommand { get; set; }
        public ICommand ShowAvailableDatesCommand { get; set; }

        private AccommodationService accommodationService;
        private AccommodationRenovationService accommodationRenovationService;
        private NotifierService notifierService;
        private AvailableDatesForReservationService availableDatesForReservationService;


        public SelectDateViewModel SelectDate { get; set; }
        public string ReservationReason { get; set; }
        public ObservableCollection<AvailableRenovationDateViewModel> AvailableDates { get; set; }
        public AvailableRenovationDateViewModel SelectedAvailableDate { get; set; }
        private int accommodationId;
        public RenovateViewModel(User user,int accommodationId)
        {
            InitializeServices();
            this.accommodationId = accommodationId;     
            AvailableDates = new ObservableCollection<AvailableRenovationDateViewModel>();
            SelectDate = new SelectDateViewModel(accommodationService.GetAccommodationNameById(accommodationId));
            loggedInUser = user;
            SelectionChangedCommand = new RelayParameterCommand(ListViewSelectionChanged);

            ShowAvailableDatesCommand = new RelayCommand(ShowAvailableDates);
            ReserveRenovationCommand = new RelayCommand(ReserveRenovation);
        }

        private void ListViewSelectionChanged(object obj)
        {
            if (obj != null)
            {
                SelectedAvailableDate= obj as AvailableRenovationDateViewModel;
                for(int i=0;i<AvailableDates.Count;i++)
                {
                    if (AvailableDates[i] == SelectedAvailableDate)
                        AvailableDates[i].IsSelected = true;
                    else
                        AvailableDates[i].IsSelected = false;

                }
                SelectDate.ShowReserve = true;
            }
            
        }
        private void InitializeServices()
        {
            notifierService = new NotifierService();
            UserService userService = new UserService(Injector.CreateInstance<IUserRepository>());
            accommodationService = new AccommodationService(Injector.CreateInstance<IAccommodationRepository>(),userService);
            accommodationRenovationService = new AccommodationRenovationService(Injector.CreateInstance<IAccommodationRenovationRepository>());
            AccommodationReservationService accommodationReservationService = new AccommodationReservationService(Injector.CreateInstance<IAccommodationReservationRepository>());
            availableDatesForReservationService = new AvailableDatesForReservationService(accommodationReservationService,accommodationRenovationService);
        }

        private void ShowAvailableDates()
        {
            AvailableDates.Clear();
            List<KeyValuePair<DateTime, DateTime>> availableDatesForRenovations = availableDatesForReservationService.GetAvailableDatesInGivenRange(SelectDate.RenovateFrom, SelectDate.RenovateTo, SelectDate.DaysForRenovation, accommodationService.GetById(accommodationId));
            availableDatesForRenovations.ForEach(date => AvailableDates.Add(new AvailableRenovationDateViewModel(date.Key, date.Value)));
            SelectDate.ShowRenovationDates = true;  
            SelectDate.ShowReserve = false;

        }
        private void ReserveRenovation()
        {
            if (SelectedAvailableDate != null)
            {
                notifierService.ShowSuccess("Renovation reserved successfully");
                SelectDate.ShowRenovationDates = false;
                accommodationRenovationService.Save(new AccommodationRenovation(accommodationId, SelectedAvailableDate.FromDate, SelectedAvailableDate.ToDate, ReservationReason));
            }
        }
    }
}
