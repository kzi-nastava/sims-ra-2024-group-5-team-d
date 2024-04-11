using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace BookingApp.WPF.Converters
{
    public class FirstImagePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string imagesPath && !string.IsNullOrEmpty(imagesPath))
            {
                string[] imagePaths = Directory.GetFiles(imagesPath, "*.*");
                if (imagePaths.Length > 0)
                {
                    return imagePaths[0];
                }
                else
                {
                    return "C:/Users/lukai/Desktop/Resource/house.png";
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
