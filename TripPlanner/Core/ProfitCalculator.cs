using System.Collections.Generic;
using TripPlanner.Business;

namespace TripPlanner.Core
{
    public static class ProfitCalculator
    {
        public static double[] GetProfits(List<Attraction> _attractions)
        {
            double[] profits = new double[_attractions.Count + 1];
            for (int i = 0; i < _attractions.Count; i++)
            {
                profits[_attractions[i].Id] = _attractions[i].Profit;
            }
            return profits;
        }
    }
}
