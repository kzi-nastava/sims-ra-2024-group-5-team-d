using Microsoft.Expression.Interactivity.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp
{
    public class AppTheme
    {
        public static string CurrentTheme { get; set; } = "LightTheme";
        public static void ChangeTheme(Uri newTheme, Uri toRemoveTheme)
        {
            CurrentTheme = newTheme.ToString();
            int startIndex = "Themes/".Length;
            int length = CurrentTheme.Length - startIndex - ".xaml".Length;
            CurrentTheme = CurrentTheme.Substring(startIndex, length);
            Debug.WriteLine(CurrentTheme);

            ResourceDictionary Theme = new ResourceDictionary() { Source = newTheme };

            ResourceDictionary deleteTheme = new ResourceDictionary() { Source = toRemoveTheme };

            App.Current.Resources.MergedDictionaries.Remove(deleteTheme);

            App.Current.Resources.MergedDictionaries.Add(Theme);
        }

    }
}
