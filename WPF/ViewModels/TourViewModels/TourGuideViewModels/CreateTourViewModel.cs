using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using BookingApp.WPF.Commands;
using BookingApp.WPF.ViewModels.TourViewModels.TourGuideViewModels;
using BookingApp.WPF.Views.TouristGuide;
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
    public class CreateTourViewModel
    {
        public ICommand ForwardCommand { get; private set; }
        public ICommand BackwardCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand LocationChangedCommand { get; private set; }
        public ICommand AddCheckPointCommand { get; private set; }
        public ICommand UploadCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public ICommand StartDemoCommand { get; private set; }


        public TourFormViewModel tourFormViewModel { get; private set; }
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

        private int PaginationIndex = 0;
        public bool IsRequest { get; set; }
        public bool IsLanguageStats { get; set; }
        public bool IsLocationsStats { get; set; }
        public CreateTourViewModel(User user) : this(user, null) { }

        public CreateTourViewModel(User user, RequestViewModel request)
        {
            InitializeCommon(user);
            if (request != null)
            {
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
                tourFormViewModel.LocationId = locationId;
                IsLocationsStats = true;
                LocationChanged();
            }
            else
            {
                tourFormViewModel.LanguageId = languageId;
                IsLanguageStats = true;
            }
            MinDate = DateTime.Now.AddMinutes(-1);
            MaxDate = DateTime.Now.AddYears(1);

        }

        private void InitializeCommon(User user)
        {
            LoggedInUser = user;
            tourFormViewModel = new TourFormViewModel();
            tourFormViewModel.User = user;
            tourFormViewModel.StartTime = DateTime.Now;

            tourRealisationService = new TourRealisationService();
            checkPointService = new CheckPointService();
            tourService = new TourService();
            locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            imageUploaderService = new ImageUploaderService(tourService);
            notificationsService = new NotificationsService();

            SaveCommand = new RelayCommand(Save);
            LocationChangedCommand = new RelayCommand(LocationChanged);
            AddCheckPointCommand = new RelayParameterCommand(AddCheckPoint);
            UploadCommand = new RelayCommand(UploadPicture);
            BackwardCommand = new RelayCommand(Backward);
            ForwardCommand = new RelayCommand(Forward);
            CancelCommand = new RelayCommand(Cancel);
            StartDemoCommand = new RelayCommand(StartDemo);

            CheckPoints = new ObservableCollection<CheckPoint>(checkPointService.SuggestCheckPoints(tourFormViewModel.LocationId));
            imagesPath = new List<string>();
            ImagesPaths = new ObservableCollection<string>();
        }

        private void InitializeFromRequest(RequestViewModel request)
        {
            tourReservationService = new TourReservationService();
            tourRequestService = new TourRequestService();
            tourFormViewModel.Capacity = request.Capacity;
            tourFormViewModel.Description = request.Description;
            tourFormViewModel.LanguageId = Convert.ToInt32(request.Language);
            tourFormViewModel.LocationId = request.Location.Id;
            LocationChanged();
        }
        private async void StartDemo()
        {
            await Task.Delay(500);

            HighlightInputField(CreateNewTourForm.NameInput);
            await TypeStringLetterByLetter("Demo Tour", letter => tourFormViewModel.Name += letter);
            RevertHighlightInputField(CreateNewTourForm.NameInput);

            await SelectComboBoxItemAsync(CreateNewTourForm.LocationComboBox, "Serbia, Novi Sad");

            await Task.Delay(500);
            HighlightInputField(CreateNewTourForm.DurationInput);
            tourFormViewModel.Duration = 2;
            RevertHighlightInputField(CreateNewTourForm.DurationInput);

            await Task.Delay(500);
            HighlightInputField(CreateNewTourForm.CapacityInput);
            tourFormViewModel.Capacity = 20;
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

            HighlightInputField(CreateNewTourForm.DescriptionInput);
            await TypeStringLetterByLetter("This is a demo tour description.", letter => tourFormViewModel.Description += letter);
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

            await Task.Delay(500);
            HighlightInputField(CreateNewTourForm.CheckBoxInput);
            await TypeStringLetterByLetter("Demo Checkpoint 2", letter => CreateNewTourForm.CheckBoxInput.Text += letter);
            AddCheckPoint("Demo Checkpoint 2");
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
            if (!tourService.AmIAvailable(LoggedInUser, tourFormViewModel.StartTime, tourFormViewModel.Duration))
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("You are busy on this term");
                return;
            }
            string folderPath = imageUploaderService.CreateTourFolder(imagesPath);

            Tour newTour = new Tour(tourFormViewModel.Name, locationService.GetById(tourFormViewModel.LocationId), tourFormViewModel.Description, (LANGUAGE)tourFormViewModel.LanguageId, tourFormViewModel.Capacity, tourFormViewModel.Duration, folderPath, tourFormViewModel.User);
            Tour SavedTour = tourService.Save(newTour);
            TourRealisation newTourRealisation = new TourRealisation(tourFormViewModel.StartTime, SavedTour.Id, tourFormViewModel.Capacity, tourFormViewModel.User);
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
            SideBar.contentControlW.Content = new CreateNewTourForm(tourFormViewModel.User);
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
            checkPointService.SuggestCheckPoints(tourFormViewModel.LocationId).ForEach(cp =>  CheckPoints.Add(cp));
        }
        public void Cancel()
        {
            SideBar.contentControlW.Content = new CreateNewTourForm(tourFormViewModel.User);
        }
    }
}
