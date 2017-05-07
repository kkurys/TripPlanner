using System;
using System.Collections.Generic;

namespace TripPlannerLogic
{
    public class RouteCrossing
    {
        Random _rand;
        public RouteCrossing()
        {
            _rand = new Random();
        }
        //  2-point crossover
        public Route Cross(Route route1, Route route2)
        {
            Route route = route1.Count > route2.Count ? route2 : route1;
            Route newRoute = new Route();
            int _leftIndex, _rightIndex;
            _leftIndex = _rand.Next() % (route.Count - 2);
            _rightIndex = _rand.Next() % (route.Count - _leftIndex) + _leftIndex;

            List<int> child1 = new List<int>();
            List<int> child2 = new List<int>();
            for (int i = 0; i < _leftIndex; i++)
            {
                child1.Add(route1[i]);
                child2.Add(route2[i]);
            }
            for (int i = _leftIndex; i < _rightIndex; i++)
            {
                child1.Add(route2[i]);
                child2.Add(route1[i]);
            }
            for (int i = _rightIndex; i < (route1.Count < route2.Count ? route2.Count : route1.Count); i++)
            {
                if (i < route1.Count && !child1.Contains(route1[i]) || i == route1.Count - 1)
                {
                    child1.Add(route1[i]);
                }
                if (i < route2.Count && !child2.Contains(route2[i]) || i == route2.Count - 1)
                {
                    child2.Add(route2[i]);
                }
            }
            if (_rand.Next() % 2 == 1)
            {
                newRoute.Points = child1;
            }
            else
            {
                newRoute.Points = child2;
            }
            RouteCalculator.CalculateRouteProfitAndLength(newRoute);
            return newRoute;
        }
    }
}
