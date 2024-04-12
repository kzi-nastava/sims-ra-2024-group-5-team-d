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
using System.Linq;
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
    public class BookingSectionViewModel
    {
        public ICommand BookCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public int NumberOfTourists { get; set; }
        public ObservableCollection<TourGuestViewModel> Tourists { get; set; }
        public TourRealisationViewModel TourRealisation { get; set; }
        public TourViewModel Tour { get; set; }
        public DateOnly TourDate { get; set; }
        public DateTime CancellationDue { get; set; }
        public string FirstCheckpointName { get; set; }
        public User User { get; set; }
        public TourGuestService TourGuestService { get; set; }
        public TourReservation TourReservation { get; set; }
        public ITourReservationRepository tourReservationRepository { get; set; }
        public ICheckPointRepository checkPointRepository { get; set; }
        public ITourRealisationRepository tourRealisationRepository { get; set; }
        public ITourGuestRepository tourGuestRepository { get; set; }

        Notifier notifier = new Notifier(cfg =>
        {
            cfg.PositionProvider = new WindowPositionProvider(
                parentWindow: Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive),
                corner: Corner.BottomRight,
                offsetX: 0,
                offsetY: 0);

            cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                notificationLifetime: TimeSpan.FromSeconds(3),
                maximumNotificationCount: MaximumNotificationCount.FromCount(5));

            cfg.Dispatcher = Application.Current.Dispatcher;
        });
        public BookingSectionViewModel(TourRealisationViewModel tourRealisation, TourViewModel tour, User user, int numberOfSeats) 
        {
            TourRealisation = tourRealisation;
            Tour = tour;
            User = user;
            TourDate = DateOnly.FromDateTime(tourRealisation.DateTime);
            CancellationDue = tourRealisation.DateTime.AddDays(-2);
            checkPointRepository = Injector.CreateInstance<ICheckPointRepository>();
            FirstCheckpointName = checkPointRepository.GetAllCheckPointsByTourId(Tour.Id).FirstOrDefault().Name;
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
