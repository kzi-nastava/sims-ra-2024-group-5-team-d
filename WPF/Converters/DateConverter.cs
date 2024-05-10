using Microsoft.VisualBasic;
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
            string resultDays = " days ago";
            string resultMonths = " months ago";
            string resultYears = " years ago";
            if(value is DateOnly) { 
            value= ((DateOnly)value).ToDateTime(new TimeOnly(0, 0));
            }
            if (value is DateTime )
            {
                DateTime time=(DateTime)value;
                if ((DateTime.Now.Date - time).Days == 0)
                {
                    return "Today";
                }
                else if((DateTime.Now.Date - time).Days == 1)
                {
                    return "Yesterday";
                }
                if (DateTime.Now.Year == time.Year)
                {
                   if ((DateTime.Now.Date - time).Days < 30)
                    {
                        return (DateTime.Now - time).Days + resultDays;
                    }
                    else if (DateTime.Now.Month - time.Month == 1)
                    {
                        return "month ago";
                    }
                    else
                    {
                        return (DateTime.Now.Month - time.Month).ToString() + resultMonths;
                    }
                }
                else 
                {
                    if (DateTime.Now.Year - time.Year == 1)
                        return "year ago";
                    else
                        return (DateTime.Now.Year-time.Year).ToString() + resultYears;
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
