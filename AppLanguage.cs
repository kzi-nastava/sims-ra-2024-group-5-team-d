using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp
{
    public class AppLanguage
    {
        public static string CurrentLanguage = "English";
        public static void ChangeLanguage(Uri newLanguage, Uri toRemoveLanguage)
        {
            CurrentLanguage = newLanguage.ToString();
            int startIndex = "Languages/".Length;
            int length = CurrentLanguage.Length - startIndex - ".xaml".Length;
            CurrentLanguage = CurrentLanguage.Substring(startIndex, length);

            ResourceDictionary Language = new ResourceDictionary() { Source = newLanguage };

            ResourceDictionary deleteLanguage = new ResourceDictionary() { Source = toRemoveLanguage };

            App.Current.Resources.MergedDictionaries.Remove(deleteLanguage);

            App.Current.Resources.MergedDictionaries.Add(Language);
        }
    }
}
