using System.Windows.Media;

namespace TripPlanner.Support
{
    public static class DayColors
    {
        public static Brush GetColor(int day)
        {
            switch (day)
            {
                case 1:
                    return Brushes.Red;
                case 2:
                    return Brushes.Blue;
                case 3:
                    return Brushes.DarkViolet;
                case 4:
                    return Brushes.Brown;
                case 5:
                    return Brushes.Green;
                case 6:
                    return Brushes.DarkOrange;
                case 7:
                    return Brushes.DarkRed;
                case 8:
                    return Brushes.DarkSeaGreen;
                case -1:
                    return Brushes.Goldenrod;
                default:
                    return Brushes.Black;
            }


        }
    }
}
