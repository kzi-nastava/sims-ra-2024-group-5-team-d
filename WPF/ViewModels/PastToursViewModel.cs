using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.Commands;
using BookingApp.WPF.Views.TouristGuide;
using BookingApp.WPF.Views.TouristView;
using Microsoft.Expression.Interactivity.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels
{
    public class PastToursViewModel
    {
        public ICommand UploadCommand { get; set; }
        public ICommand BackwardCommand { get; set; }
        public ICommand ForwardCommand { get; set; }
        public ICommand GuidesKnowladgeRatingCommand { get; set; }
        public RateTourViewModel SelectedItem { get; set; }
        public ICommand SelectionChangedCommand { get; set; }
        public ICommand RateSubmitButtonClickCommand { get; set; }
        public ICommand LiveTourTabCommand { get; private set; }
        public User User;
        public ObservableCollection<RateTourViewModel> PastTourRealisations { get; set; }
        private RateTourService tourRateService;
        private List<TourRealisation> pastTourRealisations;
        private List<TourReservation> pastTourReservations;
        private TourReservationService tourReservationService;
        private TourRatingService tourRatingService;
        private List<string> imagesPath;
        private ImageUploaderService imageUploaderService;
        private List<Tour> pastTours;
        private ITourRepository tourRepository;
        private int selectedItemIndex=-1;
        private int paginationIndex = 0;
        private int languageRating;
        private int knowledgeRating;
        private int tourAmusementRating;
        public PastToursViewModel(User user)
        {

            tourRepository = Injector.CreateInstance<ITourRepository>();
            tourRateService = new RateTourService();
            pastTours = new List<Tour>();
            tourReservationService=new TourReservationService();
            imageUploaderService=new ImageUploaderService();
            tourRatingService=new TourRatingService();
            PastTourRealisations = new ObservableCollection<RateTourViewModel>();
            pastTourReservations = tourReservationService.GetPastTourReservationsForTourist(user);
            pastTourRealisations = tourRateService.GetAllPastTourRealisationsForTourist(user);
            pastTourRealisations.ForEach(tourRealisation=> pastTours.Add(tourRepository.GetTourById(tourRealisation.TourId)));
            for(int i=0;i<pastTourRealisations.Count;i++)
            {
                PastTourRealisations.Add(new RateTourViewModel(pastTourReservations[i].Id, pastTours[i].ImagesPath, pastTours[i].Name, pastTours[i].Location, pastTours[i].Duration,DateOnly.FromDateTime( pastTourRealisations[i].StartTime),TimeOnly.FromDateTime(pastTourRealisations[i].StartTime), TimeOnly.FromDateTime(pastTourRealisations[i].StartTime).AddHours(pastTours[i].Duration)));
            }
            Debug.WriteLine("aaaaa dsad  " + PastTourRealisations.Count);
            imagesPath = new List<string>();
            LiveTourTabCommand = new RelayCommand(SwitchToLiveTour);
            RateSubmitButtonClickCommand = new RelayCommand(RateSubmitButtonClick);
            SelectionChangedCommand = new RelayParameterCommand(ListViewSelectionChanged);
            GuidesKnowladgeRatingCommand = new RelayParameterCommand(RateGuideKnowledge);
            UploadCommand = new RelayCommand(UploadPicture);
            ForwardCommand=new RelayCommand(Forward);
            BackwardCommand=new RelayCommand(Backward);
            User = user;
        }


        public void SwitchToLiveTour()
        {
            TouristHomeWindow.contentControl.Content = new YourToursUserControl(User);
        }

        public void RateSubmitButtonClick()
        {
            if(SelectedItem != null)
            {
                for(int i = 0; i < PastTourRealisations.Count; i++)
                {
                    if(SelectedItem.ReservationId == PastTourRealisations[i].ReservationId)
                    {
                        if (!PastTourRealisations[i].IsRatingEntered)
                        {
                            PastTourRealisations[i].IsRatingEntered = true;
                            PastTourRealisations[i].ButtonContent = "Sumbit";
                            selectedItemIndex = i;
                        }
                        else
                        {
                            PastTourRealisations[i].IsRatingEntered = false;
                            PastTourRealisations[i].ButtonContent = "Rate";
                            SubmitRating();

                        }
                    }
                }
            }
            Debug.WriteLine("Ubica je lep");
        }

        public void ListViewSelectionChanged(object parameter)
        {
            if (parameter != null)
            {            
                SelectedItem = parameter as RateTourViewModel;
                Debug.WriteLine(SelectedItem.ReservationId);
            }
        }

        public void RateGuideKnowledge(object parameter)
        {
            if(parameter != null)
            {
                Debug.WriteLine(parameter as string);
            }
        }
        private void UploadPicture()
        {
            string imagePath = imageUploaderService.UploadImage();
            if (imagePath != null)
            {
                imagesPath.Add(imagePath);
                PastTourRealisations[selectedItemIndex].ImagesPaths.Add(imagePath);
                if(imagesPath.Count>1)
                if (PastTourRealisations[selectedItemIndex].ImagesPaths.Count % 3 == 1)
                {
                    Forward();
                }
            }
        }
        public void Backward()
        {
            paginationIndex-=3;
            PastTourRealisations[selectedItemIndex].ImagesPaths.Clear();
            for(int i =paginationIndex; i<imagesPath.Count;i++)
            {
                    PastTourRealisations[selectedItemIndex].ImagesPaths.Add(imagesPath[i]);
                if (PastTourRealisations[selectedItemIndex].ImagesPaths.Count == 3)
                    break;

            }
        }
        public void Forward()
        {
            paginationIndex+=3;
            PastTourRealisations[selectedItemIndex].ImagesPaths.Clear();
            for (int i = paginationIndex; i < imagesPath.Count; i++)
            {
                    PastTourRealisations[selectedItemIndex].ImagesPaths.Add(imagesPath[i]);
                if (PastTourRealisations[selectedItemIndex].ImagesPaths.Count ==3)
                    break;

            }
        }

        public void SubmitRating()
        {
            string folderPath = imageUploaderService.CreateTourRateFolder(imagesPath);
            TourRating rating = new TourRating(languageRating, knowledgeRating, tourAmusementRating,PastTourRealisations[selectedItemIndex].ReservationId, folderPath, PastTourRealisations[selectedItemIndex].Comment, true);
            tourRatingService.Save(rating);
        }
    }
}
