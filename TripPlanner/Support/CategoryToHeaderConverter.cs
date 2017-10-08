using System;
using System.Globalization;
using System.Windows.Data;

namespace TripPlanner.Support
{
    public class CategoryToHeaderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string category = value as string;
            if (category == "Hotel")
            {
                return "Hotel";
            }
            else
            {
                return "Atrakcja";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
