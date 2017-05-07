using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace TripPlannerLogic
{
    class FileReader
    {
        public double[,] GetDataFromFile(string file, out int numberOfPoints, out int daysOfTrip, out int maxLength, out double[] profits, out double[,] coordinates)
        {
            List<RoutePoint> points = new List<RoutePoint>();
            points.Add(new RoutePoint(-1, -1, -1));
            using (StreamReader inFile = new StreamReader(file))
            {
                string[] t = inFile.ReadLine().Split(' ');
                numberOfPoints = Convert.ToInt32(t[0]);
                daysOfTrip = Convert.ToInt32(t[1]);
                maxLength = Convert.ToInt32(t[2]);

                coordinates = new double[numberOfPoints + 1, 2];

                for (int i = 0; i < numberOfPoints; i++)
                {
                    t = inFile.ReadLine().Split(' ');
                    double x, y, p;
                    double.TryParse(t[0], NumberStyles.Any, CultureInfo.InvariantCulture, out x);
                    double.TryParse(t[1], NumberStyles.Any, CultureInfo.InvariantCulture, out y);
                    double.TryParse(t[2], NumberStyles.Any, CultureInfo.InvariantCulture, out p);

                    coordinates[i + 1, 0] = x;
                    coordinates[i + 1, 1] = y;

                    points.Add(new RoutePoint(x, y, p));
                }
                t = inFile.ReadLine().Split(' ');
                double.TryParse(t[0], NumberStyles.Any, CultureInfo.InvariantCulture, out coordinates[0, 0]);
                double.TryParse(t[1], NumberStyles.Any, CultureInfo.InvariantCulture, out coordinates[0, 1]);
                points[0].X = coordinates[0, 0];
                points[0].Y = coordinates[0, 1];

            }
            double[,] dist = new double[numberOfPoints + 1, numberOfPoints + 1];
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
