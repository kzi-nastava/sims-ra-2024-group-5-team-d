using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO
{
    public class GuestRatingDTO : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private int accommodationId;
        public int AccommodatioId
        {
            get
            {
                return accommodationId;
            }
            set
            {
                if (value != accommodationId)
                {
                    accommodationId = value;
                    OnPropertyChanged("AccommodatioId");
                }
            }
        }
        private int guestId;
        public int GuestId
        {
            get
            {
                return guestId;
            }
            set
            {
                if (value != guestId)
                {
                    guestId = value;
                    OnPropertyChanged("GuestId");
                }
            }
        }
        private int reservationId;
        public int ReservationId
        {
            get
            {
                return reservationId;
            }
            set
            {
                if (value != reservationId)
                {
                    reservationId = value;
                    OnPropertyChanged("ReservationId");
                }
            }
        }

        private string accommodationName;
        public string AccommodationName
        {
            get
            {
                return accommodationName;
            }
            set
            {
                if (value != accommodationName)
                {
                    accommodationName = value;
                    OnPropertyChanged("AccommodationName");
                }
            }
        }
        private DateTime reservedFrom;
        public DateTime ReservedFrom
        {
            get
            {
                return reservedFrom;
            }
            set
            {
                if (value != reservedFrom)
                {
                    reservedFrom = value;
                    OnPropertyChanged("ReservedFrom");
                }
            }
        }
        private DateTime reservedTo;
        public DateTime ReservedTo
        {
            get
            {
                return reservedTo;
            }
            set
            {
                if (value != reservedTo)
                {
                    reservedTo = value;
                    OnPropertyChanged("ReservedTo");
                }
            }
        }
        public GuestRatingDTO()
        {
        }

        public GuestRatingDTO(string accommodationName, DateTime reservedFrom, DateTime reservedTo,int guestId,int reservationId,int accommodationId)
        {

            GuestId = guestId;
            ReservationId = reservationId;
            AccommodatioId = accommodationId;
            AccommodationName = accommodationName;
            ReservedFrom = reservedFrom;
            ReservedTo = reservedTo;

        }

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
