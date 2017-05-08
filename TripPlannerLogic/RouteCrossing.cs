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
            int k = _rand.Next() % 4 + 4;
            _leftIndex = _rand.Next() % (route.Count - 2 - k) + k;
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
                if (!child1.Contains(route2[i]))
                {
                    child1.Add(route2[i]);
                }

                if (!child2.Contains(route1[i]))
                {
                    child2.Add(route1[i]);
                }
            }
            for (int i = _rightIndex; i < (route1.Count < route2.Count ? route2.Count : route1.Count); i++)
            {
                if ((i < route1.Count && !child1.Contains(route1[i])) || i == route1.Count - 1)
                {
                    child1.Add(route1[i]);
                }
                if ((i < route2.Count && !child2.Contains(route2[i])) || i == route2.Count - 1)
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

        /*
        public Route Cross(Route route1, Route route2)
        {
            Route child = new Route();
            int point = route1.Count > route2.Count ? _rand.Next(route2.Count - 2) + 2 : _rand.Next(route1.Count - 2) + 2;
            child.Points.Add(route1[0]);
            child.Length = 0;
            child.Profit = 0;
            bool[] wasUsed = new bool[Params.NumberOfPoints + 1];
            wasUsed[route1[0]] = true;
            for (int i = 1; i < point; i++)
            {
                child.Points.Add(route1[i]);
                child.Profit += Params.Profits[route1[i]];
                child.Length += Params.Distances[child[i - 1], child[i]];
                wasUsed[route1[i]] = true;
            }
            for (int i = point; i < route2.Count - 1; i++)
            {
                if (!wasUsed[route2[i]]
                    && child.Length + Params.Distances[child[child.Count - 1], route2[i]] + Params.Distances[child[0], route2[i]] <= 1.03 * Params.MaxLength)
                {
                    child.Points.Add(route2[i]);
                    child.Profit += Params.Profits[route2[i]];
                    child.Length += Params.Distances[child[child.Count - 1], child[child.Count - 2]];
                }
            }
            child.Points.Add(child[0]);
            child.Length += Params.Distances[child[child.Count - 2], child[child.Count - 1]];


            return child;
        } */
    }
}
