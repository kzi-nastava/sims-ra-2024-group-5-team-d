using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace BookingApp.WPF.Views.GuestWindows
{
    /// <summary>
    /// Interaction logic for OwnerAndAccommodationRatingWindow.xaml
    /// </summary>
    public partial class OwnerAndAccommodationRatingWindow : Window
    {
        private int _cleanliness;
        public int Cleanliness
        {
            get { return _cleanliness; }
            set
            {
                if (_cleanliness != value)
                {
                    _cleanliness = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _correctness;
        public int Correctness
        {
            get { return _correctness; }
            set
            {
                if (_correctness != value)
                {
                    _correctness = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _comment;
        public string Comment
        {
            get { return _comment; }
            set
            {
                if (_comment != value)
                {
                    _comment = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _renovation;
        public string Renovation
        {
            get { return _renovation; }
            set
            {
                if (_renovation != value)
                {
                    _renovation = value;
                    OnPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public OwnerAndAccommodationRatingWindow()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
