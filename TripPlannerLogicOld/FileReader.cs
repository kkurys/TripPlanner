using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Genetic_V8
{
    public class FileReader
    {
        public int[,] GetDataFromFile(string file, out int numberOfTowns, out int daysOfTrip, out int maxLength, out double[] profits)
        {
            List<Town> towns = new List<Town>();
            towns.Add(new Town(-1, -1, -1));

            using (StreamReader inFile = new StreamReader(file))
            {
                string[] t = inFile.ReadLine().Split(' ');
                numberOfTowns = Convert.ToInt32(t[0]);
                Parameters.Coordinates = new double[numberOfTowns + 1, 2];
                daysOfTrip = Convert.ToInt32(t[1]);
                maxLength = Convert.ToInt32(t[2]);

                for (int i = 0; i < numberOfTowns; i++)
                {
                    t = inFile.ReadLine().Split(' ');
                    double x, y, p;
                    double.TryParse(t[0], NumberStyles.Any, CultureInfo.InvariantCulture, out x);
                    double.TryParse(t[1], NumberStyles.Any, CultureInfo.InvariantCulture, out y);
                    double.TryParse(t[2], NumberStyles.Any, CultureInfo.InvariantCulture, out p);

                    Parameters.Coordinates[i + 1, 0] = x;
                    Parameters.Coordinates[i + 1, 1] = y;

                    towns.Add(new Town(x, y, p));
                }
                t = inFile.ReadLine().Split(' ');
                double.TryParse(t[0], NumberStyles.Any, CultureInfo.InvariantCulture, out Parameters.Coordinates[0, 0]);
                double.TryParse(t[1], NumberStyles.Any, CultureInfo.InvariantCulture, out Parameters.Coordinates[0, 1]);
                towns[0].x = Parameters.Coordinates[0, 0];
                towns[0].y = Parameters.Coordinates[0, 1];
                towns[0].profit = 0;
            }
            int[,] dist = new int[numberOfTowns + 1, numberOfTowns + 1];
            profits = new double[numberOfTowns + 1];
            for (int i = 0; i < numberOfTowns + 1; i++)
            {
                for (int j = 0; j < numberOfTowns + 1; j++)
                {
                    int distance = towns[i].calculateDistanceToOtherTown(towns[j]);
                    dist[i, j] = distance;
                    dist[j, i] = distance;
                }
                profits[i] = towns[i].profit;
            }
            return dist;
        }
    }
}
