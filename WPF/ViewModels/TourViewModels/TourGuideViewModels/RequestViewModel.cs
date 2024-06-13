using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.WPF.Commands;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Packaging;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels.TourViewModels.TourGuideViewModels
{
    public class RequestViewModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public Location Location { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int Capacity { get; set; }
        public LANGUAGE Language { get; set; }
        public User Tourist { get; set; }
        public bool IsAcceptable { get; set; }
        public User LoggedInUser {  get; set; }
        public ICommand ComboBoxSelectionChangedCommand { get; set; }
        public TourService tourService { get; set; }
        public ComplexTourRequestService complexRequestService { get; set; }

        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged();
            }
        }
        private TimeOnly _selectedTime = TimeOnly.MinValue;
        public TimeOnly SelectedTime
        {
            get => _selectedTime;
            set
            {
                _selectedTime = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<TimeOnly> _availableDates;
        public ObservableCollection<TimeOnly> AvailableDates
        {
            get => _availableDates;
            set
            {
                _availableDates = value;
                OnPropertyChanged();
            }
        }


        public ObservableCollection<string> DateRange { get; set; }

        public RequestViewModel() { }
        public RequestViewModel(int id,string description, Location location, DateTime dateFrom, DateTime dateTo, int capacity, LANGUAGE language,User tourist, bool isAcceptable)
        {
            Id = id;
            Description = description;
            Location = location;
            DateFrom = dateFrom;
            DateTo = dateTo;
            Capacity = capacity;
            Language = language;
            Tourist = tourist;
            IsAcceptable = isAcceptable;

            ComboBoxSelectionChangedCommand = new RelayParameterCommand(OnComboBoxSelectionChanged);
            tourService = new TourService();
            complexRequestService = new ComplexTourRequestService();

            AvailableDates = new ObservableCollection<TimeOnly>();
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnComboBoxSelectionChanged(object parameter)

        {
            if (parameter != null)
            {
                string selectedValue = parameter.ToString();
                if (!string.IsNullOrEmpty(selectedValue))
                {
                    Debug.WriteLine("Selected value: " + selectedValue);

                    if (DateTime.TryParse(selectedValue, out DateTime selectedDate))
                    {
                        Debug.WriteLine("Selected date: " + selectedDate);
                        SelectedDate = selectedDate;
                        GenerateSuggestions();
                        complexRequestService.FixPotentialDateTimeOverLaping(this);
                        
                    }
                    else
                    {
                        Debug.WriteLine("Failed to parse selected date.");
                    }
                }
                else
                {
                    Debug.WriteLine("Selected value is empty or null.");
                }
            }
            else
            {
                Debug.WriteLine("Parameter is null.");
            }
        }
        private void GenerateSuggestions()
        {
            AvailableDates.Clear();
            for (int i = 0; i < 24; i+=2)
            {
                DateTime potentialStartTime = SelectedDate.AddHours(i);
                if (tourService.AmIAvailable(LoggedInUser, potentialStartTime, 2))
                {
                    AvailableDates.Add(TimeOnly.FromDateTime(potentialStartTime));
                }
            }

        }

        public ObservableCollection<string> GetRangeOfDates(User user)
        {
            DateRange = new ObservableCollection<string>();
            LoggedInUser = user;
            for (DateTime date = DateFrom; date <= DateTo; date = date.AddDays(1))
            {
                DateRange.Add(date.ToString("MM/dd/yyyy"));
            }
            return DateRange;
        }

    }
}
