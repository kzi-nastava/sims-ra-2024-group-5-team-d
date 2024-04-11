using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using BookingApp.WPF.Commands;
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

namespace BookingApp.WPF.ViewModels
{
    public class CreateTourViewModel
    {
        public ICommand SaveCommand { get; private set; }
        public ICommand LocationChangedCommand { get; private set; }

        public ICommand AddCheckPointCommand { get; private set; }
        public ICommand UploadCommand { get; private set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int LocationId { get; set; }
        public double Duration { get; set; }
        public string Description { get; set; }
        public int LanguageId { get; set; }
        public int Capacity { get; set; }
        public DateTime DateTime { get; set; }
        public string ImagesPath { get; set; }
        public User LoggedInUser {  get; set; }
        public Tour SavedTour { get; set; }
        public ObservableCollection<CheckPoint> CheckPoints { get; set; }
        private List<CheckPoint> checkPointsToSave { get; set; }
        private ImageUploaderService imageUploaderService;
        private readonly ILocationRepository locationRepository;
        private ITourRepository tourRepository;
        private ITourRealisationRepository tourRealisationRepository;
        private ICheckPointRepository checkPointRepository;
        private List<string> imagesPath;
        public CreateTourViewModel(User user) 
        {
            LoggedInUser = user;
            imagesPath = new List<string>();
            locationRepository = Injector.CreateInstance<ILocationRepository>();
            tourRepository = Injector.CreateInstance<ITourRepository>();
            tourRealisationRepository = Injector.CreateInstance<ITourRealisationRepository>();
            checkPointRepository = Injector.CreateInstance<ICheckPointRepository>();
            imageUploaderService = new ImageUploaderService();
            SaveCommand = new RelayCommand(Save);
            LocationChangedCommand = new RelayCommand(LocationChanged);
            AddCheckPointCommand = new RelayParameterCommand(AddCheckPoint);
            UploadCommand = new RelayCommand(UploadPicture);
            CheckPoints = new ObservableCollection<CheckPoint>(SuggestCheckPoints());
            checkPointsToSave = new List<CheckPoint>();
        }

        private void UploadPicture()
        {
            string imagePath = imageUploaderService.UploadImage();
            if (imagePath != null)
                imagesPath.Add(imagePath);
        }
        private void Save()
        {
            string folderPath = imageUploaderService.CreateTourFolder(imagesPath);

            Tour newTour = new Tour(Name, locationRepository.GetById(LocationId), Description,(LANGUAGE)LanguageId, Capacity, Duration, folderPath, LoggedInUser);
            SavedTour = tourRepository.SaveTour(newTour);
            TourRealisation newTourRealisation = new TourRealisation(DateTime, SavedTour.Id, Capacity, LoggedInUser);
            TourRealisation savedTourRealisation = tourRealisationRepository.SaveTourRealisation(newTourRealisation);
            //proci kroz sve selektovane cp i dodati ih u checkPointsToSave

            checkPointsToSave = CheckPoints.Where(cp => cp.IsChecked == true).ToList();

            checkPointsToSave.ForEach(cp => cp.IsChecked = false);
            checkPointsToSave.ForEach(cp => checkPointRepository.Save(cp));
        }

        private void AddCheckPoint(object parameter)
        {
            string labelText = parameter as string;
            Debug.WriteLine(labelText);
            CheckPoint newCheckPoint = new CheckPoint(labelText, tourRepository.NextIdForTour(), true);
            CheckPoints.Insert(0, newCheckPoint);
        }
        private List<CheckPoint> SuggestCheckPoints()
        {
            var checkPointsInCity = checkPointRepository.GetAll()
                                    .Where(cp => tourRepository.GetTourById(cp.TourId).Location.Id == LocationId)
                                    .DistinctBy(cp => cp.Name)
                                    .ToList();

            checkPointsInCity.ForEach(cp => cp.IsChecked = false);

            return checkPointsInCity;
        }
        private void LocationChanged()
        {
            Debug.WriteLine($"Location changed to {LocationId}");
            CheckPoints.Clear();
            SuggestCheckPoints().ForEach(cp =>  CheckPoints.Add(cp));
        }

    }
}
