using System.Collections.Generic;

namespace TripPlannerLogic
{
    public static class Params
    {
        public static int MaxLength;
        public static int DaysOfTrip;
        public static int NumberOfPoints;
        public static double[] Profits;
        public static double[,] Distances;
        public static double[,] Coordinates;
        public static HashSet<int> AvailablePoints { get; set; }
        public static void InitParams(string fileName)
        {
            FileReader fr = new FileReader();
            Distances = fr.GetDataFromFile(fileName, out NumberOfPoints, out DaysOfTrip, out MaxLength, out Profits, out Coordinates);
            AvailablePoints = new HashSet<int>();
            for (int i = 1; i < NumberOfPoints; i++)
            {
                AvailablePoints.Add(i);
            }
        }

    }
}
