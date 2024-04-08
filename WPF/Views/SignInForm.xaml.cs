using BookingApp.Domain.Models;
using BookingApp.Repositories;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using BookingApp.WPF.Views.GuestWindows;

namespace BookingApp.WPF.Views
{
    /// <summary>
    /// Interaction logic for SignInForm.xaml
    /// </summary>
    public partial class SignInForm : Window
    {

        private readonly UserRepository _repository;

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                if (value != _username)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SignInForm()
        {
            InitializeComponent();
            DataContext = this;
            _repository = new UserRepository();
        }

        private void SignIn(object sender, RoutedEventArgs e)
        {
            User user = _repository.GetByUsername(Username);
            if (user != null)
            {
                if (user.Password == txtPassword.Password)
                {
                    if (user.Type.Equals("guest"))
                    {
                        GuestWindow guestWindow = new GuestWindow(user);
                        guestWindow.Owner = this;
                        guestWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        guestWindow.ShowDialog();
                    }
                    else if (user.Type.Equals("owner"))
                    {
                        OwnerMainWindow ownerWindow = new OwnerMainWindow(user);
                        ownerWindow.Owner = this;
                        ownerWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        ownerWindow.ShowDialog();
                    }
                    else if (user.Type.Equals("tourist"))
                    {
                        TouristHomeWindow touristWindow = new TouristHomeWindow();
                        touristWindow.Owner = this;
                        touristWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        touristWindow.ShowDialog();
                    }
                    else
                    {
                        TouristGuideWindow touristGuideWindow = new TouristGuideWindow(user);
                        touristGuideWindow.Owner = this;
                        touristGuideWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        touristGuideWindow.ShowDialog();
                    }
                    //                    CommentsOverview commentsOverview = new CommentsOverview(user);
                    //                  commentsOverview.Show();
                    //                    Close();
                }
                else
                {
                    MessageBox.Show("Wrong password!");
                }
            }
            else
            {
                MessageBox.Show("Wrong username!");
            }
            
        }
    }
}
