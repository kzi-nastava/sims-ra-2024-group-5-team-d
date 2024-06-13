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
            if (value is string path && !string.IsNullOrEmpty(path))
            {
                if (Directory.Exists(path))
                {
                    string[] imagePaths = Directory.GetFiles(path, "*.*");
                    if (imagePaths.Length > 0)
                    {
                        return imagePaths[0]; // Vrati prvu sliku iz foldera
                    }
                    else
                    {
                        return "C:/Users/lukai/Desktop/Resource/house.png"; // Ako folder ne sadrži slike, vrati podrazumevanu sliku
                    }
                }
                else if (File.Exists(path))
                {
                    return path; // Ako je putanja do slike, vrati tu putanju
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
