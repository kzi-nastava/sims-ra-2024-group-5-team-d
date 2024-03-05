using BookingApp.Model;
using BookingApp.Repository;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace BookingApp.View
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
                        GuestWindow guestWindow = new GuestWindow();
                        guestWindow.Show();
                    }
                    else if (user.Type.Equals("owner"))
                    {
                        OwnerWindow ownerWindow = new OwnerWindow();
                        ownerWindow.Show();
                    }
                    else if (user.Type.Equals("tourist"))
                    {
                        TouristWindow touristWindow = new TouristWindow();
                        touristWindow.Show();
                    }
                    else
                    {
                        TouristGuideWindow touristGuideWindow = new TouristGuideWindow(user);
                        touristGuideWindow.Show();
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
