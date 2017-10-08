using System;
using System.Globalization;
using System.Windows.Data;

namespace TripPlanner.Support
{
    public class DayStatusToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool status = (bool)value;
            if (status)
            {
                return "UKRYJ";
            }
            else
            {
                return "POKAŻ";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
