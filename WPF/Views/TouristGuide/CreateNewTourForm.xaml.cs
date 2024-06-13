using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.ViewModels.TourViewModels.TourGuideViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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
using Xceed.Wpf.Toolkit;

namespace BookingApp.WPF.Views.TouristGuide
{
    /// <summary>
    /// Interaction logic for CreateNewTourForm.xaml
    /// </summary>
    public partial class CreateNewTourForm : UserControl
    {
        public static ComboBox LanguageComboBox { get; set; }
        public static ComboBox LocationComboBox { get; set; }
        public static TextBox CheckBoxInput { get; set; }
        public static DateTimePicker DateTimePicker { get; set; }
        public static TextBox NameInput {  get; set; }
        public static TextBox DurationInput {  get; set; }
        public static TextBox CapacityInput {  get; set; }
        public static TextBox DescriptionInput {  get; set; }
        public static Border ImageUploadButton {  get; set; }
        public static Border CheckPointAddButton {  get; set; }
        public User LoggedInUser { get; set; }
        public CreateNewTourForm(User user)
        {
            InitializeComponent();
            LoggedInUser = user;
            DataContext = new CreateTourViewModel(LoggedInUser);
            LanguageComboBox = languageComboBox;
            LocationComboBox = locationComboBox;
            CheckBoxInput = itemInput;
            DateTimePicker = dateTimePicker;
            NameInput = nameInput;
            DurationInput = durationInput;
            CapacityInput = capacityInput;
            DescriptionInput = descriptionInput;
            ImageUploadButton = imageUploadButton;
            CheckPointAddButton = checkPointAddButton;
        }
        public CreateNewTourForm(User user, RequestViewModel request)
        {
            InitializeComponent();
            LoggedInUser = user;
            DataContext = new CreateTourViewModel(LoggedInUser,request);

        }
        public CreateNewTourForm(User user, int locationId,int languageId)
        {
            InitializeComponent();
            LoggedInUser = user;
            DataContext = new CreateTourViewModel(LoggedInUser, locationId, languageId);

        }
    }
}
