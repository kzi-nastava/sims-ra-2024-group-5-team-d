using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.ViewModels;
using BookingApp.WPF.Views.OwnerView;
using BookingApp.WPF.Views.Utils;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.WPF.Views.TouristView
{
    /// <summary>
    /// Interaction logic for TourDetailsUserControl.xaml
    /// </summary>
    public partial class TourDetailsUserControl : UserControl, INotifyPropertyChanged
    {
        public bool IsRealisationSelected { get; set; }

        private int numberOfSeats;

        public int NumberOfSeats
        {
            get => numberOfSeats;
            set
            {
                if (value != numberOfSeats)
                {
                    numberOfSeats = value;
                    OnPropertyChanged();
                }
            }
        }

        private TimeOnly tourStartTime;
        public TimeOnly TourStartTime
        {
            get => tourStartTime;
            set
            {
                if (value != tourStartTime)
                {
                    tourStartTime = value;
                    OnPropertyChanged();
                }
            }
        }

        private TimeOnly tourEndTime;
        public TimeOnly TourEndTime
        {
            get => tourEndTime;
            set
            {
                if (value != tourEndTime)
                {
                    tourEndTime = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime selectedDate;
        public DateTime SelectedDate
        {
            get => selectedDate;
            set
            {
                if (value != selectedDate)
                {
                    selectedDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime date;
        public DateTime Date
        {
            get => date;
            set
            {
                if (value != date)
                {
                    date = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime minAvailDate;
        public DateTime MinAvailDate
        {
            get => minAvailDate;
            set
            {
                if (value != minAvailDate)
                {
                    minAvailDate = value;
                    OnPropertyChanged();
                }
            }
        }

        

        private DateTime cancelationDue;
        public DateTime CancelationDue
        {
            get => cancelationDue;
            set
            {
                if (value != cancelationDue)
                {
                    cancelationDue = value;
                    OnPropertyChanged();
                }
            }
        }

        private int numberOfTourists;

        public int NumberOfTourists
        {
            get => numberOfTourists;
            set
            {
                if (value != numberOfTourists)
                {
                    numberOfTourists = value;
                    OnPropertyChanged();
                }
            }
        }
        public string LocationName { get; set; }
        public int NumberOfCheckpoints { get; set; }
        public TourViewModel SelectedTourOnSameLocation { get; set; }

        public TourRealisationViewModel SelectedRealisation { get; set; }
        public TourViewModel SelectedTour { get; set; }
        public ObservableCollection<TourRealisationViewModel> TourRealisations { get; set; }
        public ObservableCollection<CheckPointViewModel> Checkpoints { get; set; }
        public ObservableCollection<TourViewModel> ToursOnSameLocation { get; set; }

        private TourRealisationService tourRealisationService;
        private TourService tourService;

        private LocationService locationService;

        public CheckPointService checkPointService { get; set; }
        public User User { get; set; }

        public TourDetailsUserControl(TourViewModel selectedTour, int numberOfPeople, User user)
        {
            InitializeComponent();

            locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            checkPointService = new CheckPointService();
            tourRealisationService = new TourRealisationService();
            tourService = new TourService();
            MinAvailDate = DateTime.UtcNow;
            DataContext = this;

            User = user;
            IsRealisationSelected = true;
            Date = DateTime.UtcNow;
            LocationName = locationService.GetById(selectedTour.Location.Id).ToString();
            numberOfTourists = numberOfPeople;
            SelectedTour = selectedTour;

            ToursOnSameLocation = new ObservableCollection<TourViewModel>();
            tourService.GetAllTours().ForEach(tour => { 
                if(tour.Location.Id == selectedTour.Location.Id && tour.Id != selectedTour.Id)
                    ToursOnSameLocation.Add(new TourViewModel(tour.Id, tour.Name, tour.Description, tour.Location, tour.Duration, tour.ImagesPath, tour.MaxCapacity, tour.Language, tour.User));
            });

            var sortedTours = ToursOnSameLocation.OrderByDescending(tour => tour.IsSuperGuide).ToList();
            ToursOnSameLocation.Clear();
            foreach (var tour in sortedTours)
            {
                ToursOnSameLocation.Add(tour);
            }

            Checkpoints = new ObservableCollection<CheckPointViewModel>();
            checkPointService.GetAllCheckPointsByTourId(selectedTour.Id).ForEach(checkpoint => Checkpoints.Add(new CheckPointViewModel(checkpoint.Id, checkpoint.Name)));
            NumberOfCheckpoints = Checkpoints.Count();

            TourRealisations = new ObservableCollection<TourRealisationViewModel>();

            
            CreateProgressSteps();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void IncreaseCount_Click(object sender, RoutedEventArgs e)
        {
            //int currentCount = Convert.ToInt32(PeopleCounter.Text.Split(" ")[0]);
            ++NumberOfTourists;
            Debug.WriteLine(numberOfTourists);
            Debug.WriteLine(SelectedTour.Capacity);
        }

        private void DecreaseCount_Click(object sender, RoutedEventArgs e)
        {
            if (numberOfTourists > 1)
            {
                --NumberOfTourists;
            }
        }

        private void CreateProgressSteps()
        {
            int numberOfSteps = NumberOfCheckpoints;
            double stepWidth = (progressBar.Width - (numberOfSteps - 1) * 40) / (double)(numberOfSteps - 1);
            double textBoxStep = (progressBar.Width - (numberOfSteps - 1) * 25) / (double)(numberOfSteps - 1);

            Debug.WriteLine(stepWidth);

            for (int i = 0; i < numberOfSteps; i++)
            {
                Ellipse ellipse = new Ellipse
                {
                    Width = 40,
                    Height = 40,
                    Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#218C89")),
                    Cursor = Cursors.Hand
                };

                TextBox textbox1 = new TextBox
                {
                    Text = $"{i + 1}",
                    IsReadOnly = true,
                    Width = 25,
                    Height = 25,
                    BorderThickness = new Thickness(0),
                    FontSize = 20,
                    VerticalContentAlignment = VerticalAlignment.Top,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2FC8BF")),
                    Background = Brushes.Transparent,
                    FontWeight = FontWeights.Bold,
                    FontStyle = FontStyles.Normal,
                    FontFamily = new FontFamily("Segoe UI")
                };

                TextBox textbox = new TextBox
                {
                    Text = Checkpoints[i].Name,
                    IsReadOnly = true,
                    Width = 200,
                    Height = 40,
                    Background = Brushes.Transparent,
                    BorderBrush = Brushes.Transparent,
                    FontSize = 18,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    HorizontalContentAlignment = HorizontalAlignment.Left,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#218C89")),
                    FontWeight = FontWeights.DemiBold,
                    FontStyle = FontStyles.Normal,
                    FontFamily = new FontFamily("Segoe UI")
                };

                if (i == 0)
                {
                    ellipse.Margin = new Thickness(200, 20, 0, 0);
                    textbox1.Margin = new Thickness(207, 25, 0, 0);
                    textbox.Margin = new Thickness(250, 20, 0, 0);
                }
                else
                {
                    ellipse.Margin = new Thickness(200, stepWidth, 0, 0);
                    textbox1.Margin = new Thickness(207, textBoxStep, 0, 0);
                    textbox.Margin = new Thickness(250, stepWidth, 0, 0);
                }

                numberContainer.Children.Add(textbox1);
                namesContainer.Children.Add(textbox);
                progressContainer.Children.Add(ellipse);
            }
        }


        private void ScrollToPosition(int position)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = MainScroll.VerticalOffset;
            animation.To = position;
            animation.Duration = new Duration(TimeSpan.FromSeconds(0.5));

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(animation);
            Storyboard.SetTarget(animation, MainScroll);
            Storyboard.SetTargetProperty(animation, new PropertyPath(ScrollViewerBehavior.VerticalOffsetProperty));

            storyboard.Begin();
        }


        private void CheckAvailabilty_Click(object sender, RoutedEventArgs e)
        {
            if (tourService.HasAvailableSeatsInAnyRealisation(SelectedTour.Id))
            {
                TourRealisations.Clear();
                SelectedDate = Date;
                NumberOfSeats = NumberOfTourists;
                tourRealisationService.GetRealisationsForTourOnGivenDate(SelectedTour.Id, DateOnly.FromDateTime(Date))
                    .ForEach(tR => 
                    {
                        if(tR.IsFinished == false && tR.IsLive == false)                            
                            TourRealisations.Add(new TourRealisationViewModel(tR.Id, tR.StartTime, tR.TourId, tR.AvailableSeats, SelectedTour.Duration, tR.User, NumberOfTourists));                    
                    });
                onClickVisible.Visibility = Visibility.Visible;
            }
            else
                onClickVisible2.Visibility = Visibility.Visible;

            ScrollToPosition(200);
        }

        private void Promeni_Click(object sender, MouseButtonEventArgs e)
        {
           
        }

        private void BookNow_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedRealisation != null && !SelectedRealisation.NotEnoughSpace)
                TouristHomeWindow.contentControl.Content = new BookingSection(SelectedTour, SelectedRealisation, User, NumberOfSeats);
        }

        private void SecondaryTourChoice_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            TouristHomeWindow.contentControl.Content = new TourDetailsUserControl(SelectedTourOnSameLocation, NumberOfTourists, User);
        }

        private void ListView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedRealisation != null)
            {
                TourEndTime = SelectedRealisation.EndTime;
                TourStartTime = SelectedRealisation.StartTime;
                IsRealisationSelected = true;
                CancelationDue = SelectedRealisation.DateTime.AddDays(-2);
            }
        }

        private void ListView_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if(SelectedTourOnSameLocation != null)
            {
                TouristHomeWindow.contentControl.Content = new TourDetailsUserControl(SelectedTourOnSameLocation, NumberOfTourists, User);
            }
        }
    }
}
