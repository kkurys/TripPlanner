using System;
using System.Globalization;
using System.Windows.Data;

namespace TripPlanner.Support
{
    public class CategoryToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string category = value as string;
            if (category != null)
            {
                switch (category)
                {
                    case "Hotel":
                        return "/Icons/house.png";
                    case "Muzea i skanseny":
                        return "/Icons/museum.png";
                    case "Jeziora, wodospady i rzeki":
                        return "/Icons/sea.png";
                    case "Obiekty sakralne i cmentarze":
                        return "/Icons/church.png";
                    case "Zamki i twierdze":
                        return "/Icons/fortress.png";
                    case "Rekreacja ":
                        return "/Icons/recreation.png";
                    case "Pomniki, rzeźby i ławeczki":
                        return "/Icons/sculpture.png";
                    case "Pałace i dworki":
                        return "/Icons/palace.png";
                    case "Szczyty i punkty widokowe":
                        return "/Icons/mountains.png";
                    case "Rynki, place i deptaki":
                        return "/Icons/fair.png";
                    case "Szlaki turystyczne":
                        return "/Icons/trail.png";
                    case "Skałki":
                        return "/Icons/rocks.png";
                    case "Imprezy":
                        return "/Icons/party.png";
                    case "Karczmy i restauracje":
                        return "/Icons/restaurant.png";
                    case "Parki rozrywki i zoo":
                        return "/Icons/park.png";
                    case "Fontanny":
                        return "/Icons/fountain.png";
                    case "Mury miejskie":
                        return "/Icons/wall.png";
                    case "Ratusze":
                        return "/Icons/hall.png";
                    case "Parki Narodowe i rezerwaty":
                        return "/Icons/tree.png";
                    case "Uzdrowiska, parki zdrojowe":
                        return "/Icons/spa.png";
                    case "Centra nauki":
                        return "/Icons/science.png";
                    default:
                        return "/Icons/other.png";
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
