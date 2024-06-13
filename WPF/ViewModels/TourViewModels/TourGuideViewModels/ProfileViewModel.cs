using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.Commands;
using BookingApp.WPF.Views.TouristGuide;
using BookingApp.WPF.Views.TouristView;
using Jamesnet.Wpf.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels
{
    public class ProfileViewModel
    {
        public ICommand ComboBoxSelectionChangedCommand {  get; set; }
        public ICommand QuitCommand {  get; set; }
        public ObservableCollection<TourViewModel> Tour { get; set; }
        public ObservableCollection<string> Date { get; set; }

        public User LoggedInUser { get; set; }
        public TourService tourService { get; set; }
        public TourGuestService tourGuestService { get; set; }
        public UserService userService { get; set; }
        public SuperUserService superUserService { get; set; }
        public VoucherService voucherService { get; set; }
        public TourReservationService tourReservationService { get; set; }
        public TourRealisationService tourRealisationService { get; set; }
        public TourRatingService tourRatingService { get; set; }
        public LocationService locationService { get; set; }
        public SuperGuideService superGuideService { get; set; }
        public bool IsSuperGuide { get; set; }
        public double AverageRating { get; set; }
        public int NumberOfRatings { get; set; }
        public Window SideBar {  get; set; }
        public ProfileViewModel(User user, Window sidebar) 
        { 
            LoggedInUser = user;
            SideBar = sidebar;
            superGuideService = new SuperGuideService(user);
            IsSuperGuide = superGuideService.ShouldHaveTheSuperStatus(LoggedInUser);
            tourService = new TourService();
            tourGuestService = new TourGuestService();
            tourReservationService = new TourReservationService();
            voucherService = new VoucherService();
            userService = new UserService();
            superUserService = new SuperUserService();
            tourRealisationService = new TourRealisationService();
            tourRatingService = new TourRatingService();
            locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            Tour = new ObservableCollection<TourViewModel>();
            Date = new ObservableCollection<string>();
            ComboBoxSelectionChangedCommand = new RelayParameterCommand(OnComboBoxSelectionChanged);
            AverageRating = Math.Round(tourRatingService.GetAverageRatingForGuide(LoggedInUser), 3);
            NumberOfRatings = tourRatingService.RatingsOfGuide(LoggedInUser).Count();
            QuitCommand = new RelayCommand(Quit);
            InitializeComboBox();
            OnComboBoxSelectionChanged("All Time");
        }

        private void OnComboBoxSelectionChanged(object parameter)
        {
            Tour.Clear();
            string selectedValue = parameter as string;
            if (selectedValue != null)
            {
                if (selectedValue == "All Time")
                {
                    
                    Tour tour = tourService.GetBestTourOfAllTime();
                    Tour.Add(new TourViewModel(tour.Id, tour.Name, tour.Description, locationService.GetById(tour.Location.Id), tour.Duration, tour.ImagesPath, tour.MaxCapacity,tour.Language, tour.User));
                }
                else
                {
                    if (int.TryParse(selectedValue, out int selectedYear))
                    {
                        Tour tour = tourService.GetBestTourInAYear(selectedYear);
                        if(tour == null) {  return; }
                        Tour.Add(new TourViewModel(tour.Id, tour.Name, tour.Description, locationService.GetById(tour.Location.Id), tour.Duration, tour.ImagesPath, tour.MaxCapacity, tour.Language, tour.User));
                    }
                }
            }
        }
        private void InitializeComboBox()
        {
            Date.Add("All Time");

            var uniqueYears = tourRealisationService.GetAllTourRealisations()
                .SelectMany(realisation => new[] { realisation.StartTime.Year })
                .Distinct();
            foreach (var year in uniqueYears)
            {
                Date.Add(year.ToString());
            }
        }

        private void Quit()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to quit?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                GiveVouchersToGuests();
                DeleteAllTourGuideInfo();
            }
        }

        private void GiveVouchersToGuests() 
        {
            User tourGuide = new User();
            tourGuide.Id = -1;
            tourRealisationService.GetAllTourRealisations(LoggedInUser).Where(rl => rl.IsFinished == false && rl.IsLive == false).ToList().ForEach(realisation =>
            {
                tourGuestService.GetAllTourGuests().ForEach(guest =>
                {
                    if(tourReservationService.GetById(guest.TourReservationId).TourRealisationId == realisation.Id && tourGuestService.IsUser(guest))
                    {
                        voucherService.GetAAll().ForEach(voucher =>
                        {
                            if(voucher.User.Id == userService.GetByPersonalID(guest.PersonalID).Id && voucher.TourGuide.Id == LoggedInUser.Id)
                            {
                                voucher.TourGuide.Id = -1;
                                voucherService.Update(voucher);
                            }
                            });
                        voucherService.Save(new Voucher(voucherService.NextId(), DateTime.Now.AddYears(2), VOUCHERTYPE.GUIDEQUIT,userService.GetByPersonalID(guest.PersonalID) , tourGuide));
                    }
                });
            });
        }

        public void DeleteAllTourGuideInfo()
        {
            tourRealisationService.GetAllTourRealisations(LoggedInUser).ForEach(realisation => {
                realisation.IsFinished = true;
                tourRealisationService.Update(realisation);
            });
            LoggedInUser.HasJob = false;
            userService.Update(LoggedInUser);

            SideBar.Close();
        }

    }
}
