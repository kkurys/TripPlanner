namespace TripPlannerLogic
{
    public static class RouteCalculator
    {
        public static void CalculateRouteProfitAndLength(Route pRoute)
        {
            pRoute.Profit = 0;
            pRoute.Length = 0;
            for (int i = 0; i < pRoute.Count - 1; i++)
            {
                pRoute.Profit += Params.Profits[pRoute[i]];
                pRoute.Length += Params.Distances[pRoute[i], pRoute[i + 1]];
            }
        }
    }
}
