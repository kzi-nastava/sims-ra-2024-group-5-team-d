using BookingApp.WPF.Views.TouristView;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace BookingApp.WPF.Converters
{
    public class BoolToColorConverter :IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool booleanValue)
            {
                if (AppTheme.CurrentTheme.Equals("DarkTheme")){
                    var trueColor1 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#211F1F"));
                    var falseColor1 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#344955"));
                    return booleanValue ? trueColor1 : falseColor1;
                }
                var trueColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#218C89"));
                var falseColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2FC8BF"));
                return booleanValue ? trueColor : falseColor;
                
            }
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2FC8BF"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
