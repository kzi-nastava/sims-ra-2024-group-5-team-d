using Microsoft.Expression.Interactivity.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp
{
    public class AppTheme
    {
        public static void ChangeTheme(Uri newTheme, Uri toRemoveTheme)
        {
            ResourceDictionary Theme = new ResourceDictionary() { Source = newTheme };

            ResourceDictionary deleteTheme = new ResourceDictionary() { Source = toRemoveTheme };

            App.Current.Resources.MergedDictionaries.Remove(deleteTheme);

            App.Current.Resources.MergedDictionaries.Add(Theme);
        }

    }
}
