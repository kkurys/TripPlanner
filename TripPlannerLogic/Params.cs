using System.Collections.Generic;

namespace TripPlannerLogic
{
    public static class Params
    {
        public static int MaxLength { get; set; }
        public static int DaysOfTrip { get; set; }
        public static int NumberOfPoints { get; set; }
        public static int[] Profits { get; set; }
        public static double[,] Distances { get; set; }

        public static HashSet<int> AvailablePoints { get; set; }

    }
}
