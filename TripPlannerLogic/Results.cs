using System.Collections.Generic;

namespace TripPlannerLogic
{
    public static class Results
    {
        public static Route CurrentBestOne { get; set; }
        public static List<Route> Solutions { get; set; } = new List<Route>();
        public static double TotalProfit { get; set; }
        public static double TotalLength { get; set; }
    }
}
