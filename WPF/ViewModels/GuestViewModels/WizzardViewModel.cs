using BookingApp.WPF.Commands;
using BookingApp.WPF.Views.GuestWindows;
using LiveCharts.Wpf.Charts.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels.GuestViewModels
    {
        public class WizzardViewModel : INotifyPropertyChanged
        {
            public ICommand BackCommand { get; set; }
            public ICommand CloseCommand { get; set; }
            public ICommand NextCommand { get; set; }

            private bool canGoBack;
            public bool CanGoBack
            {
            get => canGoBack;
            set
            {
                if (value != canGoBack)
                {
                    canGoBack = value;
                    OnPropertyChanged(nameof(canGoBack));
                }
            }
            }
            private bool canGoNext;
            public bool CanGoNext
            {
                 get => canGoNext;
                 set
                 {
                     if (value != canGoNext)
                     {
                         canGoNext = value;
                         OnPropertyChanged(nameof(canGoNext));
                     }
                 }
            }
            public List<string> ImagesPath { get; set; }

            private int currentImageIndex = 0;

            private string imagePath;
            public string ImagePath
            {
                get => imagePath;
                set
                {
                    if (value != imagePath)
                    {
                        imagePath = value;
                        OnPropertyChanged(nameof(imagePath));
                    }
                }
            }


        public WizzardWindow WizzardWindow { get; set; }

        public WizzardViewModel(WizzardWindow wizzardWindow)
        {
            BackCommand = new RelayCommand(Back);
            CloseCommand = new RelayCommand(Close);
            NextCommand = new RelayCommand(Next);
            WizzardWindow = wizzardWindow;
            ImagesPath = new List<string>();

            ImagesPath.Add("../../../Resources/WizzardImages/First.png");
            ImagesPath.Add("../../../Resources/WizzardImages/Search.png");
            ImagesPath.Add("../../../Resources/WizzardImages/Accommodation.png");
            ImagesPath.Add("../../../Resources/WizzardImages/Reservations.png");
            ImagesPath.Add("../../../Resources/WizzardImages/Requests.png");
            ImagesPath.Add("../../../Resources/WizzardImages/Forum.png");
            ImagesPath.Add("../../../Resources/WizzardImages/Profile.png");

            ImagePath = ImagesPath[0]; 
            CanGoNext = true;
            CanGoBack = false;
        }

        private void Next()
        {
            if (ImagesPath.Count()>currentImageIndex)
            {
                CanGoBack = true;
                currentImageIndex++;
                ImagePath = ImagesPath[currentImageIndex];

            }
            if (ImagesPath.Count()-1 == currentImageIndex)
            {
                CanGoNext = false;
            }

        }

        private void Close()
        {
                WizzardWindow.Close();
        }

        private void Back()
        {
            if (currentImageIndex!=0)
            {
                CanGoBack = true;
                CanGoNext = true;
                currentImageIndex--;
                ImagePath = ImagesPath[currentImageIndex];
            }
            if (currentImageIndex == 0)
            {
                CanGoBack = false;
            }
        }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
}

