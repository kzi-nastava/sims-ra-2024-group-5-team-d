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
        public ICommand CancelCommand { get; set; }
        public ICommand ForwardCommand { get; set; }
        public ICommand GuidesKnowladgeRatingCommand { get; set; }
        public ICommand GuidesLanguageRatingCommand { get; set; }
        public ICommand TourAmusementRatingCommand { get; set; }
        public ICommand SelectionChangedCommand { get; set; }
        public ICommand RateSubmitButtonClickCommand { get; set; }
        public ICommand LiveTourTabCommand { get; private set; }
        public ICommand VouchersTabCommand { get; private set; }

        public User User;
        public RateTourViewModel SelectedItem { get; set; }
        public ObservableCollection<RateTourViewModel> PastTourRealisations { get; set; }

        private TourRatingService tourRatingService;
        private RateTourService tourRateService;
        private TourReservationService tourReservationService;
        private TourService tourService;
        private ImageUploaderService imageUploaderService;

        private List<TourRealisation> pastTourRealisations;
        private List<TourReservation> pastTourReservations;
        private List<Tour> pastTours;
        private List<string> imagesPath;

        private int selectedItemIndex=-1;
        private int paginationIndex = 0;
        private int languageRating;
        private int knowledgeRating;
        private int tourAmusementRating;       

        public PastToursViewModel(User user)
        {
            User = user;
            tourService = new TourService();
            tourRateService = new RateTourService();
            tourReservationService=new TourReservationService();
            imageUploaderService=new ImageUploaderService();
            tourRatingService=new TourRatingService();

            PastTourRealisations = new ObservableCollection<RateTourViewModel>();
            pastTourReservations = tourReservationService.GetPastTourReservationsForTourist(user);
            pastTourRealisations = tourRateService.GetAllPastTourRealisationsForTourist(user);

            imagesPath = new List<string>();
            pastTours = new List<Tour>();
            pastTourRealisations.ForEach(tourRealisation=> pastTours.Add(tourService.GetById(tourRealisation.TourId)));

            for(int i=0;i<pastTourRealisations.Count;i++)
            {
                PastTourRealisations.Add(new RateTourViewModel(pastTourReservations[i].Id, pastTours[i].ImagesPath, pastTours[i].Name, pastTours[i].Location, pastTours[i].Duration,DateOnly.FromDateTime( pastTourRealisations[i].StartTime),TimeOnly.FromDateTime(pastTourRealisations[i].StartTime), TimeOnly.FromDateTime(pastTourRealisations[i].StartTime).AddHours(pastTours[i].Duration)));
            }

            LiveTourTabCommand = new RelayCommand(SwitchToLiveTour);
            VouchersTabCommand = new RelayCommand(SwitchToVouchers);
            RateSubmitButtonClickCommand = new RelayCommand(RateSubmitButtonClick);
            SelectionChangedCommand = new RelayParameterCommand(ListViewSelectionChanged);
            GuidesKnowladgeRatingCommand = new RelayParameterCommand(RateGuideKnowledge);
            GuidesLanguageRatingCommand = new RelayParameterCommand(RateGuideLanguage);
            TourAmusementRatingCommand = new RelayParameterCommand(RateTourAmusement);
            CancelCommand = new RelayCommand(Cancel);
            UploadCommand = new RelayCommand(UploadPicture);
            ForwardCommand=new RelayCommand(Forward);
            BackwardCommand=new RelayCommand(Backward);
        }

        public void Cancel()
        {
            PastTourRealisations[selectedItemIndex].ButtonContent = "Rate";
            PastTourRealisations[selectedItemIndex].Comment = "";
            PastTourRealisations[selectedItemIndex].ImagesPaths.Clear();
            imagesPath.Clear();
            paginationIndex = 0;
            PastTourRealisations[selectedItemIndex].IsRatingEntered = false;
        }

        public void SwitchToLiveTour()
        {
            TouristHomeWindow.contentControl.Content = new YourToursUserControl(User);
        }

        public void SwitchToVouchers()
        {
            TouristHomeWindow.contentControl.Content = new VouchersUserControl(User);
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
        }

        public void ListViewSelectionChanged(object parameter)
        {
            if (parameter != null)
            {            
                SelectedItem = parameter as RateTourViewModel;
            }
        }

        public void RateGuideKnowledge(object parameter)
        {
            if(parameter != null)
            {
                knowledgeRating = Convert.ToInt32(parameter as string);
            }
        }

        public void RateGuideLanguage(object parameter)
        {
            if(parameter != null)
            {
                languageRating = Convert.ToInt32(parameter as string);
            }
        }

        public void RateTourAmusement(object parameter)
        {
            if (parameter != null)
            {
                tourAmusementRating = Convert.ToInt32(parameter as string);
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
            PastTourRealisations[selectedItemIndex].NotRated = false;
            string folderPath = imageUploaderService.CreateTourRateFolder(imagesPath);
            TourRating rating = new TourRating(languageRating, knowledgeRating, tourAmusementRating,PastTourRealisations[selectedItemIndex].ReservationId, folderPath, PastTourRealisations[selectedItemIndex].Comment, true);
            tourRatingService.Save(rating);
        }
    }
}
