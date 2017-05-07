using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace TripPlannerLogic
{
    class FileReader
    {

        public int[,] GetDataFromFile(string file, out int numberOfPoints, out int daysOfTrip, out int maxLength, out double[] profits)
        {
            List<RoutePoint> points = new List<RoutePoint>();
            points.Add(new RoutePoint(-1, -1, -1));
            using (StreamReader inFile = new StreamReader(file))
            {
                string[] t = inFile.ReadLine().Split(' ');
                numberOfPoints = Convert.ToInt32(t[0]);
                daysOfTrip = Convert.ToInt32(t[1]);
                maxLength = Convert.ToInt32(t[2]);
                for (int i = 0; i < numberOfPoints + 1; i++)
                {
                    t = inFile.ReadLine().Split(' ');
                    double x, y, p;
                    double.TryParse(t[0], NumberStyles.Any, CultureInfo.InvariantCulture, out x);
                    double.TryParse(t[1], NumberStyles.Any, CultureInfo.InvariantCulture, out y);
                    double.TryParse(t[2], NumberStyles.Any, CultureInfo.InvariantCulture, out p);

                    points.Add(new RoutePoint(x, y, p));
                    if (i == numberOfPoints)
                    {
                        points[0].X = x;
                        points[0].Y = y;
                        points[0].Profit = p;
                    }
                }
            }
            int[,] dist = new int[numberOfPoints + 1, numberOfPoints + 1];
            profits = new double[numberOfPoints + 1];
            for (int i = 0; i < numberOfPoints + 1; i++)
            {
                for (int j = 0; j < numberOfPoints + 1; j++)
                {
                    int distance = points[i].GetEuclideanDistanceToPoint(points[j]);
                    dist[i, j] = distance;
                    dist[j, i] = distance;
                }
                profits[i] = points[i].Profit;
            }
            return dist;
        }
    }
}
