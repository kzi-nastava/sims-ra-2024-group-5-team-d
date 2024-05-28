using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using BookingApp.WPF.ViewModels;
using BookingApp.WPF.ViewModels.TourViewModels.TourGuideViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace BookingApp.WPF.Views.TouristGuide
{
    /// <summary>
    /// Interaction logic for LiveTourView.xaml
    /// </summary>
    public partial class LiveTourView : UserControl
    {
        public static ListView CheckPointsListView;
        public static ListView TourGuestsListView;
        public static Border SignUpButton;

        public LiveTourView(User user, TourRealisationViewModel tourRealisationViewModel, TourViewModel tourViewModel)
        {
            InitializeComponent();
            DataContext = new TourLiveViewModel(user,tourRealisationViewModel,tourViewModel);
            CheckPointsListView = checkPointsListView;
            TourGuestsListView = tourGuestsListView;
            SignUpButton = signUpButton;
        }


        //KADA SE ZAVRSI TURA CP.ISCHECKED VRATITI SVE NA FALSE ZA NAREDNE REALIZACIJE


    }

}
