using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp
{
    public class AppLanguage
    {
        public static void ChangeLanguage(Uri newLanguage, Uri toRemoveLanguage)
        {
            ResourceDictionary Language = new ResourceDictionary() { Source = newLanguage };

            ResourceDictionary deleteLanguage = new ResourceDictionary() { Source = toRemoveLanguage };

            App.Current.Resources.MergedDictionaries.Remove(deleteLanguage);

            App.Current.Resources.MergedDictionaries.Add(Language);
        }
    }
}
