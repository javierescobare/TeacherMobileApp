using System;
using System.Globalization;
using Xamarin.Forms;

namespace TeacherMobileApp.Helpers.Converters
{
    public class CountToIsVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)value) > 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)value) > 0;
        }
    }
}
