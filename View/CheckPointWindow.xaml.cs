using BookingApp.Model;
using BookingApp.Repository;
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
using System.Windows.Shapes;

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for CheckPointWindow.xaml
    /// </summary>
    public partial class CheckPointWindow : Window
    {
        public ObservableCollection<CheckPoint> _checkPoints;
        private readonly CheckPointRepository _repository;
        private int tourId;
        private int tourRealisationId;
        private readonly TourRepository _tourRepository;
        private readonly TourGuestRepository _guestRepository;
        public TouristGuideWindow touristGuideWindow { get; set; }

        public ObservableCollection<CheckPoint> CheckPoints
        {
            get { return _checkPoints; }
            set
            {
                if (_checkPoints != value)
                {
                    _checkPoints = value;
                    OnPropertyChanged(nameof(CheckPoints));
                }
            }
        }

        private bool isChecked;
        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                if (isChecked != value)
                {
                    isChecked = value;
                    OnPropertyChanged(nameof(IsChecked));
                }
            }
        }
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    Name = name;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public CheckPointWindow(int tourId, int tourRealisationId, User user)
        {
            InitializeComponent();
            DataContext = this;
            this.tourId = tourId;
            this.tourRealisationId = tourRealisationId;
            _repository = new CheckPointRepository();
            _tourRepository = new TourRepository();
            touristGuideWindow = new TouristGuideWindow(user);
            CheckPoints = new ObservableCollection<CheckPoint>(_repository.GetAllCheckPointsByTourId(tourId));
            if(CheckPoints.Count > 0)
            {
                CheckPoints.First().IsChecked = true;
                _repository.Update(CheckPoints.First());
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            int? checkedCheckPointId = GetCheckedCheckPointId();
            if (checkedCheckPointId != null)
            {
                CheckBox checkBox = sender as CheckBox;
                CheckPoint checkPoint = _repository.GetCheckPointById(checkedCheckPointId);
                if (checkBox.IsChecked == true)
                {
                    checkPoint.IsChecked = true;
                    _repository.Update(checkPoint);
                    CheckPoints = new ObservableCollection<CheckPoint>(_repository.GetAllCheckPointsByTourId(tourId));
                    if (_checkPoints.Last().IsChecked)
                    {
                        FinishTour_Click(sender, e);
                    }
                }
                else
                {
                    checkPoint.IsChecked = false;
                    _repository.Update(checkPoint);
                }
            }
                
        }

        private int? GetCheckedCheckPointId()
        {
            if (checkBox.SelectedItem != null)
            {
                var selectedCheckPoint = checkBox.SelectedItem as CheckPoint;

                return selectedCheckPoint.Id;
            }
            else return null;
        }

        private void FinishTour_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Tour is finished");
            Close();
        }

        
        private void RegisterTouristOnCheckPoint_Click(object sender, RoutedEventArgs e)
        {
            TourGuestCheckPointList tourGuestCheckPointList = new TourGuestCheckPointList(checkBox.SelectedItem,tourId,tourRealisationId);
            tourGuestCheckPointList.Show();
        }
    }
}
