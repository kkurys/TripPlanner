﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using TripPlanner.Business;

namespace TripPlanner.Support
{
    public class HotelTypeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Hotel && value == Hotel.ActiveHotel)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
