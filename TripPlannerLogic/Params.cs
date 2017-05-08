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
        public static void InitParams(string fileName)
        {
            FileReader fr = new FileReader();
            Distances = fr.GetDataFromFile(fileName, out NumberOfPoints, out DaysOfTrip, out MaxLength, out Profits, out Coordinates);
        }
    }
}
