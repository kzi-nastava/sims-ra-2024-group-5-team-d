using BookingApp.Domain.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BookingApp.Appl.UseCases
{
    public class SuperGuideService
    {
        private SuperUserService superUserService { get; set; }
        private TourRatingService tourRatingService { get; set; }
        private TourRealisationService tourRealisationService { get; set; }
        public TourReservationService tourReservationService { get; set; }
        private TourService tourService { get; set; }
        private UserService userService {  get; set; }
        public User Guide {  get; set; }
        public SuperGuideService(User user) 
        {
            Guide = user;
            superUserService = new SuperUserService();
            tourRatingService = new TourRatingService();
            tourRealisationService = new TourRealisationService();
            tourService = new TourService();
            tourReservationService = new TourReservationService();
            userService = new UserService();
        }

        public bool IsSuperGuide(User guide)
        {
            (bool has20GuidedTours, LANGUAGE language) = LanguageGuidedTour();
            
            bool hasSufficientRating = IsRatingSufficient(language);
            bool isAlreadyASuperGuide = superUserService.IsAlreadySuperUser(guide);
            Debug.WriteLine(has20GuidedTours.ToString() + hasSufficientRating + isAlreadyASuperGuide);
            if (has20GuidedTours && hasSufficientRating && !isAlreadyASuperGuide)
            {
                SuperUser superGuide = new SuperUser(guide.Id,DateTime.Now,language);
                superUserService.Save(superGuide);
                return true;
            }
            return false;
        }
        public bool ShouldHaveTheSuperStatus(User guide)
        {
            bool status = true;
            SuperUser? superUser = superUserService.GetAll().Find(user => user.UserId == guide.Id);
            if(superUser == null || superUser.ValidFrom.AddYears(1) <= DateTime.Now)
            {
                if (superUser != null)
                {
                    superUserService.Delete(superUser);
                }
                status = IsSuperGuide(guide);
                guide.IsSuperUser = status;
                userService.Update(guide);               
            }

            return status;
            
        }
        public (bool hasTwentyTours, LANGUAGE language) LanguageGuidedTour()
        {
            List<TourRealisation> realisations = tourRealisationService.GetAllTourRealisations().Where(realisation => realisation.User.Id == Guide.Id && realisation.StartTime >= DateTime.Now.AddYears(-1)).ToList();
            Dictionary <LANGUAGE, int> languageCount = new Dictionary<LANGUAGE, int>
            {
                        { LANGUAGE.ENGLISH, 0 },
                        { LANGUAGE.GERMAN, 0 },
                        { LANGUAGE.SERBIAN, 0 }
            };

            foreach (TourRealisation realisation in realisations)
            {
                Tour tour = tourService.GetById(realisation.TourId);
                languageCount[tour.Language]++;
            }
            foreach (var entry in languageCount)
            {
                if (entry.Value >= 20)
                {
                    return (true, entry.Key);
                }
            }

            return (false, default(LANGUAGE));
        }
        public bool IsRatingSufficient(LANGUAGE language)
        {
            double avgRating = 0;
            int counter = 0;
            List<TourRating> ratings = tourRatingService
                                        .RatingsOfGuide(Guide)
                                        .Where(rating =>                                      
                                         tourService.GetById(tourRealisationService.GetById(tourReservationService.GetById(rating.TourReservationId).TourRealisationId).TourId).Language == language)
                                        .ToList();
            ratings.ForEach(rating =>
            {
                avgRating += (rating.TouristLanguage + rating.TouristKnowladge + rating.TourAmusement)/3.0;
                counter++;
            });
            Debug.WriteLine("Prosek: " + (avgRating / (1.0 * counter)));
            return (avgRating / (1.0*counter)) > 4.0;
        }
    }
}
