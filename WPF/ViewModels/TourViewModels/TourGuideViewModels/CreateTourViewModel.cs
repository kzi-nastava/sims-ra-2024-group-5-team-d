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
using System.Windows.Controls;
using System.Windows.Input;
using ToastNotifications.Position;

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
        

        public TourFormViewModel tourFormViewModel { get; private set; }
        public ObservableCollection<CheckPoint> CheckPoints { get; set; }
        public string ImagesPath { get; set; }
        private List<string> imagesPath;
        public ObservableCollection<string> ImagesPaths { get; set; }
        private List<CheckPoint> checkPointsToSave { get; set; }

        private ImageUploaderService imageUploaderService;
        private CheckPointService checkPointService;
        private TourRealisationService tourRealisationService;
        private LocationService locationService;
        private TourService tourService;

        private int PaginationIndex = 0;
        public CreateTourViewModel(User user) 
        {
            tourFormViewModel = new TourFormViewModel();
            tourFormViewModel.User = user;
            tourFormViewModel.StartTime = DateTime.Now;

            tourRealisationService = new TourRealisationService();
            checkPointService = new CheckPointService();
            tourService = new TourService();
            locationService = new LocationService();
            imageUploaderService = new ImageUploaderService();

            SaveCommand = new RelayCommand(Save);
            LocationChangedCommand = new RelayCommand(LocationChanged);
            AddCheckPointCommand = new RelayParameterCommand(AddCheckPoint);
            UploadCommand = new RelayCommand(UploadPicture);
            BackwardCommand = new RelayCommand(Backward);
            ForwardCommand = new RelayCommand(Forward);

            CheckPoints = new ObservableCollection<CheckPoint>(checkPointService.SuggestCheckPoints(tourFormViewModel.LocationId));
            checkPointsToSave = new List<CheckPoint>();
            imagesPath = new List<string>();
            ImagesPaths = new ObservableCollection<string>();
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
            string folderPath = imageUploaderService.CreateTourFolder(imagesPath);

            Tour newTour = new Tour(tourFormViewModel.Name, locationService.GetById(tourFormViewModel.LocationId), tourFormViewModel.Description, (LANGUAGE)tourFormViewModel.LanguageId, tourFormViewModel.Capacity, tourFormViewModel.Duration, folderPath, tourFormViewModel.User);
            Tour SavedTour = tourService.Save(newTour);
            TourRealisation newTourRealisation = new TourRealisation(tourFormViewModel.StartTime, SavedTour.Id, tourFormViewModel.Capacity, tourFormViewModel.User);
            TourRealisation savedTourRealisation = tourRealisationService.Save(newTourRealisation);
            //proci kroz sve selektovane cp i dodati ih u checkPointsToSave

            checkPointsToSave = CheckPoints.Where(cp => cp.IsChecked == true).ToList();

            checkPointsToSave.ForEach(cp => cp.IsChecked = false);
            checkPointsToSave.ForEach(cp => cp.TourId = SavedTour.Id);
            checkPointsToSave.ForEach(cp => checkPointService.Save(cp));
            SideBar.contentControlW.Content = new CreateNewTourForm(tourFormViewModel.User);
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

    }
}
