using System.Collections.Generic;
using TripPlanner.Business;

namespace TripPlanner.Core
{
    public static class DistanceCalculator
    {
        public static double[,] GetDistances(Hotel _activeHotel, List<Attraction> _attractions)
        {
            double[,] distances = new double[_attractions.Count + 1, _attractions.Count + 1];
            for (int i = 0; i < _attractions.Count + 1; i++)
            {
                for (int j = i; j < _attractions.Count + 1; j++)
                {
                    double distance;
                    if (i == j)
                    {
                        distance = 0;
                    }
                    else if (i == 0)
                    {
                        distance = _activeHotel.GetGeoDistanceToPoint(_attractions[j - 1]);
                    }
                    else
                    {
                        distance = _attractions[i - 1].GetGeoDistanceToPoint(_attractions[j - 1]);
                    }
                    distances[i, j] = distance;
                    distances[j, i] = distance;
                }
            }

            return distances;
        }
    }
}
