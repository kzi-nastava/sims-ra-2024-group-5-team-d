using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using BookingApp.Validation;
using BookingApp.WPF.Commands;
using BookingApp.WPF.ViewModels.TourViewModels.TourGuideViewModels;
using BookingApp.WPF.Views.TouristGuide;
using ceTe.DynamicPDF.PageElements;
using HarfBuzzSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;
using ToastNotifications.Position;
using Xceed.Wpf.Toolkit;

namespace BookingApp.WPF.ViewModels.TourViewModels.TourGuideViewModels
{
    public class CreateTourViewModel : ValidationBase
    {
        public ICommand ForwardCommand { get; private set; }
        public ICommand BackwardCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand LocationChangedCommand { get; private set; }
        public ICommand AddCheckPointCommand { get; private set; }
        public ICommand UploadCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public ICommand StartDemoCommand { get; private set; }


        private TourFormViewModel tourFormViewModel { get;  set; }
        public ObservableCollection<CheckPoint> CheckPoints { get; set; }
        public RequestViewModel Request { get; set; }
        public string ImagesPath { get; set; }
        private List<string> imagesPath;
        public ObservableCollection<string> ImagesPaths { get; set; }
        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }
        public User LoggedInUser { get; set; }

        private ImageUploaderService imageUploaderService;
        private CheckPointService checkPointService;
        private TourRealisationService tourRealisationService;
        private LocationService locationService;
        private TourService tourService;
        private TourRequestService tourRequestService;
        private TourReservationService tourReservationService;
        private NotificationsService notificationsService;
        private ComplexTourRequestService complexTourRequestService;

        private int PaginationIndex = 0;
        public bool IsRequest { get; set; }
        public bool IsLanguageStats { get; set; }
        public bool IsLocationsStats { get; set; }
        public bool IsSelectedDate { get; set; }
        public bool IsDuration { get; set; }

        public TourFormViewModel TourFormViewModel
        {
            get { return tourFormViewModel; }
            set
            {
                tourFormViewModel = value;
                OnPropertyChanged(nameof(TourFormViewModel));
            }
        }
        public CreateTourViewModel(User user) : this(user, null) { }

        public CreateTourViewModel(User user, RequestViewModel request)
        {
            InitializeCommon(user);
            if (request != null)
            {
                IsSelectedDate = false;
                IsRequest = true;
                IsLocationsStats = true;
                IsLanguageStats = true;
                Request = request;
                MinDate = request.DateFrom;
                MaxDate = request.DateTo;
                InitializeFromRequest(request);
                
            }
            else
            {
                IsRequest = false;
                IsLocationsStats = false;
                IsLanguageStats = false;
                MinDate = DateTime.Now.AddMinutes(-1);
                MaxDate = DateTime.Now.AddYears(1);
            }
        }

        public CreateTourViewModel(User user, int locationId, int languageId) 
        {
            InitializeCommon(user);
            if(locationId != -1)
            {
                TourFormViewModel.LocationId = locationId;
                IsLocationsStats = true;
                LocationChanged();
            }
            else
            {
                TourFormViewModel.LanguageId = languageId;
                IsLanguageStats = true;
            }
            MinDate = DateTime.Now.AddMinutes(-1);
            MaxDate = DateTime.Now.AddYears(1);

        }

        private void InitializeCommon(User user)
        {
            LoggedInUser = user;
            TourFormViewModel = new TourFormViewModel();
            TourFormViewModel.User = user;
            TourFormViewModel.StartTime = DateTime.Now;

            tourRealisationService = new TourRealisationService();
            checkPointService = new CheckPointService();
            tourService = new TourService();
            locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            imageUploaderService = new ImageUploaderService(tourService);
            notificationsService = new NotificationsService();
            complexTourRequestService = new ComplexTourRequestService();

            SaveCommand = new RelayCommand(Save);
            LocationChangedCommand = new RelayCommand(LocationChanged);
            AddCheckPointCommand = new RelayParameterCommand(AddCheckPoint);
            UploadCommand = new RelayCommand(UploadPicture);
            BackwardCommand = new RelayCommand(Backward);
            ForwardCommand = new RelayCommand(Forward);
            CancelCommand = new RelayCommand(Cancel);
            StartDemoCommand = new RelayCommand(StartDemo);

            CheckPoints = new ObservableCollection<CheckPoint>(checkPointService.SuggestCheckPoints(TourFormViewModel.LocationId));
            imagesPath = new List<string>();
            ImagesPaths = new ObservableCollection<string>();
        }

        private void InitializeFromRequest(RequestViewModel request)
        {
            tourReservationService = new TourReservationService();
            tourRequestService = new TourRequestService();
            TourFormViewModel.Capacity = request.Capacity;
            TourFormViewModel.Description = request.Description;
            TourFormViewModel.LanguageId = Convert.ToInt32(request.Language);
            TourFormViewModel.LocationId = request.Location.Id;
            if(request.SelectedDate != null)
            TourFormViewModel.Capacity = request.Capacity;
            TourFormViewModel.Description = request.Description;
            TourFormViewModel.LanguageId = Convert.ToInt32(request.Language);
            TourFormViewModel.LocationId = request.Location.Id;
            if(tourRequestService.GetAllSimpleRequests().Any(req => req.Id == request.Id))
            {
                IsSelectedDate = true;
                IsDuration = true;
                TourFormViewModel.StartTime = request.SelectedDate.AddHours(request.SelectedTime.Hour);
                TourFormViewModel.Duration = 2;
                Debug.WriteLine("Date: " + request.SelectedDate + "Time: " + request.SelectedTime.Hour);
                TourFormViewModel.StartTime = request.SelectedDate;
                TourFormViewModel.StartTime = TourFormViewModel.StartTime.AddHours(request.SelectedTime.Hour);
                Debug.WriteLine(TourFormViewModel.StartTime);
                tourFormViewModel.Duration = 2;
            }
            LocationChanged();
        }
        private async void StartDemo()
        {
            await Task.Delay(500);

            HighlightInputField(CreateNewTourForm.NameInput);
            await TypeStringLetterByLetter("Demo Tour", letter => TourFormViewModel.Name += letter);
            RevertHighlightInputField(CreateNewTourForm.NameInput);

            await SelectComboBoxItemAsync(CreateNewTourForm.LocationComboBox, "Serbia, Novi Sad");

            await Task.Delay(500);
            HighlightInputField(CreateNewTourForm.DurationInput);
            TourFormViewModel.Duration = 2;
            RevertHighlightInputField(CreateNewTourForm.DurationInput);

            await Task.Delay(500);
            HighlightInputField(CreateNewTourForm.CapacityInput);
            TourFormViewModel.Capacity = 20;
            RevertHighlightInputField(CreateNewTourForm.CapacityInput);

            await Task.Delay(500);
            await SelectComboBoxItemAsync(CreateNewTourForm.LanguageComboBox, "English");

            await Task.Delay(500);
            HighlightInputField(CreateNewTourForm.DateTimePicker);
            CreateNewTourForm.DateTimePicker.IsOpen = true;
            await Task.Delay(500);
            CreateNewTourForm.DateTimePicker.Value = DateTime.Now.AddDays(1);
            CreateNewTourForm.DateTimePicker.IsOpen = false;
            RevertHighlightInputField(CreateNewTourForm.DateTimePicker);

            await Task.Delay(500);
            CreateNewTourForm.ImageUploadButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#218C89"));
            string demoImagePath = "C:\\Users\\milan\\OneDrive\\Radna površina\\SIMPROJEKAT\\sims-ra-2024-group-5-team-d\\Resources\\TourImages\\Tour1\\beograd-na-vodi.jpg";
            imagesPath.Add(demoImagePath);
            ImagesPaths.Clear();
            ImagesPaths.Add(demoImagePath);
            PaginationIndex = imagesPath.Count - 1;
            await Task.Delay(500);
            CreateNewTourForm.ImageUploadButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00FFFFFF"));

            HighlightInputField(CreateNewTourForm.DescriptionInput);
            await TypeStringLetterByLetter("This is a demo tour description.", letter => TourFormViewModel.Description += letter);
            RevertHighlightInputField(CreateNewTourForm.DescriptionInput);

            await Task.Delay(500);
            HighlightInputField(CreateNewTourForm.CheckBoxInput);
            await TypeStringLetterByLetter("Demo Checkpoint 1", letter => CreateNewTourForm.CheckBoxInput.Text += letter);
            CreateNewTourForm.CheckPointAddButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#218C89"));
            await Task.Delay(500);

            AddCheckPoint("Demo Checkpoint 1");
            CreateNewTourForm.CheckBoxInput.Clear();
            RevertHighlightInputField(CreateNewTourForm.CheckBoxInput);
            CreateNewTourForm.CheckPointAddButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#218C89"));
            await Task.Delay(500);
            CreateNewTourForm.CheckPointAddButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00FFFFFF"));
            await Task.Delay(500);
            HighlightInputField(CreateNewTourForm.CheckBoxInput);
            await TypeStringLetterByLetter("Demo Checkpoint 2", letter => CreateNewTourForm.CheckBoxInput.Text += letter);
            AddCheckPoint("Demo Checkpoint 2");
            CreateNewTourForm.CheckPointAddButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#218C89"));
            await Task.Delay(500);
            CreateNewTourForm.CheckPointAddButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00FFFFFF"));
            CreateNewTourForm.CheckBoxInput.Clear();
            RevertHighlightInputField(CreateNewTourForm.CheckBoxInput);
            System.Windows.MessageBox.Show("End of demo", "Demonstration");
            Cancel();
        }

        private void HighlightInputField(Control inputField)
        {
            inputField.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#218C89"));
            inputField.BorderThickness = new Thickness(2);
        }

        private void RevertHighlightInputField(Control inputField)
        {
            Task.Delay(500).ContinueWith(_ =>
            {
                inputField.Dispatcher.Invoke(() =>
                {
                    inputField.BorderBrush = Brushes.Transparent;
                    inputField.BorderThickness = new Thickness(1);
                });
            });
        }

        private async Task SelectComboBoxItemAsync(ComboBox comboBox, string itemText)
        {
            comboBox.IsDropDownOpen = true;
            await Task.Delay(500);
            comboBox.SelectedItem = comboBox.Items.OfType<ComboBoxItem>().FirstOrDefault(item => item.Content.ToString() == itemText);
            comboBox.IsDropDownOpen = false;
        }

        private async Task TypeStringLetterByLetter(string text, Action<char> appendAction)
        {
            foreach (var letter in text)
            {
                appendAction(letter);
                await Task.Delay(100);
            }
        }


        public void Backward()
        {
            PaginationIndex--;
            ImagesPaths.Clear();
            ImagesPaths.Add(imagesPath[PaginationIndex]);
        }
        public void Forward()
        {
            PaginationIndex++;
            ImagesPaths.Clear();
            ImagesPaths.Add(imagesPath[PaginationIndex]);
        }

        private void UploadPicture()
        {
            string imagePath = imageUploaderService.UploadImage();
            if (imagePath != null)
            {
                imagesPath.Add(imagePath);
                ImagesPaths.Clear();
                ImagesPaths.Add(imagePath);
                PaginationIndex = imagesPath.Count - 1;
            }
        }
        private void Save()
        {
            TourFormViewModel.Validate();
            this.Validate();
            if (!TourFormViewModel.IsValid)
                return;
            if (!tourService.AmIAvailable(LoggedInUser, TourFormViewModel.StartTime, TourFormViewModel.Duration))
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("You are busy on this term");
                return;
            }
            string folderPath = imageUploaderService.CreateTourFolder(imagesPath);

            Tour newTour = new Tour(TourFormViewModel.Name, locationService.GetById(TourFormViewModel.LocationId), TourFormViewModel.Description, (LANGUAGE)TourFormViewModel.LanguageId, TourFormViewModel.Capacity, TourFormViewModel.Duration, folderPath, TourFormViewModel.User);
            Tour SavedTour = tourService.Save(newTour);
            TourRealisation newTourRealisation = new TourRealisation(TourFormViewModel.StartTime, SavedTour.Id, TourFormViewModel.Capacity, TourFormViewModel.User);
            TourRealisation savedTourRealisation = tourRealisationService.Save(newTourRealisation);
            SaveCheckPoints(SavedTour.Id);
            UpdateTourRequestStatus(savedTourRealisation.Id);
            if (IsLanguageStats)
            {
                notificationsService.SendNotificationForWantedLanguage(SavedTour);
            }else if (IsLocationsStats)
            {
                notificationsService.SendNotificationForWantedLocation(SavedTour);
            }
            SideBar.contentControlW.Content = new CreateNewTourForm(TourFormViewModel.User);
        }
        private void SaveCheckPoints(int tourId)
        {
            List<CheckPoint> selectedCheckPoints = CheckPoints.Where(cp => cp.IsChecked).ToList();
            selectedCheckPoints.ForEach(cp => cp.IsChecked = false);
            selectedCheckPoints.ForEach(cp => cp.TourId = tourId);
            selectedCheckPoints.ForEach(cp => checkPointService.Save(cp));
        }

        private void UpdateTourRequestStatus(int tourRealisationId)
        {
            if (IsRequest)
            {
                TourRequest request = tourRequestService.GetById(Request.Id);
                TourReservation tourReservation = tourReservationService.GetById(request.TourReservationId);
                tourReservation.TourRealisationId = tourRealisationId;
                tourReservationService.Update(tourReservation);
                request.Status = STATE.ACCEPTED;
                TourRealisation savedTourRealisation = tourRealisationService.GetById(tourRealisationId);
                savedTourRealisation.AvailableSeats = 0;
                tourRealisationService.Update(savedTourRealisation);
                tourRequestService.Update(request);
                notificationsService.SendAcceptedRequestNotification(request.TouristId, request.Id);
            }
            IsRequest = false;
        }

        private void AddCheckPoint(object parameter)
        {
            string labelText = parameter as string;
            CheckPoint newCheckPoint = new CheckPoint(labelText, tourService.NextId(), true);
            CheckPoints.Insert(0, newCheckPoint);
        }
        private void LocationChanged()
        {
            CheckPoints.Clear();
            checkPointService.SuggestCheckPoints(TourFormViewModel.LocationId).ForEach(cp =>  CheckPoints.Add(cp));
        }
        public void Cancel()
        {
            SideBar.contentControlW.Content = new CreateNewTourForm(TourFormViewModel.User);
        }

        protected override void ValidateSelf()
        {
            if (this.ImagesPaths.Count() < 1)
            {
                this.ValidationErrors["Images"] = "Please upload a picture.";
            }
            if (this.CheckPoints.Where(cp => cp.IsChecked).ToList().Count() < 2)
            {
                this.ValidationErrors["CP"] = "Please select two checkpoints minimum.";
            }
        }
    }
}
