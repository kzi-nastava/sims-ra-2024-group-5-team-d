using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using BookingApp.WPF.ViewModels;
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
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace BookingApp.WPF.Views.TouristGuide
{
    /// <summary>
    /// Interaction logic for LiveTourView.xaml
    /// </summary>
    public partial class LiveTourView : UserControl,INotifyPropertyChanged
    {
        public User LoggedInUser { get; set; }
        public TourRealisationViewModel tourRealisation { get; set; }
        public TourViewModel tour { get; set; }
        public ObservableCollection<TourGuestViewModel> TourGuests { get; set; }
        public ObservableCollection<CheckPointViewModel> CheckPoints { get; set; }
        public TourGuestService tourGuestService { get; set; }
        private ICheckPointRepository checkPointRepository { get; set; }
        private ITourGuestRepository tourGuestRepository { get; set; }
        private ObservableCollection<int> Numbers { get; set; }

        public LiveTourView(User user, TourRealisationViewModel tourRealisationViewModel, TourViewModel tourViewModel)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;
            tourRealisation = tourRealisationViewModel;
            tour = tourViewModel;
            CheckPoints = new ObservableCollection<CheckPointViewModel>();

            TourGuests = new ObservableCollection<TourGuestViewModel>();
            tourGuestService = new TourGuestService();
            checkPointRepository = Injector.CreateInstance<ICheckPointRepository>();
            tourGuestRepository = Injector.CreateInstance<ITourGuestRepository>();
            tourGuestService.GetTourGuestsOnTourRealisation(tourRealisation.Id).ForEach(t => TourGuests.Add(new TourGuestViewModel(t.Id, t.FullName, t.Years, t.TourReservationId, t.CheckPointId)));
            checkPointRepository.GetAllCheckPointsByTourId(tour.Id).ForEach(cp => CheckPoints.Add(new CheckPointViewModel(cp.Id, cp.Name, cp.TourId, cp.IsChecked)));
            CheckPoints[0].IsChecked = true;
            CheckPoint cp = checkPointRepository.GetCheckPointById(CheckPoints[0].Id);
            cp.IsChecked = true;
            checkPointRepository.Update(cp);
            Numbers = new ObservableCollection<int>();
            AssignNumbersToCheckPoints();
            OnPropertyChanged(nameof(Numbers));
        }

        private CheckPointViewModel selectedCheckPoint;
        public CheckPointViewModel SelectedCheckPoint
        {
            get => selectedCheckPoint;
            set
            {
                if (value != selectedCheckPoint)
                {
                    selectedCheckPoint = value;
                    OnPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            SideBar.contentControlW.Content = new ViewMoreTourToday(tour, LoggedInUser);
        }
        private void AssignNumbersToCheckPoints()
        {
            for (int i = 0; i< CheckPoints.Count; i++)
            {
                Numbers.Add(i+1);
            }
        }


        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedCheckPoint = (CheckPointViewModel)CheckPointsListView.SelectedItem;

            if (selectedCheckPoint == null)
            {
                MessageBox.Show("Please select a CheckPoint.");
                return;
            }

            var selectedTourGuests = TourGuestsListView.SelectedItems.Cast<TourGuestViewModel>().ToList();

            selectedTourGuests.ForEach(tourGuest =>
            {
                UpdateTourGuestCheckpoint(tourGuest, selectedCheckPoint.Id);
                RemoveTourGuestFromList(tourGuest);
            });
        }

        private void UpdateTourGuestCheckpoint(TourGuestViewModel tourGuest, int checkPointId)
        {
            tourGuest.CheckPointId = checkPointId;

            TourGuest guest = new TourGuest(tourGuest.Id, tourGuest.FullName, tourGuest.Years, tourGuest.TourReservationId, tourGuest.CheckPointId);
            tourGuestRepository.UpdateTourGuest(guest);
        }

        private void RemoveTourGuestFromList(TourGuestViewModel tourGuest)
        {
            TourGuests.Remove(tourGuest);
        }

        private void Check_Click(object sender, MouseButtonEventArgs e)
        {
            if(SelectedCheckPoint != null)
            {
                Debug.WriteLine("AAAAAAAAaaaaaaaaaaaaa" +  SelectedCheckPoint.Id);
                for(int i = 0; i < CheckPoints.Count; i++)
                {
                    if(CheckPoints[i].Id == SelectedCheckPoint.Id)
                    {
                        CheckPoints[i].IsChecked = true;
                    }
                }
            }


            CheckPoint cp = checkPointRepository.GetCheckPointById(SelectedCheckPoint.Id);
            cp.IsChecked = true;
            checkPointRepository.Update(cp);
        }

        //KADA SE ZAVRSI TURA CP.ISCHECKED VRATITI SVE NA FALSE ZA NAREDNE REALIZACIJE


    }

}
