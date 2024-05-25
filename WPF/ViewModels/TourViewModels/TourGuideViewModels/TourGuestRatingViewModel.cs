using BookingApp.WPF.ViewModels.TourViewModels.TourGuideViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels
{
    public class TourGuestRatingViewModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public string JoinedOn { get; set; }
        public DateTime TourRealisationStartTime { get; set; }
        public int LanguageRate { get; set; } 
        public int KnowledgeRate { get; set; }
        public int AmusementRate { get; set; }
        public string Comment { get; set; }
        public StarRatingViewModel starRatingLanguage { get; set; }
        public StarRatingViewModel starRatingKnowledge { get; set; }
        public StarRatingViewModel starRatingAmusement { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool isNotValid;
        public bool IsNotValid
        {
            get => isNotValid;
            set
            {
                if (value != isNotValid)
                {
                    isNotValid = value;
                    OnPropertyChanged();
                }
            }
        }

        public TourGuestRatingViewModel() { }
        public TourGuestRatingViewModel(int id,string fullName, int age, string joinedOn, DateTime tourRealisationStartTime, int languageRate, int knowledgeRate, int amusementRate, string comment, bool isValid)
        {
            Id = id;
            FullName = fullName;
            Age = age;
            JoinedOn = joinedOn;
            TourRealisationStartTime = tourRealisationStartTime;
            LanguageRate = languageRate;
            KnowledgeRate = knowledgeRate;
            AmusementRate = amusementRate;
            Comment = comment;
            IsNotValid = !isValid;
            starRatingLanguage = new StarRatingViewModel(languageRate);
            starRatingKnowledge = new StarRatingViewModel(knowledgeRate);
            starRatingAmusement = new StarRatingViewModel(amusementRate);
        }
    }
}
