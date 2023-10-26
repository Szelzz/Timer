using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Alarms.UI;

[ValueConversion(typeof(TimeSpan), typeof(Visibility))]
public class TimesUpToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is TimeSpan time && time <= TimeSpan.Zero)
            return Visibility.Visible;

        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
