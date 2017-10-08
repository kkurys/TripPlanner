using System;
using System.Globalization;
using System.Windows.Data;

namespace TripPlanner.Support
{
    public class DayToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int day = (int)value;

            try
            {
                return DayColors.GetColor(day);
            }
            catch
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
