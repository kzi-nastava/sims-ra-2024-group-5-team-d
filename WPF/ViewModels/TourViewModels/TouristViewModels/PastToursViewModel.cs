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
using ToastNotifications.Core;

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
            tourRatingService=new TourRatingService();
            imageUploaderService = new ImageUploaderService(tourRatingService);
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
            RateSubmitButtonClickCommand = new RelayParameterCommand(RateSubmitButtonClick);
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
            PastTourRealisations[selectedItemIndex].Language1 = false;
            PastTourRealisations[selectedItemIndex].Language2 = false;
            PastTourRealisations[selectedItemIndex].Language3 = false;
            PastTourRealisations[selectedItemIndex].Language4 = false;
            PastTourRealisations[selectedItemIndex].Language5 = false;
            PastTourRealisations[selectedItemIndex].Knowledge1 = false;
            PastTourRealisations[selectedItemIndex].Knowledge2 = false;
            PastTourRealisations[selectedItemIndex].Knowledge3 = false;
            PastTourRealisations[selectedItemIndex].Knowledge4 = false;
            PastTourRealisations[selectedItemIndex].Knowledge5 = false;
            PastTourRealisations[selectedItemIndex].Amusement1 = false;
            PastTourRealisations[selectedItemIndex].Amusement2 = false;
            PastTourRealisations[selectedItemIndex].Amusement3 = false;
            PastTourRealisations[selectedItemIndex].Amusement4 = false;
            PastTourRealisations[selectedItemIndex].Amusement5 = false;
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

        public void RateSubmitButtonClick(object parameter)
        {            
            if (parameter is RateTourViewModel pastTour)
            {
                if (pastTour.IsRatingEntered)
                {
                    pastTour.IsRatingEntered = false;
                    pastTour.ButtonContent = "Rate";
                    SubmitRating();
                    pastTour.IsRated = true;
                }
                else
                {
                    pastTour.IsRatingEntered = true;
                    pastTour.ButtonContent = "Submit";
                    selectedItemIndex = PastTourRealisations.IndexOf(pastTour);
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
                PastTourRealisations[selectedItemIndex].Knowledge1 = knowledgeRating == 1;
                PastTourRealisations[selectedItemIndex].Knowledge2 = knowledgeRating == 2;
                PastTourRealisations[selectedItemIndex].Knowledge3 = knowledgeRating == 3;
                PastTourRealisations[selectedItemIndex].Knowledge4 = knowledgeRating == 4;
                PastTourRealisations[selectedItemIndex].Knowledge5 = knowledgeRating == 5;
            }
        }

        public void RateGuideLanguage(object parameter)
        {
            if (parameter != null)
            {
                int languageRating = Convert.ToInt32(parameter as string);

                PastTourRealisations[selectedItemIndex].Language1 = languageRating == 1;
                PastTourRealisations[selectedItemIndex].Language2 = languageRating == 2;
                PastTourRealisations[selectedItemIndex].Language3 = languageRating == 3;
                PastTourRealisations[selectedItemIndex].Language4 = languageRating == 4;
                PastTourRealisations[selectedItemIndex].Language5 = languageRating == 5;
            }
        }

        public void RateTourAmusement(object parameter)
        {
            if (parameter != null)
            {
                int tourAmusementRating = Convert.ToInt32(parameter as string);

                PastTourRealisations[selectedItemIndex].Amusement1 = tourAmusementRating == 1;
                PastTourRealisations[selectedItemIndex].Amusement2 = tourAmusementRating == 2;
                PastTourRealisations[selectedItemIndex].Amusement3 = tourAmusementRating == 3;
                PastTourRealisations[selectedItemIndex].Amusement4 = tourAmusementRating == 4;
                PastTourRealisations[selectedItemIndex].Amusement5 = tourAmusementRating == 5;
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
            if (paginationIndex > 2)
            {
                paginationIndex -= 3;
                PastTourRealisations[selectedItemIndex].ImagesPaths.Clear();
                for (int i = paginationIndex; i < imagesPath.Count; i++)
                {

                    PastTourRealisations[selectedItemIndex].ImagesPaths.Add(imagesPath[i]);
                    if (PastTourRealisations[selectedItemIndex].ImagesPaths.Count == 3)
                        break;
                }
            }
               
        }
        public void Forward()
        {
            if (paginationIndex + 3 < imagesPath.Count)
            {
                paginationIndex += 3;
                PastTourRealisations[selectedItemIndex].ImagesPaths.Clear();
                for (int i = paginationIndex; i < imagesPath.Count; i++)
                {
                    PastTourRealisations[selectedItemIndex].ImagesPaths.Add(imagesPath[i]);
                    if (PastTourRealisations[selectedItemIndex].ImagesPaths.Count == 3)
                        break;
                }
            }
            
        }

        public void SubmitRating()
        {
            PastTourRealisations[selectedItemIndex].Rating = (double)(languageRating + knowledgeRating + tourAmusementRating) / 3;
            PastTourRealisations[selectedItemIndex].SetStarPaths();
            PastTourRealisations[selectedItemIndex].NotRated = false;
            PastTourRealisations[selectedItemIndex].IsRated = true;
            string folderPath = imageUploaderService.CreateTourRateFolder(imagesPath);
            TourRating rating = new TourRating(languageRating, knowledgeRating, tourAmusementRating,PastTourRealisations[selectedItemIndex].ReservationId, folderPath, PastTourRealisations[selectedItemIndex].Comment, true);
            tourRatingService.Save(rating);
        }
    }
}
