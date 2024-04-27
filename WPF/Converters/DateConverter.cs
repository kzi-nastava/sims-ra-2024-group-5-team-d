using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BookingApp.WPF.Converters
{
    internal class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime)
            {
                DateTime time=(DateTime)value;
                if ((DateTime.Now.Date-time).Days<30)
                {       
                    return (DateTime.Now-time).Days;
                }
                else if ((DateTime.Now.Date-time).Days<365)
                {
                    return ((DateTime.Now-time).Days/30).ToString();
                }
                else if((DateTime.Now.Date-time).Days>365)
                {
                    return ((DateTime.Now-time).Days/365).ToString();
                }
                else
                {
                    return ((DateTime)value).ToString("dd/MM/yyyy HH:mm");
                }
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
